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
    public partial class frmVendor : Form
    {
        frmVendorList f;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        public frmVendor(frmVendorList frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSabe_Click(object sender, EventArgs e)
        {
            try
            {
               if(MessageBox.Show("SAVE THIS DATE CLICK YES TO CONFIRM ","CONFIRM",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("insert into tblvendor (vendor,address,contactperson,telephone,email,fax) values (@vendor,@address,@contactperson,@telephone,@email,@fax) ", cn);
                    cm.Parameters.AddWithValue("@vendor", txtVendor.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtContact.Text);
                    cm.Parameters.AddWithValue("@telephone", txtTelephone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@fax", txtFax.Text);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Vendor has been successfully seved", "Vendor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    cn.Close();
                    f.loadRecords();
                }
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        public void clear()
        {
            txtVendor.Clear();
            txtAddress.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtFax.Clear();
            txtTelephone.Clear();

            txtVendor.Focus();
            txtAddress.Focus();
            txtContact.Focus();
            txtEmail.Focus();
            txtFax.Focus();
            txtTelephone.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Update THIS DATE CLICK YES TO CONFIRM ", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblvendor set  vendor= @vendor,address=@address,contactperson= @contactperson,telephone=@telephone,email=@email,fax=@fax where  id = @id ", cn);
                    cm.Parameters.AddWithValue("@id", lblID.Text);
                    cm.Parameters.AddWithValue("@vendor", txtVendor.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtContact.Text);
                    cm.Parameters.AddWithValue("@telephone", txtTelephone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@fax", txtFax.Text);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Vendor has been successfully updated", "Vendor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    cn.Close();
                    
                    f.loadRecords();
                    this.Dispose();
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
