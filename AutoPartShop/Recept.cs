using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace AutoPartShop
{
    public partial class Recept : Form
    {
        DataBase db = new DataBase();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        Cashier cashier;

        public Recept(Cashier cash)
        {
            InitializeComponent();
            db.GetConnection();
            cashier = cash;
        }

        private void Recept_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        public void LoadRecept(string pcash, string pchange)
        {
            ReportDataSource rptDataSourece;
            try
            {

                //this.reportViewer1.LocalReport.ReportPath = @"D:\C# projects\AutoPartShop\AutoPartShop\bin\Debug\Reports\rptRecept.rdlc";
                string reportPath = Path.Combine(Application.StartupPath, @"Reports\rptRecept.rdlc");
                this.reportViewer1.LocalReport.ReportPath = reportPath;
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                db.openConnection();
                da.SelectCommand = new SqlCommand("SELECT c.ID, c.Transno, a.PartName, c.PartNumber, c.Price, c.QuantityInStock, c.Total, c.sdate, c.Status FROM Cart AS c INNER JOIN AutoParts AS a ON a.PartNumber = c.PartNumber WHERE c.Transno LIKE '" + cashier.lblTranNo.Text + "'", db.GetConnection());
                da.Fill(ds.Tables["dtRecept"]);
                db.closeConnection();

                ReportParameter pTotal = new ReportParameter("pTotal", cashier.lblDisplayTotal.Text);
                ReportParameter pCash = new ReportParameter("pCash", pcash);
                ReportParameter pChange = new ReportParameter("pChange", pchange);
                ReportParameter pTransaction = new ReportParameter("pTransaction", "Invoice #: " + cashier.lblTranNo.Text);
                ReportParameter pCashier = new ReportParameter("pCashier", cashier.lblUsername.Text);

                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCashier);

                rptDataSourece = new ReportDataSource("DataSet1", ds.Tables["dtRecept"]);
                reportViewer1.LocalReport.DataSources.Add(rptDataSourece);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 30;
            }
            catch (Exception ex)
            {
                db.closeConnection();
                MessageBox.Show(ex.Message);
            }
        }

        private void Recept_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
