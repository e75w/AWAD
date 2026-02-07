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
            if (Session["UserId"] == null) Response.Redirect("login");

            if (!IsPostBack)
            {
                GenerateQRCode();
            }
        }

        private void GenerateQRCode()
        {
            string secretKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            Session["TempSecret"] = secretKey;

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("MusicStore", Session["UserId"].ToString(), secretKey, false, 300);

            imgQRCode.ImageUrl = setupInfo.QrCodeSetupImageUrl;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string secretKey = Session["TempSecret"].ToString();
            string userCode = txtCode.Text;

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool isCorrect = tfa.ValidateTwoFactorPIN(secretKey, userCode);

            if (isCorrect)
            {
                SaveSecretToDB(secretKey);

                Session["MFA_Status"] = "Enabled";

                Response.Redirect("Profile");
            }
            else
            {
                lblMessage.Text = "Invalid Code. Please try again.";
                lblMessage.CssClass = "text-danger fw-bold";
            }
        }

        private void SaveSecretToDB(string secret)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Users SET TwoFactorSecret = @Secret, IsTwoFactorEnabled = 1 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Secret", secret);
                cmd.Parameters.AddWithValue("@Id", Session["UserId"]);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}