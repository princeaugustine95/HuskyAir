using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace Husky_Air
{
    public partial class Pilotlogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            string query = "Select count(*) from Pilot where Emailid='"+TextBox1.Text+"'";
            SqlCommand com = new SqlCommand(query,con);
            int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
            con.Close();
            if (temp==1)
            {
                con.Open();
                string checkpassword = "select passowrd from Pilot where Emailid='"+TextBox1.Text+"'";
                SqlCommand com1 = new SqlCommand(checkpassword,con);
                string password = com1.ExecuteScalar().ToString().Replace(" ","");
                if(password==TextBox2.Text)
                {
                    Session["New"] = TextBox1.Text;
                    Response.Write("password is corect");

                }
                else
                {
                    Response.Write("password is wrong");
                }
            }
            else
            {
                Response.Write("user is not registered ");

            }
        
        }
    }
}