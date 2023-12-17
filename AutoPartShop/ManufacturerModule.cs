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
    public partial class ManufacturerModule : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        Manufacturer manufacturer;

        public ManufacturerModule(Manufacturer mn)
        {
            InitializeComponent();
            db.GetConnection();
            manufacturer = mn;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try 
            { 
                if(MessageBox.Show("Ви впевнені, що хочете зберегти цього виробника?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
                {
                    db.openConnection();
                    cm = new SqlCommand("INSERT INTO Manufacturers(ManufacturerName) VALUES (@ManufacturerName)", db.GetConnection());
                    cm.Parameters.AddWithValue("@ManufacturerName", txtManufacturer.Text);
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Запис було успішно збережено.", "APS");
                    Clear();
                    manufacturer.LoadManufacturer();
                }
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

        public void Clear() 
        {
            txtManufacturer.Clear();
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
            txtManufacturer.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ви впевнені, що хочете оновити цього виробника?", "Оновлення виробника!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.openConnection();
                cm = new SqlCommand("UPDATE Manufacturers SET ManufacturerName = @ManufacturerName WHERE ManufacturerID LIKE '" + lblId.Text + "'", db.GetConnection());
                cm.Parameters.AddWithValue("@ManufacturerName", txtManufacturer.Text);
                cm.ExecuteNonQuery();
                db.closeConnection();
                MessageBox.Show("Виробника було успішно змінено.", "APS");
                Clear();
                this.Dispose();
            }
        }

    }
}
