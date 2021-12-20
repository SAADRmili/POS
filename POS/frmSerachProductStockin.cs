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
    public partial class frmSerachProductStockin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        frmStockin slist;
        public frmSerachProductStockin ( frmStockin flist)
        {
            InitializeComponent();
            slist = flist;
            cn = new SqlConnection(dbcon.MyConnection());
           
        }
        public void loadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select  pcode,pdesc,qty from tblproduct where  pdesc like '" + txtSearch.Text + "%' order by pdesc", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
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
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                if (slist.txtRef.Text == string.Empty)
                {
                    MessageBox.Show("Please enter reference no", "POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    slist.txtRef.Focus();
                    return;
                }
                if (slist.txtStockby.Text == string.Empty)
                {
                    MessageBox.Show("Please enter stock in by", "POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    slist.txtStockby.Focus();
                    return;
                }
                if (MessageBox.Show("Add this item?", "POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("insert  into tblstockin (refno,pcode,sdate,stockinby,vendorid) Values (@refno,@pcode,@sdate,@stockinby,@vendorid)", cn);
                    cm.Parameters.AddWithValue("@refno", slist.txtRef.Text);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.Parameters.AddWithValue("@sdate", slist.dateStockin.Value);
                    cm.Parameters.AddWithValue("@stockinby", slist.txtStockby.Text);
                    cm.Parameters.AddWithValue("@vendorid", slist.lblVendorId.Text);
                    cm.ExecuteNonQuery();

                    cn.Close();

                    MessageBox.Show("Successfully added", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //  clear();
                    slist.loadStockIn();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadProduct();
        }

        private void frmSerachProductStockin_Load(object sender, EventArgs e)
        {

        }
    }
}
