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
    public partial class Register : System.Web.UI.Page
    {
        // Connection string
        string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Basic Validation
            if (txtPassword.Text != txtConfirmPass.Text)
            {
                lblMessage.Text = "Passwords do not match.";
                lblMessage.Visible = true;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 2. Check if Email already exists
                string checkSql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    lblMessage.Text = "This email is already registered.";
                    lblMessage.Visible = true;
                    return;
                }

                // 3. Insert New User
                // We use NEWID() for the Id and hardcode 'Customer' for the Role
                string sql = "INSERT INTO Users (Id, Email, Password, Role) VALUES (NEWID(), @Email, @Password, 'Customer')";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text); // Note: In a real app, hash this password!

                cmd.ExecuteNonQuery();

                // 4. Redirect to Login
                Response.Redirect("login.aspx");
            }
        }
    }
}