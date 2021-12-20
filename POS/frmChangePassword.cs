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
    public partial class frmChangePassword : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        frmPOS f;
        public frmChangePassword(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string _oldpass = dbcon.getPassword(f.lblusername.Text);
                if(_oldpass != txtOld.Text)
                {
                    MessageBox.Show("Old password did not matched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(txtNew.Text!=txtConfirm.Text)
                {
                    MessageBox.Show("Confirm password did not matched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if(MessageBox.Show("Change Password?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("update tbluser set password = @password where  username = @username", cn);
                        cm.Parameters.AddWithValue("@username", f.lblusername.Text);
                        cm.Parameters.AddWithValue("@password", txtNew.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Password has been successfully saved!", "Save Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();

                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
