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
    public partial class frmLookUp : Form
    {
        frmPOS f; 
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        public frmLookUp(frmPOS frmpos)
        {
            InitializeComponent();
            f = frmpos;
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
         
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadrecords();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                frmQty frm = new frmQty(f);
               
                
                frm.ProductDeatails(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), Double.Parse( dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()), f.lblTransno.Text, int.Parse( dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString())); ;
                frm.ShowDialog();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmLookUp_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
