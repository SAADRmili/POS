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
    public partial class frmUserAccount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        Form1 frm;
        public frmUserAccount(Form1 f)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frm = f;
            txtUser1.Text = f._user;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmUserAccount_Resize(object sender, EventArgs e)
        {
            metroTabControl1.Left = (this.Width - metroTabControl1.Width) / 2;
            metroTabControl1.Top = (this.Height - metroTabControl1.Height) / 2;
        }

        private void Clear()
        {
            txtName.Clear();
            txtPassword.Clear();
            txtRetype.Clear();
            txtUserName.Clear();
            cRole.Text = "";
            txtUserName.Focus();
        }
        private void frmUserAccount_Load(object sender, EventArgs e)
        {
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPassword.Text!= txtRetype.Text)
                {
                    MessageBox.Show("Password did not match!", "POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                cn.Open();
                cm = new SqlCommand("insert into tblUser (username,password,role,name) values (@username,@password,@role,@name)", cn);
                cm.Parameters.AddWithValue("@username", txtUserName.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                cm.Parameters.AddWithValue("@role", cRole.Text);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.ExecuteNonQuery();

                cn.Close();
                MessageBox.Show("New account has seved");
                Clear();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtOld1.Text != frm._pass)
                {
                    MessageBox.Show("Old password did not matched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (txtNew1.Text != txtReNew1.Text)
                {
                    MessageBox.Show("Confirm password did not matched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                cn.Open();
                cm = new SqlCommand("update tbluser set password = @password where username =@username", cn);
                cm.Parameters.AddWithValue("@password", txtNew1.Text);
                cm.Parameters.AddWithValue("@username", txtUser1.Text);
                cm.ExecuteNonQuery();

                cn.Close();
                MessageBox.Show("Password has been successfully changed!", "Changed Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser1.Clear();
                txtOld1.Clear();
                txtReNew1.Clear();
                txtNew1.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
