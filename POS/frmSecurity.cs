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
    public partial class frmSecurity : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public string _pass; 
        public frmSecurity()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void frmSecurity_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string _username ="",_role="",_name = "";
            try
            {
                bool found = false;
                cn.Open();
                cm = new SqlCommand("select * from tbluser where  username = @username and password = @password ", cn);
                cm.Parameters.AddWithValue("@username", txtUser.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    found = true;
                    _username = dr["username"].ToString();
                    _role = dr["role"].ToString();
                    _name = dr["name"].ToString();
                    _pass = dr["password"].ToString();
                }

                else
                {
                    found = false;
                }
                dr.Close();
                cn.Close();

                if(found)
                {
                    if (_role == "Cashier")
                    {
                        MessageBox.Show("Welcom " + _name, " Access Granted!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUser.Clear();
                        txtPassword.Clear();
                        this.Hide();
                        frmPOS frm = new frmPOS(this);
                        frm.lblname.Text = _name +" | "+_role;
                        frm.lblusername.Text = _username;
                        frm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("Welcom " + _name, " Access Granted!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUser.Clear();
                        txtPassword.Clear();
                        this.Hide();
                        Form1 frm = new Form1();
                        frm.lblrole.Text= _role;
                        frm.lblName.Text = _name;
                        frm.lblusername.Text = _username;
                        frm._pass = _pass;
                        frm._user = _username;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Username or password", " Access Denied!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("EXIT APPLICATION ? ", "EXIT", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {

        }
    }
}
