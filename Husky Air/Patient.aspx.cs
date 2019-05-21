﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;

namespace Husky_Air
{
    public partial class Patient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMonths();
                LoadYears();
                Calendar1.Visible = true;
            }
            else
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
                conn.Open();
                string query1 = "select count(*) from Patient where Emailid='" + TextBox3.Text + "'";
                SqlCommand command = new SqlCommand(query1, conn);
                int temp = Convert.ToInt32(command.ExecuteScalar().ToString());
                if (temp == 1)
                {
                    Response.Write("User already registered");
                }
                conn.Close();
            }

        }

        private void LoadYears()
        {
            DataSet DsYears = new DataSet();
            DsYears.ReadXml(Server.MapPath("~/Data/Years.xml"));
            DropDownList2.DataSource = DsYears;
            DropDownList2.DataTextField = "Number";
            DropDownList2.DataValueField = "Number";
            DropDownList2.DataBind();
        }

        private void LoadMonths()
        {
            DataSet DsMonths = new DataSet();
            DsMonths.ReadXml(Server.MapPath("~/Data/Months.xml"));
            DropDownList1.DataSource = DsMonths;
            DropDownList1.DataTextField = "Name";
            DropDownList1.DataValueField = "Number";
            DropDownList1.DataBind();
        }

        private string CreateBody()
        {
           
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Username}", TextBox3.Text);
            body = body.Replace("{Password}", TextBox7.Text);
            body = body.Replace("{User}", TextBox1.Text);
            return body;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string Gender = string.Empty;
            if(RadioButton1.Checked)
            {
                Gender = "Male";
            }
            else if(RadioButton2.Checked)
            {
                Gender = "Female";
            }
            else if(RadioButton3.Checked)
            {
                Gender = "Other";
            }

            string input = TextBox7.Text;
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            int temp = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            string count = "Select count(*) from Patient where Emailid='"+TextBox3.Text+"'";
            SqlCommand comm = new SqlCommand(count,con);
            int num = Convert.ToInt32(comm.ExecuteScalar().ToString());
            if (num != 0)
            {
                Response.Write("Patient already Registered");
            }
            else
            {

                string query = "Insert into Patient(Name,Gender,Emailid,Dob,Address,Phoneno,Password,status)values(@Name,@Gender,@Emailid,@Dob,@Address,@Phoneno,@Password,@status)";
                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.AddWithValue("@Name", TextBox1.Text);
                com.Parameters.AddWithValue("@Gender", Gender);
                com.Parameters.AddWithValue("@Emailid", TextBox3.Text);
                com.Parameters.AddWithValue("@dob", TextBox4.Text);
                com.Parameters.AddWithValue("@Address", TextBox5.Text);
                com.Parameters.AddWithValue("@Phoneno", TextBox6.Text);
                com.Parameters.AddWithValue("@Password", sb.ToString());
                com.Parameters.AddWithValue("@status", temp);
                com.ExecuteNonQuery();


                MailMessage mail = new MailMessage(Label1.Text, TextBox3.Text);
                mail.Subject = "Patient Registration";
                mail.Body = CreateBody();

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential networkcred = new NetworkCredential(Label1.Text, Label2.Text);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkcred;
                smtp.Port = 587;
                smtp.Send(mail);
                Response.Write("Registered");
                Response.Write("Data Storted Successfully");

                con.Close();
            }
        }

      

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(DropDownList1.SelectedValue);
            int year = Convert.ToInt32(DropDownList2.SelectedValue);
            Calendar1.VisibleDate = new DateTime(year,month,1);
            Calendar1.SelectedDate = new DateTime(year,month,1);

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(DropDownList1.SelectedValue);
            int year = Convert.ToInt32(DropDownList2.SelectedValue);
            Calendar1.VisibleDate = new DateTime(year, month, 1);
            Calendar1.SelectedDate = new DateTime(year, month, 1);

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            TextBox4.Text = Calendar1.SelectedDate.ToString("d");
            Calendar1.Visible = false;
        }
    }
}