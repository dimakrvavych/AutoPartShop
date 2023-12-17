using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AutoPartShop
{
    public partial class frmAdmin : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();

        public frmAdmin()
        {
            InitializeComponent();
            customizeDesing();
            openChildForm(new Dashboard());
            db.GetConnection();
            db.openConnection();
        }

        #region panelSlide
        private void customizeDesing()
        {
            panelSubProduct.Visible = false;
            panelSubSupllier.Visible = false;
        }

        private void hideSubmenu() 
        {
            if (panelSubProduct.Visible == true)
                panelSubProduct.Visible = false;
            if (panelSubSupllier.Visible == true)
                panelSubSupllier.Visible = false;
        }

        private void showSubmenu(Panel submenu) 
        {
            if (submenu.Visible == false) 
            {
                hideSubmenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }
        #endregion panelSlide

        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if(activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
            hideSubmenu();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            showSubmenu(panelSubProduct);
        }

        private void btnProductList_Click(object sender, EventArgs e)
        {
            openChildForm(new AutoParts());
            hideSubmenu();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            openChildForm(new Category());
            hideSubmenu();
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            openChildForm(new Manufacturer());
            hideSubmenu();
        }

        private void btnSupplie_Click(object sender, EventArgs e)
        {
            showSubmenu(panelSubSupllier);
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            openChildForm (new Supplier());
            hideSubmenu();
        }

        private void btnSupplierDetail_Click(object sender, EventArgs e)
        {
            openChildForm (new SupplierDetails());
            hideSubmenu();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            hideSubmenu();
            if (MessageBox.Show("Вийти з системи?", "Вихід з системи", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmLogin login = new frmLogin();
                login.ShowDialog();
            }
        }
    }
}
