using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Authenticator;

namespace _240795P_EvanLim
{
    public partial class MFA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TempUserId"] == null) Response.Redirect("login");
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "Please log in again to generate a new code.";
            lblMessage.CssClass = "text-warning fw-bold";
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string userCode = txtOTP.Text.Trim();
            bool isVerified = false;

            if (Session["AuthMode"].ToString() == "App")
            {
                string secretKey = GetUserSecret(Session["TempUserId"].ToString());
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                isVerified = tfa.ValidateTwoFactorPIN(secretKey, userCode);
            }
            else
            {
                string serverCode = Session["OTP"] as string;
                isVerified = (userCode == serverCode);
            }

            if (isVerified)
            {
                Session["UserId"] = Session["TempUserId"];
                Session["Role"] = Session["TempRole"];

                // Cleanup
                Session.Remove("TempUserId");
                Session.Remove("TempRole");
                Session.Remove("OTP");
                Session.Remove("AuthMode");

                Response.Redirect("/");
            }
            else
            {
                lblMessage.Text = "Incorrect Code.";
            }
        }

        private string GetUserSecret(string userId)
        {
            string secret = "";
            string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT TwoFactorSecret FROM Users WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", userId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value) secret = result.ToString();
            }
            return secret;
        }
    }
}