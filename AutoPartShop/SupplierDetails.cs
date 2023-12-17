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
    public partial class SupplierDetails : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public SupplierDetails()
        {
            InitializeComponent();
            db.GetConnection();
            LoadSuppliersDetails();
        }

        public void LoadSuppliersDetails()
        {
            int i = 0;
            dgvSuppliersDetails.Rows.Clear();
            cm = new SqlCommand("SELECT d.SupplierDetailID, s.ContactName, a.PartName, d.SupplyDate, d.UnitPrice, d.QuantitySupplied FROM SuppliersDetails AS d INNER JOIN Suppliers AS s ON s.SupplierID = d.SupplierID INNER JOIN AutoParts AS a ON a.PartID = d.PartID WHERE CONCAT (s.ContactName, a.PartName, d.SupplyDate) LIKE '%" + txtSearch.Text + "%'", db.GetConnection());
            db.openConnection();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvSuppliersDetails.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            db.closeConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SupplierDetailsModule supplierDetailsModule = new SupplierDetailsModule(this);
            supplierDetailsModule.ShowDialog();
        }

        private void dgvSuppliersDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSuppliersDetails.Columns[e.ColumnIndex].Name;
            if(colName == "Edit")
            {
                SupplierDetailsModule supplierDetails = new SupplierDetailsModule(this);
                supplierDetails.lblId.Text = dgvSuppliersDetails.Rows[e.RowIndex].Cells[1].Value.ToString();
                supplierDetails.cmbSupplierName.Text = dgvSuppliersDetails.Rows[e.RowIndex].Cells[2].Value.ToString();
                supplierDetails.cmbPartName.Text = dgvSuppliersDetails.Rows[e.RowIndex].Cells[3].Value.ToString();
                supplierDetails.dttSupply.Value = DateTime.Parse(dgvSuppliersDetails.Rows[e.RowIndex].Cells[4].Value.ToString());
                supplierDetails.txtUnitPrice.Text = dgvSuppliersDetails.Rows[e.RowIndex].Cells[5].Value.ToString();
                supplierDetails.UDQuantity.Value = int.Parse(dgvSuppliersDetails.Rows[e.RowIndex].Cells[6].Value.ToString());

                supplierDetails.btnSave.Enabled = false;
                supplierDetails.btnUpdate.Enabled = true;
                supplierDetails.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цей запис?", "Видалити запис", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("DELETE FROM SuppliersDetails WHERE SupplierDetailID LIKE '" + dgvSuppliersDetails[1, e.RowIndex].Value.ToString() + "'", db.GetConnection());
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Деталі постачальника були успішно видалені.", "APS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadSuppliersDetails();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSuppliersDetails();
        }
    }
}
