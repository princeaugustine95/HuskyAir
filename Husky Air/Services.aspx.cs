using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;


namespace Husky_Air
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            string query = "Insert into Services(Date,No_of_Passengers,source,destination,Emailid)values(@Date,@No_of_Passengers,@source,@destination,@Emailid)";
            SqlCommand com= new SqlCommand(query,con);
            com.Parameters.AddWithValue("Date",TextBox1.Text);
            com.Parameters.AddWithValue("@No_of_Passengers",TextBox5.Text);
            com.Parameters.AddWithValue("@source",TextBox2.Text);
            com.Parameters.AddWithValue("@destination",TextBox3.Text);
            com.Parameters.AddWithValue("@Emailid",TextBox4.Text);
            com.ExecuteNonQuery();

            string query1 = "select id from Services where Emailid='"+TextBox1.Text+"'";
            SqlCommand com1 = new SqlCommand(query1,con);
            int requestid = Convert.ToInt32( com1.ExecuteNonQuery());
            
            MailMessage Mail = new MailMessage(Label1.Text, TextBox4.Text)
            {
                Subject = "Your Request Confirmed",
                Body = "You will receive the details of the flight shortly \n.Your Request Id is"+ requestid ,
                IsBodyHtml = false
            };

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;

            NetworkCredential networkcred = new NetworkCredential(Label1.Text, "96prince@96");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkcred;
            smtp.Port = 587;
            smtp.Send(Mail);
            Response.Write("Your Request has been Sent Check your Mail ");
            Response.Redirect("Index.aspx");

            con.Close();


        }
    }
}