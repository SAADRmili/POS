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
using Tulpep.NotificationWindow;
namespace POS
{
    public partial class Form1 : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr; 
        DBConnection dbcon = new DBConnection();
        public string _pass,_user; 
        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            NotifyCriticalItems();
           
           // cn.Open();
            //MessageBox.Show("Connected");
        }

        public void NotifyCriticalItems()
        {
            string critical = "";
            cn.Open();
            cm = new SqlCommand("select count(*) from vwCriticalItems", cn);
            string count = cm.ExecuteScalar().ToString();
            cn.Close();
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from vwCriticalItems", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                critical += i + dr["pdesc"].ToString() + Environment.NewLine;
            }
            dr.Close();
            cn.Close();
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.prohibition;
            popup.TitleText = count +  " CRITICAL ITEM(S)";
            popup.ContentText = critical;
            popup.Popup();


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            frmBrandList frm = new frmBrandList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            frmCategoryList frm = new POS.frmCategoryList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProductList frm = new POS.frmProductList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.loadrecords();
            frm.Show();
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            frmStockin frm = new frmStockin();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmUserAccount frm = new frmUserAccount(this);
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnSold_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();
          
           
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmRecords frm = new frmRecords();
            frm.TopLevel = false;
            frm.loadCriticalItem();
            frm.loadInventory();
            frm.loadCancelledOrder();
            frm.loadStockInHistory();
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("LOGOUT APPLICATION ?" ,"CONFIRM",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Hide();
                frmSecurity frm = new frmSecurity();
                frm.ShowDialog();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmStore f = new frmStore();
            f.loadRecords();
            f.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDashboard frm = new frmDashboard();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.lblDailySales.Text = dbcon.DailySeles().ToString("#,##0.00");
            frm.lblProduct.Text = dbcon.productLine().ToString();
            frm.lblStockOnHand.Text = dbcon.StockOnHand().ToString();
            frm.lblCritical.Text = dbcon.CriticalItems().ToString();
            frm.BringToFront();
            frm.Show();
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            frmVendorList frm = new frmVendorList();
            frm.TopLevel = false;
            frm.loadRecords();
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAdjustment frm = new frmAdjustment(this);
            frm.loadrecords();
            frm.txtUser.Text = lblusername.Text;
            frm.RefernceNo();
            frm.ShowDialog();
        }
    }
}
