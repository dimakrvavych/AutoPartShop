using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPartShop
{
    public partial class Cashier : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string stitle = "AutoPartsShop";

        int qty;
        string id;
        string price;


        public Cashier()
        {
            InitializeComponent();
            db.GetConnection();
            GetTranNo();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Вийти з додатку?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        public void slide(Button button)
        {
            panelSlide.BackColor = Color.White;
            panelSlide.Height = button.Height;
            panelSlide.Top = button.Top;
        }
        #region button
        private void btnNTran_Click(object sender, EventArgs e)
        {
            slide(btnNTran);
            GetTranNo();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            slide(btnSearch);
            LookUpAutoParts lookUp = new LookUpAutoParts(this);
            lookUp.LoadAutoparts();
            lookUp.ShowDialog();
        }

        private void btnSettle_Click(object sender, EventArgs e)
        {
            slide(btnSettle);
            Settle settle = new Settle(this);
            settle.txtSale.Text = lblDisplayTotal.Text;
            settle.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            slide(btnClear);
            if (MessageBox.Show("Видалити всі предмети з кошика?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.openConnection();
                cm = new SqlCommand("DELETE FROM Cart WHERE Transno LIKE '" + lblTranNo.Text + "'", db.GetConnection());
                cm.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show("Усі предмети були успішно видалені", "Видалення предметів", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
            if(dgvCash.Rows.Count > 0)
            {
                MessageBox.Show("Неможливо вийти. Будь ласка, скасуйте транзакцію.", "Попердження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Вийти з системи?", "Вихід з системи", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmLogin login = new frmLogin();
                login.ShowDialog();
            }
        }
        #endregion button
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public void LoadCart()
        {
            try
            {
                Boolean hascart = false;
                int i = 0;
                double total = 0;
                dgvCash.Rows.Clear();
                db.openConnection();
                cm = new SqlCommand("SELECT c.ID, c.PartNumber, a.PartName, c.Price, c.QuantityInStock, c.Total FROM Cart AS c INNER JOIN AutoParts AS a ON c.PartNumber=a.PartNumber WHERE c.Transno LIKE @Transno and c.Status LIKE 'Pending'", db.GetConnection());
                cm.Parameters.AddWithValue("@Transno", lblTranNo.Text);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Convert.ToDouble(dr["Total"].ToString());
                    dgvCash.Rows.Add(i, dr["ID"].ToString(), dr["PartNumber"].ToString(), dr["PartName"].ToString(), dr["Price"].ToString(), dr["QuantityInStock"].ToString(), double.Parse(dr["Total"].ToString()).ToString("#,##0.00"));
                    hascart = true;
                }
                dr.Close();
                db.closeConnection();
                lblSaleTotal.Text = total.ToString("#,##0.00");
                GetCartTotal();
                if (hascart) { btnClear.Enabled = true; btnSettle.Enabled = true; }
                else { btnClear.Enabled = false; btnSettle.Enabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
        }

        public void GetCartTotal()
        {
            double sales = double.Parse(lblSaleTotal.Text);
            lblDisplayTotal.Text = sales.ToString("#,##0.00");
        }

        public void GetTranNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;
                db.openConnection();
                cm = new SqlCommand("SELECT TOP 1 Transno FROM Cart WHERE Transno LIKE '" + sdate + "%' ORDER BY ID desc", db.GetConnection());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTranNo.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTranNo.Text = transno;
                }
                dr.Close();
                db.closeConnection();
            }
            catch (Exception ex)
            {
                db.closeConnection();
                MessageBox.Show(ex.Message, stitle);
            }
            
        }

        private void txtPartnumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPartnumber.Text == string.Empty) return;
                else
                {
                    string _partnumber;
                    double _price;
                    int _qty;

                    db.openConnection();
                    cm = new SqlCommand("SELECT * FROM AutoParts WHERE PartNumber LIKE '" + txtPartnumber.Text + "'", db.GetConnection());
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["QuantityInStock"].ToString());
                        _partnumber = dr["PartNumber"].ToString();
                        _price = double.Parse(dr["Price"].ToString());
                        _qty = int.Parse(txtQty.Text);
                        dr.Close();
                        db.closeConnection();
                        AddToCart(_partnumber, _price, _qty);
                    }
                    dr.Close();
                    db.closeConnection();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void AddToCart(string _partnumber, double _price, int _qty)
        {
            try
            {
                string id = "";
                int cart_qty = 0;
                bool found = false;
                db.openConnection();
                cm = new SqlCommand("SELECT * FROM Cart WHERE Transno = @Transno and PartNumber = @PartNumber", db.GetConnection());
                cm.Parameters.AddWithValue("@Transno", lblTranNo.Text);
                cm.Parameters.AddWithValue("@PartNumber", _partnumber);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    id = dr["ID"].ToString();
                    cart_qty = int.Parse(dr["QuantityInStock"].ToString());
                    found = true;
                }
                else found = false;
                dr.Close();
                db.closeConnection();

                if (found)
                {
                    if (qty < (int.Parse(txtQty.Text)+ cart_qty)) 
                    {
                        MessageBox.Show("Неможливо продовжити. Залишилася кількість на руках " + qty, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    db.openConnection();
                    cm = new SqlCommand("UPDATE Cart SET QuantityInStock = (QuantityInStock + " + _qty + ") WHERE ID = '" + id + "'", db.GetConnection());
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    txtPartnumber.SelectionStart = 0;
                    txtPartnumber.SelectionLength = txtPartnumber.Text.Length;
                    LoadCart();
                }
                else
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Неможливо продовжити. Залишилася кількість на руках " + qty, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    db.openConnection();
                    cm = new SqlCommand("INSERT INTO Cart (Transno, PartNumber, Price, QuantityInStock, sdate, Cashier) VALUES (@Transno, @PartNumber, @Price, @QuantityInStock, @sdate, @Cashier)", db.GetConnection());
                    cm.Parameters.AddWithValue("@Transno", lblTranNo.Text);
                    cm.Parameters.AddWithValue("@PartNumber", _partnumber);
                    cm.Parameters.AddWithValue("@Price", _price);
                    cm.Parameters.AddWithValue("@QuantityInStock", _qty);
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@Cashier", lblUsername.Text);
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    LoadCart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Видалити цей предмет?", "Видалення предмета", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.ExecuteQuery("DELETE FROM Cart WHERE ID LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Предмет були успішно видалено", "Видалення предмета", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
            else if(colName == "colAdd")
            {
                int i = 0;
                db.openConnection();
                cm = new SqlCommand("SELECT SUM(QuantityInStock) AS QuantityInStock FROM AutoParts WHERE PartNumber LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY PartNumber", db.GetConnection());
                i = int.Parse(cm.ExecuteScalar().ToString());
                db.closeConnection();

                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[5].Value.ToString()) < i) 
                { 
                    db.ExecuteQuery("UPDATE Cart SET QuantityInStock = QuantityInStock + " + int.Parse(txtQty.Text) + "WHERE Transno LIKE '" + lblTranNo.Text + "'  AND PartNumber LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Залишилася кількість на руках " + i + "!", "Немає в наявності", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "colReduce")
            {
                int i = 0;
                db.openConnection();
                cm = new SqlCommand("SELECT SUM(QuantityInStock) AS QuantityInStock FROM Cart WHERE PartNumber LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY PartNumber", db.GetConnection());
                i = int.Parse(cm.ExecuteScalar().ToString());
                db.closeConnection();

                if (i > 1)
                {
                    db.ExecuteQuery("UPDATE Cart SET QuantityInStock = QuantityInStock - " + int.Parse(txtQty.Text) + " WHERE Transno LIKE '" + lblTranNo.Text + "'  AND PartNumber LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Залишена кількість в кошику " + i + "!", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
    }
}
