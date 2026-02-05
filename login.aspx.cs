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
                        Response.Redirect("mfa_verify.aspx");
                    }
                    else
                    {
                        Response.Redirect("mainPg.aspx");
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