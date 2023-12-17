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
    public partial class Category : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public Category()
        {
            InitializeComponent();

            db.GetConnection();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            db.openConnection();
            cm = new SqlCommand("SELECT * FROM Categories ORDER BY CategoryName", db.GetConnection());
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, dr["CategoryID"].ToString(), dr["CategoryName"].ToString());
            }
            dr.Close();
            db.closeConnection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryModule category = new CategoryModule(this);
            category.ShowDialog();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити цей запис?", "Видалити запис", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("DELETE FROM Categories WHERE CategoryID LIKE '" + dgvCategory[1, e.RowIndex].Value.ToString() + "'", db.GetConnection());
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Виробник був успішно видалений.", "APS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else if (colName == "Edit")
            {
                CategoryModule categoryModule = new CategoryModule(this);
                categoryModule.lblId.Text = dgvCategory[1, e.RowIndex].Value.ToString();
                categoryModule.txtCategory.Text = dgvCategory[2, e.RowIndex].Value.ToString();
                categoryModule.btnSave.Enabled = false;
                categoryModule.btnUpdate.Enabled = true;
                categoryModule.ShowDialog();
            }
            LoadCategory();
        }
    }
}
