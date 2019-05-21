﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
namespace Husky_Air
{
    public partial class AircraftView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label8.Visible = false;
                Label7.Visible = false;
            }
            else
                Label8.Visible = true;
            Label7.Visible = true;

        }
        private string CreateBody()
        {
            string body = string.Empty;
            using (StreamReader Reader = new StreamReader(Server.MapPath("~/Service2Template.html")))
            {
                body = Reader.ReadToEnd();
            }
            body = body.Replace("{Name}",Label3.Text);
            body = body.Replace("{Flight}",TextBox1.Text);
            body = body.Replace("{Date}",TextBox2.Text);
            body = body.Replace("{Source}",TextBox4.Text);
            body = body.Replace("{Destination}", TextBox5.Text);

            return body;
        }

        private string CreateBody1()//Code fill up the email template
        {
            string body = string.Empty;
            using (StreamReader Reader = new StreamReader(Server.MapPath("~/Service2Template2.html")))
            {
                body = Reader.ReadToEnd();
            }
            body = body.Replace("{Name}", Label4.Text);
            body = body.Replace("{Flight}", TextBox1.Text);
            body = body.Replace("{Date}", TextBox2.Text);
            body = body.Replace("{Source}", TextBox4.Text);
            body = body.Replace("{Destination}", TextBox5.Text);

            return body;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)// To Insert data from the gridview to textboxes
        {
            GridViewRow gr = GridView1.SelectedRow;
            TextBox10.Text = gr.Cells[4].Text;
            TextBox1.Text = gr.Cells[1].Text;
            TextBox7.Text = gr.Cells[2].Text;
            TextBox9.Text = gr.Cells[3].Text;
            


            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            string Pilotid = string.Empty;
            DateTime myDate = DateTime.ParseExact(TextBox2.Text, "dd/MM/yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);

            if (myDate>DateTime.Today)
            {
                status = "yet to depart";
            }
            else if(myDate>=DateTime.Today)
            {
                status = "On route";
            }
            else if(myDate<DateTime.Today)
            {
                status = "Complete";
            }
            Pilotid = DropDownList2.SelectedValue;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            
            string query = "Delete from Aircraft1 where id='"+ TextBox10.Text+"'";
            SqlCommand com = new SqlCommand(query,con);
            string Name = "Select Name from Patient where Emailid='"+TextBox6.Text+"'";
            SqlCommand com5 = new SqlCommand(Name,con);
            string PilotName = "Select Name from Pilot where Emailid='"+DropDownList2.SelectedItem+"'";

            SqlCommand com6 = new SqlCommand(PilotName,con);
            Label4.Text = com6.ExecuteScalar().ToString();
            //string Date = "Select Date from Services where id='"+DropDownList1.SelectedItem+"'";

            //SqlCommand comm = new SqlCommand(Date,con);
            //TextBox11.Text =comm.ExecuteScalar().ToString();
            

            
          
          
            string query2 = "Insert into Stats(Date,Time,Source,Destination,status,Name,id,Type,Engine,Pilotid,Orderid)values(@Date,@Time,@Source,@Destination,@status,@Name,@id,@Type,@Engine,@Pilotid,@Orderid)";
            SqlCommand com1 = new SqlCommand(query2,con);
            com1.Parameters.AddWithValue("@Date",TextBox2.Text);
            com1.Parameters.AddWithValue("@Time",TextBox3.Text);
            com1.Parameters.AddWithValue("@Source",TextBox4.Text);
            com1.Parameters.AddWithValue("@Destination",TextBox5.Text);
            com1.Parameters.AddWithValue("@status",status);
            com1.Parameters.AddWithValue("@Name",TextBox1.Text);
            com1.Parameters.AddWithValue("@id",TextBox10.Text);
            com1.Parameters.AddWithValue("@Type",TextBox9.Text);
            com1.Parameters.AddWithValue("@Engine",TextBox7.Text);
            com1.Parameters.AddWithValue("@Pilotid",Pilotid);
            com1.Parameters.AddWithValue("@Orderid",DropDownList1.SelectedValue);
            
            com1.ExecuteNonQuery();
            Label3.Text = com5.ExecuteScalar().ToString();

            MailMessage mail = new MailMessage(Label1.Text,TextBox6.Text);
            mail.Subject = "Your request is Confirmed";
            mail.Body = CreateBody();/*"Here is your Flight details \n" +"Name ="+ TextBox1.Text+"\n" + "Date of Flight =" +TextBox2.Text+"Source Station="+TextBox4.Text+"Destination Station ="+TextBox5.Text;*/

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

            MailMessage mail1 = new MailMessage(Label1.Text,Pilotid);
            mail1.Subject = "Client request ";
            mail1.Body = CreateBody1(); /*"Here is your Flight details \n" +"Name = "+ TextBox1.Text+"\n" + "Date of Flight = " +TextBox2.Text+"Source Station = "+TextBox4.Text+"Destination Station = "+TextBox5.Text;*/
            mail1.IsBodyHtml = true;
            SmtpClient smtp1 = new SmtpClient();
            smtp1.Host = "smtp.gmail.com";
            smtp1.EnableSsl = true;

            NetworkCredential networkCred1 = new NetworkCredential(Label1.Text,Label2.Text);
            smtp1.UseDefaultCredentials = true;
            smtp1.Credentials = networkCred1;
            smtp1.Port = 587;
            smtp1.Send(mail1);
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
            SqlConnection con = NewMethod();
            con.Open();
            string query = "Select Emailid from Services where id='" + DropDownList1.SelectedItem + "'";
            string query1 = "Select source from Services where id='" + DropDownList1.SelectedValue + "'";
            string query2 = "Select destination from Services where id='" + DropDownList1.SelectedValue + "'";
            string Date = "Select Date from Services where id='" + DropDownList1.SelectedItem + "'";
            SqlCommand com = new SqlCommand(query, con);
            SqlCommand com1 = new SqlCommand(query1, con);
            SqlCommand com2 = new SqlCommand(query2, con);
            SqlCommand comm = new SqlCommand(Date, con);
            Label8.Text = comm.ExecuteScalar().ToString();
            TextBox6.Text = com.ExecuteScalar().ToString();
            TextBox4.Text = com1.ExecuteScalar().ToString();
            TextBox5.Text = com2.ExecuteScalar().ToString();

            con.Close();
        }

        private static SqlConnection NewMethod()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            TextBox2.Text = Calendar1.SelectedDate.ToString("d");
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