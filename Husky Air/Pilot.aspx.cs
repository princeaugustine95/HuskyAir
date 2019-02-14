using System;
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

namespace Husky_Air
{
    public partial class Pilot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYears();
                LoadMonths();
                Calendar1.Visible = false;
            }
            else
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
                conn.Open();
                string query1 = "select count(*) from Pilot where Emailid='" + TextBox1.Text + "'";
                SqlCommand command = new SqlCommand(query1, conn);
                int temp = Convert.ToInt32(command.ExecuteScalar().ToString());
                if(temp==1)
                {
                    Response.Write("User already registered");
                }
                conn.Close();
            }
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

        private void LoadYears()
        {
            DataSet DsYears = new DataSet();
            DsYears.ReadXml(Server.MapPath("~/Data/Years.xml"));
            DropDownList2.DataSource = DsYears;
            DropDownList2.DataTextField = "Number";
            DropDownList2.DataValueField = "Number";
            DropDownList2.DataBind();
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
                Gender = "female";
            }
            else if(RadioButton3.Checked)
            {
                Gender = "Other";
            }

            if (TextBox9.Text != null)
            {

                string input = TextBox9.Text;

                MD5 md5 = System.Security.Cryptography.MD5.Create();

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)

                {

                    sb.Append(hash[i].ToString("x2"));

                }
            


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HuskyAirConnectionString"].ConnectionString);
            con.Open();
            string query1 = "select count(*) from Pilot where Emailid='" + TextBox1.Text + "'";
            SqlCommand command = new SqlCommand(query1, con);
            int temp = Convert.ToInt32(command.ExecuteScalar().ToString());
            con.Close();
            if (temp == 1)
            {
                Response.Write("Cant Register as user is already registered");
            }
            else
            {
                con.Open();
                string query = "Insert into Pilot(Emailid,Name,Gender,Dob,Address,Phoneno,Hrs,Certification,Rating,Passowrd)values(@Emailid,@Name,@Gender,@Dob,@Address,@Phoneno,@Hrs,@Certification,@Rating,@Passowrd)";
                SqlCommand com = new SqlCommand(query, con);
                com.Parameters.AddWithValue("@Emailid", TextBox1.Text);
                com.Parameters.AddWithValue("@Name", TextBox2.Text);
                com.Parameters.AddWithValue("@Gender", Gender);
                com.Parameters.AddWithValue("@Dob", TextBox3.Text);
                com.Parameters.AddWithValue("@Address", TextBox4.Text);
                com.Parameters.AddWithValue("@Phoneno", TextBox5.Text);
                com.Parameters.AddWithValue("@Hrs", TextBox6.Text);
                com.Parameters.AddWithValue("@Certification", TextBox7.Text);
                com.Parameters.AddWithValue("@Rating", TextBox8.Text);
                com.Parameters.AddWithValue("@Passowrd", sb.ToString());
                com.ExecuteNonQuery();
                Response.Write("Data Storted Successfully");
                con.Close();
            }
            }


        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            if (Calendar1.Visible)
            {
                Calendar1.Visible = false;
            }
            else
                Calendar1.Visible = true;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(DropDownList1.SelectedValue);
            int year = Convert.ToInt32(DropDownList2.SelectedValue);
            Calendar1.VisibleDate = new DateTime(year,month,1);
            Calendar1.SelectedDate=new DateTime(year, month, 1);
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
            TextBox3.Text = Calendar1.SelectedDate.ToString("d");
            Calendar1.Visible = false;
        }
    }
}