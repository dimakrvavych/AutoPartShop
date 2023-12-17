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
    public partial class Supplier : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public Supplier()
        {
            InitializeComponent();
            db.GetConnection();
            LoadSupplier();
        }

        public void LoadSupplier()
        {
            dgvSuppliers.Rows.Clear();
            int i = 0;
            db.openConnection();
            cm = new SqlCommand("SELECT * FROM Suppliers WHERE CONCAT (ContactName, ContactPhone, ContactEmail, SupplierAddress) LIKE '%" + txtSearch.Text + "%'", db.GetConnection());
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvSuppliers.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }
            dr.Close();
            db.closeConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SupplierModule supplierModule = new SupplierModule(this);
            supplierModule.ShowDialog();
        }

        private void dgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSuppliers.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                SupplierModule supplierModule = new SupplierModule(this);
                supplierModule.lblId.Text = dgvSuppliers.Rows[e.RowIndex].Cells[1].Value.ToString();
                supplierModule.txtSupplier.Text = dgvSuppliers.Rows[e.RowIndex].Cells[2].Value.ToString();
                supplierModule.txtPhone.Text = dgvSuppliers.Rows[e.RowIndex].Cells[3].Value.ToString();
                supplierModule.txtEmail.Text = dgvSuppliers.Rows[e.RowIndex].Cells[4].Value.ToString();
                supplierModule.txtAddress.Text = dgvSuppliers.Rows[e.RowIndex].Cells[5].Value.ToString();

                supplierModule.btnSave.Enabled = false;
                supplierModule.btnUpdate.Enabled = true;
                supplierModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цей запис?", "Видалити запис", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("DELETE FROM Suppliers WHERE SupplierID LIKE '" + dgvSuppliers.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", db.GetConnection());
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Постачальника було успішно видалено.", "APS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadSupplier();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSupplier();
        }
    }
}
