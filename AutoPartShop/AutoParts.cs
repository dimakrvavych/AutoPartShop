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
    public partial class AutoParts : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public AutoParts()
        {
            InitializeComponent();
            db.GetConnection();
            LoadAutoparts();
        }

        public void LoadAutoparts()
        {
            int i = 0;
            dgvAutoparts.Rows.Clear();
            cm = new SqlCommand("SELECT a.PartID, a.PartName, a.PartNumber, m.ManufacturerName, c.CategoryName, a.Price, a.QuantityInStock FROM AutoParts AS a INNER JOIN Manufacturers AS m ON m.ManufacturerID = a.ManufacturerID INNER JOIN Categories AS c ON c.CategoryID = a.CategoryID WHERE CONCAT (a.PartName, a.PartNumber, m.ManufacturerName, c.CategoryName) LIKE '%" + txtSearch.Text + "%'", db.GetConnection());
            db.openConnection();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvAutoparts.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            db.closeConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AutoPartsModule autoPartsModule = new AutoPartsModule(this);
            autoPartsModule.ShowDialog();
        }

        private void dgvAutoparts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvAutoparts.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                AutoPartsModule autoparts = new AutoPartsModule(this);
                autoparts.txtPartName.Text = dgvAutoparts.Rows[e.RowIndex].Cells[2].Value.ToString();
                autoparts.txtPartNumber.Text = dgvAutoparts.Rows[e.RowIndex].Cells[3].Value.ToString();
                autoparts.cboManufacturer.Text = dgvAutoparts.Rows[e.RowIndex].Cells[4].Value.ToString();
                autoparts.cboCategory.Text = dgvAutoparts.Rows[e.RowIndex].Cells[5].Value.ToString();
                autoparts.txtPrice.Text = dgvAutoparts.Rows[e.RowIndex].Cells[6].Value.ToString();
                autoparts.UDQuantity.Value = int.Parse(dgvAutoparts.Rows[e.RowIndex].Cells[7].Value.ToString());

                autoparts.btnSave.Enabled = false;
                autoparts.btnUpdate.Enabled = true;
                autoparts.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цей запис?", "Видалити запис", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("DELETE FROM AutoParts WHERE PartID LIKE '" + dgvAutoparts[1, e.RowIndex].Value.ToString() + "'", db.GetConnection());
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Автозапчастина була успішно видалена.", "APS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadAutoparts();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadAutoparts();
        }
    }
}
