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
using System.Globalization;

namespace POS
{
    public partial class frmPOS : Form
    {
        String id;
        String price;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        int qty;
        //private String pcode;
        //private double price;
        //private string transno;
        frmSecurity f;
        public frmPOS(frmSecurity frs)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            lblDate.Text = DateTime.Now.ToLongDateString();
            this.KeyPreview = true;
            f = frs;
        }

        public void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string transo;
                int count; 
                cn.Open();
                cm = new SqlCommand("select top 1 transno from tblCart where transno like '" + sdate+ "%' order by id desc", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    transo = dr[0].ToString();
                    count = int.Parse(transo.Substring(8,4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transo = sdate + "1001";
                    lblTransno.Text = transo;
                }
                dr.Close();
                cn.Close();
                
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0)
            {
                return;
            }
            GetTransNo();
            txtSearch.Enabled = true;
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text == String.Empty)
                {
                    return;
                }
                else
                {
                    String _pcode;
                    double _price;
                    int _qty;
                    cn.Open();
                    cm = new SqlCommand("select * from tblproduct where barcode like '" + txtSearch.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                       
                        _pcode = dr["pcode"].ToString();
                        _price = Double.Parse(dr["price"].ToString());
                        _qty = int.Parse(txtQty.Text);


                        dr.Close();
                        cn.Close();
                        AddToCart(_pcode, _price, _qty);

                        txtSearch.Clear();
                        txtSearch.Focus();

                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //cn.Close();
                MessageBox.Show(ex.Message, "POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
     
        private void AddToCart(string _pcode , double _price,int _qty)
        {
            bool found = false;
            int cart_qty = 0;
            cn.Open();
            cm = new SqlCommand("select * from tblcart where  transno =@transno and  pcode =@pcode", cn);
            cm.Parameters.AddWithValue("@transno", lblTransno.Text);
            cm.Parameters.AddWithValue("@pcode", _pcode);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                found = true;
                id = dr["id"].ToString();
                cart_qty = int.Parse(dr["qty"].ToString());
            }
            else
            {
                found = false;
            }
            dr.Close();
            cn.Close();
            if (found)
            {
                if (qty < int.Parse(txtQty.Text) + cart_qty)
                {
                    MessageBox.Show("Unable to procced, Remaining qty on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cn.Open();
                cm = new SqlCommand(" update tblcart set qty =( qty + " + _qty+ ") where id = " + id + " ", cn);

                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Item has been added in cart", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
                loadCart();

            }
            else
            {
                if (qty < int.Parse(txtQty.Text))
                {
                    MessageBox.Show("Unable to procced, Remaining qty on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cn.Open();
                cm = new SqlCommand("Insert  into tblcart (transno,pcode,price,qty,sdate,cashier) values(@transno,@pcode,@price,@qty,@sdate,@cashier)", cn);
                cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                cm.Parameters.AddWithValue("@pcode", _pcode);
                cm.Parameters.AddWithValue("@price", _price);
                cm.Parameters.AddWithValue("@qty", _qty);
                cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                cm.Parameters.AddWithValue("@cashier", lblusername.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Item has been added in cart", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSearch.SelectionStart = 0;

                txtSearch.SelectionLength = txtSearch.Text.Length;
                loadCart();
               
            }
        }

        public void loadCart ()
        {
            try
            {
                Boolean hasrecord = false;
                int i = 0;
                double total = 0;
                double disc = 0;
                dataGridView1.Rows.Clear(); 
                cn.Open();
                cm = new SqlCommand("select c.id,c.pcode , p.pdesc ,c.price,c.qty,c.disc,c.total from tblCart as c inner join tblProduct as p on p.pcode = c.pcode where transno like '"+lblTransno.Text+"' and  status like 'Pending'", cn);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    disc += Double.Parse(dr["disc"].ToString());
                    dataGridView1.Rows.Add(i, dr["id"].ToString(),dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(),Double.Parse( dr["total"].ToString()).ToString(),"[ + ]","[ - ]");
                    hasrecord = true;
                }
                dr.Close();
                cn.Close();
                lbltotal.Text = total.ToString();
                lbldiscount.Text = disc.ToString();
                GetCartTotal();
                if(hasrecord == true)
                {
                    btnSettle.Enabled = true;
                    btnDiscount.Enabled = true;
                    btnCancel.Enabled = true;
                }
                else
                {
                    btnSettle.Enabled = false;
                    btnDiscount.Enabled = false;
                    btnCancel.Enabled = false;
                }
            }
            catch(Exception ex )
            {
                cn.Close();
                MessageBox.Show(ex.Message, "POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(lblTransno.Text == "000000000000000000")
            {
                return; 
            }
            frmLookUp frm = new frmLookUp(this);
            frm.loadrecords();
            frm.ShowDialog();
        }

        private void lblTransno_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if(MessageBox.Show("Remove this item?","POS",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblcart where  id like '"+dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()+"'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    MessageBox.Show("Item has successfully removed ", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadCart();
                }
            }
            else if(colName == "colAdd")
            {
                int i = 0;
               cn.Open();
                cm = new SqlCommand("select sum(qty) as qty  from tblproduct where  pcode like '"+ dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' group by pcode", cn);
               
                i = int.Parse(cm.ExecuteScalar().ToString());
                
                cn.Close();

                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString())<= i)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblcart set qty = qty + " + int.Parse(txtQty.Text) + "where transno like '" + lblTransno.Text + "' and pcode like'" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadCart();
                }
                else
                {
                    MessageBox.Show("Remaining qty on hand is " + i + " !", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                    
            }
            else if(colName == "colRemove")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select sum(qty) as qty  from tblcart where  pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'  and transno like '"+lblTransno.Text+"' group by pcode", cn);

                i = int.Parse(cm.ExecuteScalar().ToString());

                cn.Close();

                if (i > 1)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblcart set qty = qty - " + int.Parse(txtQty.Text) + "where transno like '" + lblTransno.Text + "' and pcode like'" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadCart();
                }
                else
                {
                    MessageBox.Show("Remaining qty on cart is " + i + " !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public  void GetCartTotal()
        {
           
            double discount = Double.Parse(lbldiscount.Text);
            double sales = double.Parse(lbltotal.Text);
            Double vat = sales * dbcon.GetVal();
            Double vatable = sales - vat;
            
            lblVat.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatable.ToString("#,##0.00");
            lblDisplayTotal.Text = sales.ToString("#, ##0.00");

        }


        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbltotal_Click(object sender, EventArgs e)
        {

        }

        private void lbldiscount_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount(this);
            frm.lblID.Text = id;
            frm.txtPrice.Text = price;
            frm.ShowDialog();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index;
            id = dataGridView1[1, i].Value.ToString();
            price = dataGridView1[7, i].Value.ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CultureInfo fr = System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR");
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss",fr);
            lblDate1.Text = DateTime.Now.ToLongDateString();
        }

        private void btnSettle_Click(object sender, EventArgs e)
        {
            frmSettle frm = new frmSettle(this);
            frm.txtSale.Text = lblDisplayTotal.Text;
            frm.ShowDialog();

        }

        private void frmPOS_Load(object sender, EventArgs e)
        {

        }

        private void btnDaily_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();
            frm.dt1.Enabled = false;
            frm.dt2.Enabled = false;
            frm.suser = lblusername.Text;
            frm.cboCashier.Enabled = false;
            frm.cboCashier.Text = lblusername.Text;
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0)
            {
                MessageBox.Show("Unable to logout , Please cancel the transaction", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(MessageBox.Show("Logout Application!","POS",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Hide();
                frmSecurity frm = new frmSecurity();
                frm.ShowDialog();
            }
        }

        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.F1)
            {
                btnNew_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnSearch_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnDiscount_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F4)
            {
                btnSettle_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnCancel_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F6)
            {
                btnDaily_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F7)
            {
                btnChangePas_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F8)
            {
                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
            }
            else if (e.KeyCode == Keys.F10)
            {
                btnClose_Click(sender, e);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Remove all items from cart?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("delete from tblcart where transno like '" + lblTransno.Text + "'", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("All items has been successfully removed", "Remove item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadCart();
            }
        }

        private void btnChangePas_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(this);
            frm.ShowDialog();
        }
    }
}
