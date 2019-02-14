using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
namespace Husky_Air
{
    public partial class AircraftView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gr = GridView1.SelectedRow;
            TextBox1.Text = gr.Cells[1].Text;
            TextBox7.Text = gr.Cells[4].Text;

            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            string query = "Delete from Aircraft1 where id='"+ TextBox7.Text+"'";
            SqlCommand com = new SqlCommand(query,con);


            MailMessage mail = new MailMessage(Label1.Text,TextBox6.Text);
            mail.Subject = "Your request is Confirmed";
            mail.Body = "Here is your Flight details \n" +"Name ="+ TextBox1.Text+"\n" + "Date of Flight =" +TextBox2.Text+"Source Station="+TextBox4.Text+"Destination Station ="+TextBox5.Text;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;

            NetworkCredential networkcred = new NetworkCredential(Label1.Text, Label2.Text);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkcred;
            smtp.Port = 587;
            smtp.Send(mail);
            Response.Write("Service Granted ");
            com.ExecuteNonQuery();
            con.Close();

            HttpCookie cookie = new HttpCookie("Aircraft");
            cookie["Name"] = TextBox1.Text;
            cookie["Date"] = TextBox2.Text;
            cookie["Time"] = TextBox3.Text;
            cookie["Source"] = TextBox4.Text;
            cookie["Destination"] = TextBox5.Text;

            cookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(cookie);
            Response.Redirect("Stats.aspx");


        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString1"].ConnectionString);
            con.Open();
            string query = "Select Emailid from Services where id='"+ DropDownList1.SelectedItem+"'";
            SqlCommand com = new SqlCommand(query,con);

            TextBox6.Text = com.ExecuteNonQuery().ToString();

                con.Close();
        }
    }
}