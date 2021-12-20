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
    public partial class frmProductList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmProductList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            
        }

        public void loadrecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select p.pcode,p.barcode,p.pdesc,b.brand,c.category,p.price, p.reorder from  tblproduct as p inner join tblbrand as b on b.id = p.bid inner join tblcategory as c on c.id = p.cid  WHERE p.pdesc LIKE '" + txtSearch.Text+"%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct(this);
            frm.btnSabe.Enabled = true;
            frm.btnUpdate.Enabled = false;
            frm.LoadCategory();
            frm.LoadBrand();
            frm.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                frmProduct frm = new frmProduct(this);
                frm.btnSabe.Enabled = false;
                frm.txtPcode.Text = dataGridView1[1, e.RowIndex].Value.ToString();

                frm.txtBarcode.Text = dataGridView1[2, e.RowIndex].Value.ToString();
                frm.txtDesc.Text = dataGridView1[3, e.RowIndex].Value.ToString();
               
                frm.txtPrice.Text = dataGridView1[6, e.RowIndex].Value.ToString();

                frm.cbBrand.Text = dataGridView1[4, e.RowIndex].Value.ToString();
                frm.cbCategory.Text = dataGridView1[5, e.RowIndex].Value.ToString();
                frm.txtReorder.Text= dataGridView1[7, e.RowIndex].Value.ToString();
                frm.LoadCategory();
                frm.LoadBrand();
                frm.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure to want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblproduct where pcode like '" + dataGridView1[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product  has been  successfully deleted.", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadrecords();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadrecords();
        }
    }
}