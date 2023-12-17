using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AutoPartShop
{
    public partial class frmRegister : Form
    {
        DataBase dataBase = new DataBase();
        SqlCommand command = new SqlCommand();

        public frmRegister()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtFirstname.Text == "" && txtLastname.Text == "" && txtEmail.Text == "" && txtUsername.Text == "" && txtPassword.Text == "" && txtComPassword.Text == "")
            {
                MessageBox.Show("Перевірте чи ви всі дані заповнили!", "Неуспішна реєстрація", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPassword.Text == txtComPassword.Text)
            {
                dataBase.openConnection();

                var Firstname = txtFirstname.Text;
                var Lastname = txtLastname.Text;
                var Email = txtEmail.Text;
                var Username = txtUsername.Text;
                var Password = md5.hashPassword(txtPassword.Text);
                
                string register = $"INSERT INTO Users(FirstName, LastName, Email, Username, Password) values ('{Firstname}', '{Lastname}', '{Email}', '{Username}', '{Password}')";
                command = new SqlCommand(register, dataBase.GetConnection());
                
                command.ExecuteNonQuery();
                dataBase.closeConnection();

                txtFirstname.Text = "";
                txtLastname.Text = "";
                txtEmail.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtComPassword.Text = "";

                MessageBox.Show("Ваш обліковий запис успішно створено!", "Реєстрація пройшла успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Паролі не збігаються, перевірте правильність вводу", "Неуспішна реєстрація", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtComPassword.Text = "";
                txtPassword.Focus();
            }
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbxShowPas.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtComPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
                txtComPassword.PasswordChar = '•';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtComPassword.Text = "";
            txtFirstname.Focus();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
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
