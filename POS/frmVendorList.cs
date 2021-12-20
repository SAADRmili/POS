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
    public partial class frmVendorList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public frmVendorList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());

        }

        public void loadRecords()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from tblvendor", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i,dr["id"].ToString(), dr["vendor"].ToString(), dr["address"].ToString(), dr["contactperson"].ToString(), dr["telephone"].ToString(), dr["email"].ToString(), dr["fax"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmVendor f = new frmVendor(this);
            f.btnSabe.Enabled = true;
            f.btnUpdate.Enabled = false;
            f.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName == "Edit")
            {
                frmVendor f = new frmVendor(this);
                f.btnSabe.Enabled = false;
                f.btnUpdate.Enabled = true;
                f.lblID.Text = dataGridView1[1, e.RowIndex].Value.ToString();
                f.txtVendor.Text = dataGridView1[2,e.RowIndex].Value.ToString();
                f.txtAddress.Text = dataGridView1[3, e.RowIndex].Value.ToString();
                f.txtContact.Text = dataGridView1[4, e.RowIndex].Value.ToString();
                f.txtTelephone.Text = dataGridView1[5, e.RowIndex].Value.ToString();
                f.txtEmail.Text = dataGridView1[6, e.RowIndex].Value.ToString();
                f.txtFax.Text = dataGridView1[7, e.RowIndex].Value.ToString();
                f.ShowDialog();

            }
            else if(colName == "Delete") {
                try
                {
                    if(MessageBox.Show("ARE YOU SURE FOR DELETE THIS RECORD?","VENDOR",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("delete from tblvendor where id = @id", cn);
                        cm.Parameters.AddWithValue("@id", dataGridView1[1, e.RowIndex].Value.ToString());
                        cm.ExecuteNonQuery();
                        
                        cn.Close();
                        MessageBox.Show("THIS VENDORE HAS BEEN SUCCESSFULLY DELETED", "VENDOR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadRecords();
                    }
                }
                catch(Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
