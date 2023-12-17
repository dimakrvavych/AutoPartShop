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
    public partial class SupplierModule : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        string stitle = "AutoPartsShop";
        Supplier supplier;

        public SupplierModule(Supplier sp)
        {
            InitializeComponent();
            db.GetConnection();
            supplier = sp;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtSupplier.Clear();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtSupplier.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви впенені, що хочете зберегти цього постачальника?", "Збереження постачальника", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("INSERT INTO Suppliers (ContactName, ContactPhone, ContactEmail, SupplierAddress) VALUES (@ContactName, @ContactPhone, @ContactEmail, @SupplierAddress)", db.GetConnection());
                    cm.Parameters.AddWithValue("@ContactName", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@ContactPhone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@ContactEmail", txtEmail.Text);
                    cm.Parameters.AddWithValue("@SupplierAddress", txtAddress.Text);
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Постачальника було успішно добавлено.", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    supplier.LoadSupplier();
                }

            }
            catch (Exception ex)  
            {
                MessageBox.Show(ex.Message, stitle);            
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви впевнені, що хочете оновити цього постачальника?", "Оновлення постачальника", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.openConnection();
                    cm = new SqlCommand("UPDATE Suppliers SET ContactName=@ContactName, ContactPhone=@ContactPhone, ContactEmail=@ContactEmail, SupplierAddress=@SupplierAddress WHERE SupplierID=@SupplierID", db.GetConnection());
                    cm.Parameters.AddWithValue("@SupplierID", lblId.Text);
                    cm.Parameters.AddWithValue("@ContactName", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@ContactPhone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@ContactEmail", txtEmail.Text);
                    cm.Parameters.AddWithValue("@SupplierAddress", txtAddress.Text);
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Постачальника було успішно добавлено.", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
