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
    public partial class frmLogin : Form
    {
        DataBase dataBase = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public frmLogin()
        {
            InitializeComponent();
            dataBase.GetConnection();
            txtUsername.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _username = "", _name = "", _role = "";
            try
            {
                bool found;
                dataBase.openConnection();
                cm = new SqlCommand("SELECT * FROM Users WHERE Username = @Username and Password = @Password", dataBase.GetConnection());
                cm.Parameters.AddWithValue("@Username", txtUsername.Text);
                cm.Parameters.AddWithValue("@Password", md5.hashPassword(txtPassword.Text));
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    found = true;
                    _username = dr["Username"].ToString();
                    _name = dr["FirstName"].ToString();
                    _role = dr["Role"].ToString();
                }
                else
                {
                    found = false;
                }

                if(found)
                {
                    if (_role == "Cashier")
                    {
                        txtUsername.Clear();
                        txtPassword.Clear();
                        this.Hide();
                        Cashier cashier = new Cashier();
                        cashier.lblUsername.Text = _username;
                        cashier.lblname.Text = _name + " | " + _role;
                        cashier.ShowDialog();
                    }
                    else
                    {
                        txtUsername.Clear();
                        txtPassword.Clear();
                        this.Hide();
                        frmAdmin admin = new frmAdmin();
                        admin.lblUsername.Text = _username;
                        admin.lblName.Text = _name;
                        admin.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Неправильний логін, або пароль!", "Доступ відхилено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                dataBase.closeConnection();
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbxShowPas.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            new frmRegister().Show();
            this.Hide();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вийти з програми?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
