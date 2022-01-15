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
using System.Windows.Forms.DataVisualization.Charting;
namespace POS
{
    public partial class frmRecords : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

        }

         public void loadInventory()
        {
            dataGridView5.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select p.pcode,p.barcode,p.pdesc,b.brand,c.category,p.price,p.qty,p.reorder from tblProduct as p  inner join  tblBrand as b on b.id = p.bid inner join tblCategory as c  on c.id = p.cid ", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView5.Rows.Add(i,dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdesc"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["reorder"].ToString());
            }
            dr.Close();
            cn.Close();
        }
         public void loadCriticalItem ()
        {
            try
            {
                int i = 0; 
                cn.Open();
                cm = new SqlCommand("select * from vwCriticalItems", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView4.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdesc"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["reorder"].ToString(), dr["qty"].ToString());
                }
                dr.Close();

                cn.Close();

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void loadCancelledOrder()
        {
            dataGridView6.Rows.Clear();
            int i = 0;

            cn.Open();

            cm = new SqlCommand("select * from vwcancelledOrder where sdate between '"+dateTimePicker6.Value.ToString("yyyy-MM-dd")+"' and '"+dateTimePicker5.Value.ToString("yyyy-MM-dd") +"' ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView6.Rows.Add(i, dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["total"].ToString(), dr["voidby"].ToString(), dr["cancelledby"].ToString(), dr["reason"].ToString(), dr["action"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        public void loadRecord()
        {
            dataGridView1.Rows.Clear();
            int i =0;
            
            cn.Open();
            
            if(cboSort.Text == "SORT BY QTY")
            {
                cm = new SqlCommand("select top 10 pcode, pdesc,isnull( sum(qty),0) as qty , isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and status like 'Sold'   group by pcode,pdesc order by qty desc", cn);

            }
            else if( cboSort.Text == "SORT BY TOTAL")
            {
                cm = new SqlCommand("select top 10 pcode, pdesc,isnull( sum(qty),0) as qty , isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and status like 'Sold'   group by pcode,pdesc order by total desc", cn);

            }

            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["qty"].ToString(), double.Parse( dr["total"].ToString()).ToString("#,##0.00"));
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.loadReport();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadCancelledOrder();
        }
        public void loadStockInHistory()
        {
            int i = 0;
            dataGridView7.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwstockin  where sdate between '" + dateTimePicker8.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker7.Value.ToString("yyyy-MM-dd") + "' and status like 'Done'", cn);
            // cm = new SqlCommand("select * from vwstockin", cn); tan wslo vendor
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                dataGridView7.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();

            if(cboSort.Text == "SORT BY QTY")
            {
                frm.loadTop("select top 10 pcode, pdesc,isnull( sum(qty),0) as qty , isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and status like 'Sold'   group by pcode,pdesc order by qty desc", "DATE -> FROM : " + dateTimePicker1.Value.ToShortDateString() + " TO " + dateTimePicker2.Value.ToShortDateString());

            }
            else if(cboSort.Text == "SORT BY TOTAL")
            {
                frm.loadTop("select top 10 pcode, pdesc,isnull( sum(qty),0) as qty , isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and status like 'Sold'   group by pcode,pdesc order by total desc", "DATE -> FROM : " + dateTimePicker1.Value.ToShortDateString() + " TO " + dateTimePicker2.Value.ToShortDateString());

            }

            frm.ShowDialog();

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.loadSold("select v.pcode, p.pdesc, v.price, sum(v.qty) as qty, sum(v.disc) as disc, sum(v.total) as total  from tblCart as v inner join tblProduct as p on p.pcode = v.pcode   where v.status like 'Sold' and v.sdate between '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker4.Value.ToString("yyyy-MM-dd") + "'  group by v.pcode, p.pdesc, v.price", "DATE -> FROM : " + dateTimePicker3.Value.ToShortDateString() + " TO " + dateTimePicker4.Value.ToShortDateString());
            frm.ShowDialog();

        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(cboSort.Text == String.Empty)
            {
                MessageBox.Show("Please select from the dropdown list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            loadRecord();
            loadChartTopSelling();
        }


        public void loadChartTopSelling()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cn.Open();
            if (cboSort.Text == "SORT BY QTY")
            {
                 da = new SqlDataAdapter ("select top 10 pdesc,isnull( sum(qty),0) as qty  from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and status like 'Sold'   group by pdesc order by qty desc", cn);

            }
            else if (cboSort.Text == "SORT BY TOTAL")
            {
                  da = new SqlDataAdapter("select top 10 pdesc , isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and status like 'Sold'   group by pdesc order by total desc", cn);

            }
            DataSet ds = new DataSet();
            da.Fill(ds, "TopSELLING");
            chart1.DataSource = ds.Tables["TopSELLING"];
            Series  series = chart1.Series[0];
            series.ChartType = SeriesChartType.Doughnut;
            series.Name = "TOP SELLING";
            var chart = chart1;
            chart.Series[0].XValueMember = "pdesc";
            if (cboSort.Text == "SORT BY QTY")
            {
                chart.Series[0].YValueMembers = "qty";
            }
            if (cboSort.Text == "SORT BY TOTAL")
            {
                chart.Series[0].YValueMembers = "total";
            }
            chart.Series[0].IsValueShownAsLabel = true;
            if (cboSort.Text == "SORT BY TOTAL") {
                chart.Series[0].LabelFormat = "{#,##0.00}";
               }
            if (cboSort.Text == "SORT BY QTY")
            {
                chart.Series[0].LabelFormat = "{#,##0}";
            }

            cn.Close();
        }
        private void cboSort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                dataGridView3.Rows.Clear();
                int i = 0;

                cn.Open();

                cm = new SqlCommand("select v.pcode, p.pdesc , sum(v.qty) as qty ,v.price, sum(v.disc) as disc,sum(v.total) as total  from tblCart as v inner join tblProduct as p on p.pcode = v.pcode   where v.status like 'Sold' and v.sdate between '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker4.Value.ToString("yyyy-MM-dd") + "'  group by v.pcode,p.pdesc,v.price ", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView3.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());

                }
                dr.Close();
                cn.Close();

                String x = "";
                cn.Open();

                cm = new SqlCommand("select isnull( sum(total) ,0) from tblCart where status like 'Sold' and sdate between '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker4.Value.ToString("yyyy-MM-dd") + "' ", cn);

                x = cm.ExecuteScalar().ToString();
                lblTotal.Text = double.Parse(x).ToString();


                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmChart frm = new frmChart();
            frm.lbltitle.Text = "SOLD ITEMS ["+dateTimePicker3.Value.ToShortDateString()+" - "+dateTimePicker4.Value.ToShortDateString()+"]";
            frm.loadChartSoldItems("select  p.pdesc, sum(v.total) as total  from tblCart as v inner join tblProduct as p on p.pcode = v.pcode   where v.status like 'Sold' and v.sdate between '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker4.Value.ToString("yyyy-MM-dd") + "'  group by  p.pdesc  order by total desc");
            frm.ShowDialog();
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            loadStockInHistory();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.loadStockInReport("select * from vwstockin where sdate between '" + dateTimePicker8.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker7.Value.ToString("yyyy-MM-dd") + "' and status like 'Done'");
            frm.ShowDialog();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
