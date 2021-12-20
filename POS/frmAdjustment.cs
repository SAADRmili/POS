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
    public partial class frmAdjustment : Form
    {
        Form1 frm;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        int _qty = 0;
        public frmAdjustment(Form1 f)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frm = f; 
        }
        public void RefernceNo()
        {
            Random rn = new Random();

            txtRef.Text = rn.Next().ToString();
        }
        public void loadrecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select p.pcode,p.barcode,p.pdesc,b.brand,c.category,p.price, p.qty from  tblproduct as p inner join tblbrand as b on b.id = p.bid inner join tblcategory as c on c.id = p.cid  WHERE p.pdesc LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String colName = dataGridView1.Columns[e.ColumnIndex].Name; 
            if(colName == "SELECT")
            {
                txtPcode.Text = dataGridView1[1, e.RowIndex].Value.ToString();
                txtDescription.Text = dataGridView1[3, e.RowIndex].Value.ToString();
                _qty = int.Parse( dataGridView1[7, e.RowIndex].Value.ToString());
                
            }
        }

        private void txtRef_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboAction_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtRemarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSabe_Click(object sender, EventArgs e)
        {
            try
            {
               //validation for empty fields

                if(int.Parse(txtQty.Text) > _qty)
                {
                    MessageBox.Show("STOCK ON HAND QUANTITY SHOULD BE GEATER THAN THE ADJUST QTY", "VALIDATION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

               //update stack 

                if(cboAction.Text == "REMOVE FROM INVENTORY")
                {
                    SqlStatement("update tblproduct set qty =(qty-" + int.Parse(txtQty.Text) + " ) where pcode like '" + txtPcode.Text + "'");
                    clear();
                    loadrecords();
                }
                else if(cboAction.Text == "ADD TO INVENTORY")
                {
                    SqlStatement("insert into  tblAdjustment (referenceno,pcode,qty,action,remarks,sdate,[user])  values ('" + txtRef.Text + "','" + txtPcode.Text + "'," + int.Parse(txtQty.Text) + ",'" + cboAction.Text + "','" + txtRemarks.Text + "','" + DateTime.Now.ToShortDateString() + "','" + txtUser.Text + "')");
                    MessageBox.Show("STOCK HAS BEEN SUCESSFULLY ADJUSTED ", "PROCESS COMPLETE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadrecords();
                    clear();
                
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
            txtDescription.Clear();
            txtPcode.Clear();
            txtQty.Clear();
            txtRemarks.Clear();
            txtRef.Clear();
            cboAction.Text = "";
            RefernceNo();
        }

        public void SqlStatement(string sql)
        {
            cn.Open();
            cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                loadrecords();
            }
        }
    }
}
