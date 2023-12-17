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
    public partial class SupplierDetailsModule : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        string stitle = "AutoPartsShop";
        SupplierDetails supplierDetails;

        public SupplierDetailsModule(SupplierDetails sd)
        {
            InitializeComponent();
            db.GetConnection();
            supplierDetails = sd;
            LoadSupplierName();
            LoadPartName();
        }

        public void LoadSupplierName()
        {
            cmbSupplierName.Items.Clear();
            cmbSupplierName.DataSource = db.getTable("SELECT * FROM Suppliers");
            cmbSupplierName.DisplayMember = "ContactName";
            cmbSupplierName.ValueMember = "SupplierID";
        }

        public void LoadPartName()
        {
            cmbPartName.Items.Clear();
            cmbPartName.DataSource = db.getTable("SELECT * FROM AutoParts");
            cmbPartName.DisplayMember = "PartName";
            cmbPartName.ValueMember = "PartID";
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            cmbSupplierName.SelectedIndex = 0;
            cmbPartName.SelectedIndex = 0;
            txtUnitPrice.Clear();
            UDQuantity.Value = 1;
            dttSupply.Value = DateTime.Now;

            txtUnitPrice.Enabled = true;
            cmbSupplierName.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Ви впевнені, що хочете зберегти ці деталі постачальника?", "Збереження деталей постачальника", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO SuppliersDetails(SupplierID, PartID, SupplyDate, UnitPrice, QuantitySupplied) VALUES (@SupplierID, @PartID, @SupplyDate, @UnitPrice, @QuantitySupplied)", db.GetConnection());
                    cm.Parameters.AddWithValue("@SupplierID", cmbSupplierName.SelectedValue);
                    cm.Parameters.AddWithValue("@PartID", cmbPartName.SelectedValue);
                    cm.Parameters.AddWithValue("@SupplyDate", dttSupply.Value);
                    cm.Parameters.AddWithValue("@UnitPrice", double.Parse(txtUnitPrice.Text));
                    cm.Parameters.AddWithValue("@QuantitySupplied", UDQuantity.Value);
                    db.openConnection();
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Деталі постачальника було успішно добавлено.", stitle);
                    Clear();
                    supplierDetails.LoadSuppliersDetails();
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
                if (MessageBox.Show("Ви впевнені, що хочете оновити ці деталі постачальника?", "Оновлення деталей постачальника", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE SuppliersDetails SET SupplierID=@SupplierID, PartID=@PartID, SupplyDate=@SupplyDate, UnitPrice=@UnitPrice, QuantitySupplied=@QuantitySupplied WHERE SupplierDetailID LIKE @SupplierDetailID", db.GetConnection());
                    cm.Parameters.AddWithValue("@SupplierDetailID", lblId.Text);
                    cm.Parameters.AddWithValue("@SupplierID", cmbSupplierName.SelectedValue);
                    cm.Parameters.AddWithValue("@PartID", cmbPartName.SelectedValue);
                    cm.Parameters.AddWithValue("@SupplyDate", dttSupply.Value);
                    cm.Parameters.AddWithValue("@UnitPrice", double.Parse(txtUnitPrice.Text));
                    cm.Parameters.AddWithValue("@QuantitySupplied", UDQuantity.Value);
                    db.openConnection();
                    cm.ExecuteNonQuery();
                    db.closeConnection();
                    MessageBox.Show("Деталі постачальника було успішно оновлено.", stitle);
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
