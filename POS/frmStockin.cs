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

namespace POS
{
    public partial class frmStockin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmStockin()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            loadVendor();
        }
       
        private void frmStockin_Load(object sender, EventArgs e)
        {

        }

       private void loadStockInHistory()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            //cm = new SqlCommand("select s.id , s.refno,s.pcode,p.pdesc , s.qty,s.sdate,s.stockinby from tblStockIn as s inner  join  tblProduct as p on s.pcode = p.pcode where cast( s.sdate as date) between '"+date1.Value+"' and '"+date2.Value+"' and s.status like 'Done'", cn);
            cm = new SqlCommand("select * from vwstockin where  sdate  between '" + date1.Value + "' and '" + date2.Value + "' and status like 'Done'", cn); 
           dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(),dr["vendor"].ToString(), dr[3].ToString(), dr[4].ToString(),DateTime.Parse( dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void clear ()
        {
            txtRef.Clear();
            txtStockby.Clear();
            dateStockin.Value = DateTime.Now;
            txtRef.Focus();
            txtStockby.Focus();
        }
       
        public void loadStockIn()
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
           // cm = new SqlCommand("select s.id , s.refno,s.pcode,p.pdesc , s.qty,s.sdate,s.stockinby from tblStockIn as s inner  join  tblProduct as p on s.pcode = p.pcode where refno like '"+ txtRef.Text+"' and status like 'Pending'  order by s.sdate desc", cn);
           cm = new SqlCommand("select * from vwstockin where refno like '"+txtRef.Text+ "' and status like 'Pending'", cn); 
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i+=1;
                dataGridView2.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[8].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSerachProductStockin frm = new frmSerachProductStockin(this);
            frm.loadProduct();
                frm.ShowDialog();
                }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView2.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Remove this item?", "POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblstockin where  id =" + dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString() + "", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been removed");
                    loadStockIn();
                }
             
            }
        }
        
        private void btnSabe_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView2.Rows.Count>0)
                {
                    if(MessageBox.Show("Are you sure you want to save this records?","POS",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                    {
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            //update product qty
                            cn.Open();
                            cm = new SqlCommand("update tblproduct set qty=qty +" + int.Parse(dataGridView2.Rows[i].Cells[6].Value.ToString()) + " where pcode like '" + dataGridView2.Rows[i].Cells[3].Value.ToString() + "' ", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            //update tblstockin qty
                            cn.Open();
                            cm = new SqlCommand("update tblstockin set qty = qty + " + int.Parse(dataGridView2.Rows[i].Cells[6].Value.ToString()) + ", status ='Done' where id =" + int.Parse(dataGridView2.Rows[i].Cells[1].Value.ToString()) + "", cn);
                            cm.ExecuteNonQuery();
                             cn.Close();
                        }
                        clear();
                        loadStockIn();
                    }
                  
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"POS",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            loadStockInHistory();
        }

        public void  loadVendor()
        {
            comboBox1.Items.Clear();
            
            cn.Open();
            cm = new SqlCommand("select * from tblvendor", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                comboBox1.Items.Add(dr["vendor"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("select * from tblvendor where vendor like '" + comboBox1.Text + "'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                lblVendorId.Text = dr["id"].ToString();
                txtContactPerson.Text = dr["contactperson"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Random rand = new Random();
            int i = 0;
            txtRef.Clear();
            txtRef.Text += rand.Next();
        }
    }
}
