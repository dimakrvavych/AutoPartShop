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
    public partial class Manufacturer : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public Manufacturer()
        {
            InitializeComponent();

            db.GetConnection();
            LoadManufacturer();
        }

        public void LoadManufacturer()
        {
            int i = 0;
            dgvManufacturer.Rows.Clear();
            db.openConnection();
            cm = new SqlCommand("SELECT * FROM Manufacturers ORDER BY ManufacturerName", db.GetConnection());
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvManufacturer.Rows.Add(i, dr["ManufacturerID"].ToString(), dr["ManufacturerName"].ToString());
            }
            dr.Close();
            db.closeConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ManufacturerModule moduleForm = new ManufacturerModule(this);
            moduleForm.ShowDialog();
        }

        private void dgvManufacturer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvManufacturer.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if(MessageBox.Show("Ви впевнені, що хочете видалити цей запис?", "Видалити запис", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("DELETE FROM Manufacturers WHERE ManufacturerID LIKE '" + dgvManufacturer[1, e.RowIndex].Value.ToString() + "'", db.GetConnection());
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Виробник був успішно видалений.", "APS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colName == "Edit")
            {
                ManufacturerModule manufacturerModule = new ManufacturerModule(this);
                manufacturerModule.lblId.Text = dgvManufacturer[1, e.RowIndex].Value.ToString();
                manufacturerModule.txtManufacturer.Text = dgvManufacturer[2, e.RowIndex].Value.ToString();
                manufacturerModule.btnSave.Enabled = false;
                manufacturerModule.btnUpdate.Enabled = true;
                manufacturerModule.ShowDialog();
            }
            LoadManufacturer();
        }
    }
}
