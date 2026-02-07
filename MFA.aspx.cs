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
            if (Session["TempUserId"] == null) Response.Redirect("Login.aspx");
        }

        protected void btnVerifyEmail_Click(object sender, EventArgs e)
        {
            string userCode = txtEmailCode.Text.Trim();
            string serverCode = Session["EmailOTP"] as string;

            if (userCode == serverCode)
            {
                bool needsApp = (bool)Session["NeedsAppVerification"];

                if (needsApp)
                {
                    pnlEmail.Visible = false;
                    pnlApp.Visible = true;
                    lblMessage.Text = "";
                }
                else
                {
                    PerformLogin();
                }
            }
            else
            {
                lblMessage.Text = "Incorrect Email Code.";
            }
        }

        protected void btnVerifyApp_Click(object sender, EventArgs e)
        {
            string userId = Session["TempUserId"].ToString();
            string userCode = txtAppCode.Text.Trim();
            string secretKey = GetUserSecret(userId);

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool isCorrect = tfa.ValidateTwoFactorPIN(secretKey, userCode);

            if (isCorrect)
            {
                PerformLogin();
            }
            else
            {
                lblMessage.Text = "Incorrect App Code.";
            }
        }

        private void PerformLogin()
        {
            Session["UserId"] = Session["TempUserId"];
            Session["Role"] = Session["TempRole"];

            Session.Remove("TempUserId");
            Session.Remove("TempRole");
            Session.Remove("EmailOTP");
            Session.Remove("NeedsAppVerification");

            Response.Redirect("/");
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