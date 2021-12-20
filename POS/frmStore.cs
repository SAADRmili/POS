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
namespace POS
{
    public partial class frmStore : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmStore()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void loadRecords()
        {
            cn.Open();
            cm = new SqlCommand("select * from tblstore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                txtStore.Text = dr["store"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            else
            {
                txtStore.Clear();
                txtAddress.Clear();
            }
            cn.Close();
        }
        private void btnSabe_Click(object sender, EventArgs e)
        {
            try
            {
                 if(MessageBox.Show("SAVE STORE DETAILS ?","CONFIRM",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    if(getStoreInfo() > 0)
                    {
                        cn.Open();
                        cm = new SqlCommand("update  tblstore set store =@store , address = @address", cn);
                        cm.Parameters.AddWithValue("@store", txtStore.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    else
                    {
                        cn.Open();
                        cm = new SqlCommand(" insert into  tblstore  (store,address) values(@store ,@address)", cn);
                        cm.Parameters.AddWithValue("@store", txtStore.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    MessageBox.Show("STORE DETAILS HAS BEEN SUCCESSFULLY SAVED!", "SAVE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int getStoreInfo()
        {
            int count;
            cn.Open();
            cm = new SqlCommand("select count(*) from tblstore", cn);
            count = int.Parse(cm.ExecuteScalar().ToString());

            cn.Close();
            return count; 
        }
    }
}
