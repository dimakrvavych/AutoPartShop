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
    public partial class Qty : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string stitle = "AutoPartsShop";
        private string PartNumber;
        private double Price;
        private String Transno;
        private int QuantityInStock;
        Cashier cashier;

        public Qty(Cashier cash)
        {
            InitializeComponent();
            db.GetConnection();
            cashier = cash; 
        }

        public void AutoPartsDetails (string  PartNumber, double Price, string Transno, int QuantityInStock)
        {
            this.PartNumber = PartNumber;
            this.Price = Price;
            this.Transno = Transno;
            this.QuantityInStock = QuantityInStock;
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13) && (txtQty.Text != string.Empty))
            {
                try
                {
                    string id = "";
                    int cart_qty = 0;
                    bool found = false;
                    db.openConnection();
                    cm = new SqlCommand("SELECT * FROM Cart WHERE Transno = @Transno and PartNumber = @PartNumber", db.GetConnection());
                    cm.Parameters.AddWithValue("@Transno", Transno);
                    cm.Parameters.AddWithValue("@PartNumber", PartNumber);
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
                        if (QuantityInStock < (int.Parse(txtQty.Text) + cart_qty))
                        {
                            MessageBox.Show("Неможливо продовжити. Залишилася кількість на руках" + QuantityInStock, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        db.openConnection();
                        cm = new SqlCommand("UPDATE Cart SET QuantityInStock = (QuantityInStock + " + int.Parse(txtQty.Text) + ") WHERE ID = '" + id + "'", db.GetConnection());
                        cm.ExecuteNonQuery();
                        db.closeConnection();
                        cashier.txtPartnumber.Clear();
                        cashier.txtPartnumber.Focus();
                        cashier.LoadCart();
                        this.Dispose();
                    }
                    else
                    {
                        if (QuantityInStock < (int.Parse(txtQty.Text) + cart_qty))
                        {
                            MessageBox.Show("Неможливо продовжити. Залишилася кількість на руках" + QuantityInStock, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        db.openConnection();
                        cm = new SqlCommand("INSERT INTO Cart (Transno, PartNumber, Price, QuantityInStock, sdate, Cashier) VALUES (@Transno, @PartNumber, @Price, @QuantityInStock, @sdate, @Cashier)", db.GetConnection());
                        cm.Parameters.AddWithValue("@Transno", Transno);
                        cm.Parameters.AddWithValue("@PartNumber", PartNumber);
                        cm.Parameters.AddWithValue("@Price", Price);
                        cm.Parameters.AddWithValue("@QuantityInStock", int.Parse(txtQty.Text));
                        cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                        cm.Parameters.AddWithValue("@Cashier", cashier.lblUsername.Text);
                        cm.ExecuteNonQuery();
                        db.closeConnection();
                        cashier.txtPartnumber.Clear();
                        cashier.txtPartnumber.Focus();
                        cashier.LoadCart();
                        this.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, stitle);
                }
            }
        }
    }
}
