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
    public partial class frmProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        frmProductList frmlist;
        public frmProduct( frmProductList frmProduct)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmlist = frmProduct;

        }

        public void LoadCategory()
        {
            cbCategory.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select category from tblcategory", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                cbCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LoadBrand()
        {
            cbBrand.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select brand from tblbrand", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbBrand.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {

        }
        private void clear()
        {
            btnSabe.Enabled = true;
            btnUpdate.Enabled = false;
            txtPcode.Clear();
            txtDesc.Clear();
            txtPrice.Clear();
            txtBarcode.Clear();
            cbCategory.Text="";
            cbBrand.Text = "";
        }
        private void btnSabe_Click(object sender, EventArgs e)
        {

            try
            {
                if (MessageBox.Show("Are you sure you want to save this Product?", "POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //get value ids for  category and  brand

                    //for brand
                    string bid ="";
                    string cid = "";
                    cn.Open();
                    cm = new SqlCommand("select id from tblbrand where brand like  '" + cbBrand.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if(dr.HasRows)
                    {
                        bid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();
                    //for category

                    cn.Open();
                    cm = new SqlCommand("select id from tblcategory where category  like  '" + cbCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        cid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();


                    // insrt to table product
                    cn.Open();
                    cm = new SqlCommand("Insert into tblproduct (pcode,barcode,pdesc,bid,cid,price,reorder) Values(@pcode,@barcode,@pdec,@bid,@cid,@price,@reorder)", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdec", txtDesc.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price", double.Parse( txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully saved");
                    clear();
                    frmlist.loadrecords();
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this Prpduct ?", " POS, Update record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string bid = "";
                    string cid = "";
                    cn.Open();
                    cm = new SqlCommand("select id from tblbrand where  brand like '" + cbBrand.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        bid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();
                    //for category

                    cn.Open();
                    cm = new SqlCommand("select id from tblcategory where category like  '" + cbCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        cid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();
                    cn.Open();
                    cm = new SqlCommand("update tblproduct set barcode = @barcode ,pdesc = @pdec , bid=@bid , cid = @cid , price=@price ,reorder=@reorder where pcode like '" + txtPcode.Text + "'", cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdec", txtDesc.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price",Convert.ToDecimal( txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReorder.Text));

                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product has been  successfully updated");
                    clear();
                    frmlist.loadrecords();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==46)
            {
                //accept . char
            } else if(e.KeyChar==8)
            {
                //acceept backspace
            }
            else  if((e.KeyChar<48)||(e.KeyChar>57)) // asci code 48 -57 between 0-9
            {
                e.Handled = true;
            }
        }
    }
}
