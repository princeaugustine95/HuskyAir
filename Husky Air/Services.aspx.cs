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
using System.IO;

namespace Husky_Air
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Label5.Visible = false;
                Label6.Visible = false;
            }
        }

        private string CreateBody()
        {
            string body = string.Empty;
            using (StreamReader reader=new StreamReader(Server.MapPath("~/ServiceTemplate.html")) )
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}",Label3.Text);
            body = body.Replace("{id}",Label4.Text);
            return body;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            string query2 = "select count(*) from Services where Emailid='"+TextBox4.Text+"' ";
            string query3 = "select count(*) from Patient where Emailid='"+TextBox4.Text+"'";
            string query4 = "select Name from Patient where Emailid='"+TextBox4.Text+"'";
            SqlCommand com4 = new SqlCommand(query4,con);
            SqlCommand com3 = new SqlCommand(query3,con);
            SqlCommand com2 = new SqlCommand(query2,con);
            int temp1 = Convert.ToInt32(com3.ExecuteScalar().ToString());
            int temp = Convert.ToInt32(com2.ExecuteScalar().ToString());
            if (temp == 0  && temp1!= 0)
            {
                Label3.Text = com4.ExecuteScalar().ToString();
                
                string query = "Insert into Services(Date,No_of_Passengers,source,destination,Emailid)values(@Date,@No_of_Passengers,@source,@destination,@Emailid)";
                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.AddWithValue("Date", TextBox1.Text);
                com.Parameters.AddWithValue("@No_of_Passengers", TextBox5.Text);
                com.Parameters.AddWithValue("@source", DropDownList1.SelectedValue);
                com.Parameters.AddWithValue("@destination", DropDownList2.SelectedValue);
                com.Parameters.AddWithValue("@Emailid", TextBox4.Text);
                com.ExecuteNonQuery();


                string query1 = "select id from Services where Emailid='" + TextBox4.Text + "'";
                SqlCommand com1 = new SqlCommand(query1, con);
                Label4.Text= com1.ExecuteScalar().ToString();


                MailMessage Mail = new MailMessage(Label1.Text, TextBox4.Text)
                {
                    Subject = "Your Request Confirmed",
                    Body = CreateBody(),
                    IsBodyHtml = true
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
            else
            {
                Response.Write("You already requested the service or you are not registered Patient");
            }

            Label5.Visible = true;
            Label6.Visible = true;

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            TextBox1.Text = Calendar1.SelectedDate.ToString("d");
            Calendar1.Visible = false;
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            if(e.Day.Date<DateTime.Now.Date)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Blue;
            }
        }
    }
}