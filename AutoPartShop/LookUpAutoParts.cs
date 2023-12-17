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
    public partial class LookUpAutoParts : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        string stitle = "AutoPartsShop";
        Cashier cashier;

        public LookUpAutoParts(Cashier cash)
        {
            InitializeComponent();
            db.GetConnection();
            cashier = cash;
            LoadAutoparts();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadAutoparts()
        {
            int i = 0;
            dgvAutoparts.Rows.Clear();
            cm = new SqlCommand("SELECT a.PartID, a.PartName, a.PartNumber, m.ManufacturerName, c.CategoryName, a.Price, a.QuantityInStock FROM AutoParts AS a INNER JOIN Manufacturers AS m ON m.ManufacturerID = a.ManufacturerID INNER JOIN Categories AS c ON c.CategoryID = a.CategoryID WHERE CONCAT (a.PartName, a.PartNumber, m.ManufacturerName, c.CategoryName) LIKE '%" + txtSearch.Text + "%'", db.GetConnection());
            db.openConnection();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvAutoparts.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            db.closeConnection();
        }

        private void dgvAutoparts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvAutoparts.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                Qty qty = new Qty(cashier);
                qty.AutoPartsDetails(dgvAutoparts.Rows[e.RowIndex].Cells[3].Value.ToString(), double.Parse(dgvAutoparts.Rows[e.RowIndex].Cells[6].Value.ToString()), cashier.lblTranNo.Text, int.Parse(dgvAutoparts.Rows[e.RowIndex].Cells[7].Value.ToString()));
                qty.ShowDialog();
                LoadAutoparts();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadAutoparts();
        }

        private void LookUpAutoParts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
