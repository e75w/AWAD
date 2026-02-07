using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using Google.Authenticator;
using QRCoder;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace _240795P_EvanLim
{
    public partial class Setup2FA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                GenerateQRCode();
            }
        }

        private void GenerateQRCode()
        {
            // 1. Generate a random Secret Key
            string secretKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            Session["TempSecret"] = secretKey; // Save temporarily

            // 2. Create QR Code
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("MusicStore", Session["UserId"].ToString(), secretKey, false, 300);

            imgQRCode.ImageUrl = setupInfo.QrCodeSetupImageUrl;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            // 3. Validate the code to ensure they scanned it correctly
            string secretKey = Session["TempSecret"].ToString();
            string userCode = txtCode.Text;

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool isCorrect = tfa.ValidateTwoFactorPIN(secretKey, userCode);

            if (isCorrect)
            {
                // 4. Save Secret Key to Database
                SaveSecretToDB(secretKey);
                lblMessage.Text = "Success! You can now use the App to login.";
                lblMessage.CssClass = "text-success";
            }
            else
            {
                lblMessage.Text = "Invalid Code. Please try again.";
                lblMessage.CssClass = "text-danger";
            }
        }

        private void SaveSecretToDB(string secret)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Users SET TwoFactorSecret = @Secret, TwoFactorEnabled = 1 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Secret", secret);
                cmd.Parameters.AddWithValue("@Id", Session["UserId"]);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}