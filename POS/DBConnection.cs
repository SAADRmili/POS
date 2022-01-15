using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace POS
{
    
    public class DBConnection
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private double dailysales;
        private int productline;
        private int stockonhand;
        private int critical;
      
        public String MyConnection()
        {
            string con = @"Data Source=DESKTOP-0CN63T0\SQLEXPRESS;Initial Catalog=POS_DEMO_DB;Integrated Security=True";
            return con; 

        }
        public double GetVal()
        {
            double vat = 0; 
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select vat from tblvat", cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                vat = double.Parse(dr["vat"].ToString());
            }
            dr.Close();
            cn.Close();
            return vat;
        }

        public double DailySeles()
        {
            string sdate = DateTime.Now.ToString("yyyy-MM-dd");
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select isnull(sum(total),0) as total from tblcart where sdate between '"+sdate+"' and '"+sdate+"' and status like 'Sold'  ", cn);
             dailysales = Double.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return dailysales;
        }

        public int StockOnHand()
        {
            
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select isnull(sum(qty),0) as qty from tblproduct   ", cn);
            stockonhand = int.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return stockonhand;
        }
        public int CriticalItems()
        {
            
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select count(*)  from vwcriticalitems   ", cn);
            critical = int.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return critical;
        }

        public int productLine()
        {
           
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select count(*) from tblproduct ", cn);
            productline = int.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return productline;
        }
        public string getPassword(string user)
        {
            string password="";
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("select * from tbluser where username = @username", cn);
            cm.Parameters.AddWithValue("@username", user);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                password = dr["password"].ToString();
            }
          
            dr.Close();
            cn.Close();
            return password; 
        }
    }
}
