using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace _240795P_EvanLim
{
    public partial class login : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["mode"] == "auto_logout")
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();

                if (Request.Cookies[".ASPXAUTH"] != null)
                {
                    HttpCookie myCookie = new HttpCookie(".ASPXAUTH");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }

                Response.End();
                return;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Id, Role FROM Users WHERE Email = @Email AND Password = @Password";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPass.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Session["UserId"] = reader["Id"];
                    Session["Role"] = reader["Role"];

                    if (RequiresMFA(txtEmail.Text))
                    {
                        Response.Redirect("MFA");
                    }
                    else
                    {
                        Response.Redirect("/");
                    }
                }
                else
                {
                    lblError.Text = "Invalid email or password.";
                }
            }
        }

        private bool RequiresMFA(string email)
        {
            return false; // Default false
        }
    }
}