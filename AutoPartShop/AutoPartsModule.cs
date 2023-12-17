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
    public partial class AutoPartsModule : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        string stitle = "AutoPartsShop";
        AutoParts autoParts;

        public AutoPartsModule(AutoParts at)
        {
            InitializeComponent();
            db.GetConnection();
            autoParts = at;
            LoadManufacturer();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cboCategory.DataSource = db.getTable("SELECT * FROM Categories");
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryID";
        }

        public void LoadManufacturer()
        {
            cboManufacturer.Items.Clear();
            cboManufacturer.DataSource = db.getTable("SELECT * FROM Manufacturers");
            cboManufacturer.DisplayMember = "ManufacturerName";
            cboManufacturer.ValueMember = "ManufacturerID";
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtPartName.Clear();
            txtPartNumber.Clear();
            txtPrice.Clear();
            cboManufacturer.SelectedIndex = 0;
            cboCategory.SelectedIndex = 0;
            UDQuantity.Value = 1;

            txtPartName.Enabled = true;
            txtPartName.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Ви впенені, що хочете зберегти цю автозапчастину?", "Збереження автозапчастини", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO AutoParts(PartName, PartNumber, Price, QuantityInStock, ManufacturerID, CategoryID) VALUES (@PartName,@PartNumber,@Price,@QuantityInStock,@ManufacturerID,@CategoryID)", db.GetConnection());
                    cm.Parameters.AddWithValue("@PartName", txtPartName.Text);
                    cm.Parameters.AddWithValue("@PartNumber", txtPartNumber.Text);
                    cm.Parameters.AddWithValue("@Price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@QuantityInStock", UDQuantity.Value);
                    cm.Parameters.AddWithValue("@ManufacturerID", cboManufacturer.SelectedValue);
                    cm.Parameters.AddWithValue("@CategoryID", cboCategory.SelectedValue);
                    db.openConnection();
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Автозапчастину було успішно добавлено.", stitle);
                    Clear();
                    autoParts.LoadAutoparts();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви впевнені, що хочете оновити цю автозапчастину?", "Оновлення Автозапчастини", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE AutoParts SET PartName=@PartName, PartNumber=@PartNumber, Price=@Price, QuantityInStock=@QuantityInStock, ManufacturerID=@ManufacturerID, CategoryID=@CategoryID WHERE PartNumber LIKE @PartNumber", db.GetConnection());
                    cm.Parameters.AddWithValue("@PartName", txtPartName.Text);
                    cm.Parameters.AddWithValue("@PartNumber", txtPartNumber.Text);
                    cm.Parameters.AddWithValue("@Price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@QuantityInStock", UDQuantity.Value);
                    cm.Parameters.AddWithValue("@ManufacturerID", cboManufacturer.SelectedValue);
                    cm.Parameters.AddWithValue("@CategoryID", cboCategory.SelectedValue);
                    db.openConnection();
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Автозапчастину було успішно оновлено.", stitle);
                    Clear();
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
