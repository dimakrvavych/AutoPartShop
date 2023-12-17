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
    public partial class CategoryModule : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        Category category;

        public CategoryModule(Category ct)
        {
            InitializeComponent();
            db.GetConnection();
            category = ct;
        }

        public void Clear()
        {
            txtCategory.Clear();
            txtCategory.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;

        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви впевнені, що хочете зберегти цю категорію?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("INSERT INTO Categories(CategoryName) VALUES (@CategoryName)", db.GetConnection());
                    cm.Parameters.AddWithValue("@CategoryName", txtCategory.Text);
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Запис було успішно збережено.", "APS");
                    Clear();
                }
                category.LoadCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені, що хочете оновити цю категорію?", "Оновлення категорії!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.openConnection();
                cm = new SqlCommand("UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID LIKE '" + lblId.Text + "'", db.GetConnection());
                cm.Parameters.AddWithValue("@CategoryName", txtCategory.Text);
                cm.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show("Категорію було успішно змінено.", "APS");
                Clear();
                this.Dispose();
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
