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
                // Requirement: Authentication
                // NOTE: In production, ALWAYS compare Hashed passwords, never plain text.
                string sql = "SELECT Id, Role FROM Users WHERE Email = @Email AND Password = @Password";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPass.Text); // Assuming plain text for simplicity of snippet

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Set Session
                    Session["UserId"] = reader["Id"];
                    Session["Role"] = reader["Role"]; // 'Admin' or 'User'

                    // Requirement: MFA Check (Mock logic)
                    if (RequiresMFA(txtEmail.Text))
                    {
                        Response.Redirect("mfa_verify.aspx"); // You would create this page for the code check
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
            // Logic to check if user has MFA enabled in DB
            return false; // Default false for basic flow
        }
    }
}