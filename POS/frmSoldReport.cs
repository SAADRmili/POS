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
using Microsoft.Reporting.WinForms;

namespace POS
{
    public partial class frmSoldReport : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmSoldItems fs;
        string store = "Chrif Software Solution";
        string address = "N 510 LOTLAHRACH2 MARRAKECH";
        public frmSoldReport(frmSoldItems frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            fs = frm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmSoldReport_Load(object sender, EventArgs e)
        {

           
        }
      
        public void loadReport()
        {
            ReportDataSource rtpDs;
            try
            {
                

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report2.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                if (fs.cboCashier.Text == "All Cashier")
                {
                    da.SelectCommand = new SqlCommand("select c.id,c.transno,c.pcode , p.pdesc ,c.price,c.qty,c.disc,c.total ,c.status from tblCart as c inner join tblProduct as p on p.pcode = c.pcode where status like 'Sold' and sdate between '" + fs.dt1.Value + "' and '" + fs.dt2.Value + "'", cn);
                }
                else
                {
                    da.SelectCommand = new SqlCommand("select c.id,c.transno,c.pcode , p.pdesc ,c.price,c.qty,c.disc,c.total ,c.status from tblCart as c inner join tblProduct as p on p.pcode = c.pcode where status like 'Sold' and sdate between '" + fs.dt1.Value + "' and '" + fs.dt2.Value + "' and  cashier like '" + fs.cboCashier.Text + "'", cn);

                }

                da.Fill(ds.Tables["dtSoldReport"]);
                cn.Close();
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAddress", address);

                ReportParameter pCashier = new ReportParameter("pCashier","Cashier :"+ fs.cboCashier.Text);
                ReportParameter pDate = new ReportParameter("pDate", "Date From :"+fs.dt1.Value.ToShortDateString()+" To :"+fs.dt2.Value.ToShortDateString());
                ReportParameter pHeader = new ReportParameter("pHeader", "SELES REPORT");

                //ReportParameter sTotal = new ReportParameter("sTotal", fs.lblTotal.Text);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);

                reportViewer1.LocalReport.SetParameters(pCashier);
                reportViewer1.LocalReport.SetParameters(pDate);
                reportViewer1.LocalReport.SetParameters(pHeader);
                //reportViewer1.LocalReport.SetParameters(sTotal);


                rtpDs = new ReportDataSource("DataSet1", ds.Tables["dtSoldReport"]);
                reportViewer1.LocalReport.DataSources.Add(rtpDs);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

    }
}
