using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Authenticator;

namespace _240795P_EvanLim
{
    public partial class Profile : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadUserProfile();
                LoadOrderHistory();
            }
        }

        // --- LOAD DATA ---
        private void LoadUserProfile()
        {
            string userId = Session["UserId"].ToString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Email, IsTwoFactorEnabled FROM Users WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtEmail.Text = reader["Email"].ToString();

                    // Handle MFA Button Visibility
                    bool isMfa = reader["IsTwoFactorEnabled"] != DBNull.Value && Convert.ToBoolean(reader["IsTwoFactorEnabled"]);
                    if (isMfa)
                    {
                        lblMFAStatus.Text = "✅ MFA Enabled";
                        lblMFAStatus.CssClass = "text-success fw-bold";
                        btnEnableMFA.Visible = false;
                    }
                    else
                    {
                        lblMFAStatus.Text = "⚠️ Account Insecure";
                        lblMFAStatus.CssClass = "text-danger";
                        btnEnableMFA.Visible = true;
                    }
                }
            }
        }

        // --- BUTTON HANDLERS ---

        // 1. User Clicks "View" -> Show Password Verification Box
        protected void btnInitView_Click(object sender, EventArgs e)
        {
            pnlVerification.Visible = true;
            pnlMFA.Visible = false; // Hide MFA step initially
            txtVerifyPass.Focus();
        }

        // 2. User Enters Password -> Check DB
        protected void btnVerifyPass_Click(object sender, EventArgs e)
        {
            string userId = Session["UserId"].ToString();
            string enteredPass = txtVerifyPass.Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Password, IsTwoFactorEnabled, TwoFactorSecret, Email FROM Users WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string realPass = reader["Password"].ToString();
                    bool isMfa = reader["IsTwoFactorEnabled"] != DBNull.Value && Convert.ToBoolean(reader["IsTwoFactorEnabled"]);

                    if (enteredPass == realPass)
                    {
                        // PASSWORD CORRECT!

                        if (isMfa)
                        {
                            // --- MFA IS ON: Start Step 2 ---
                            lblVerifyMsg.Text = "";
                            pnlMFA.Visible = true; // Show MFA Box
                            btnVerifyPass.Enabled = false; // Lock Step 1

                            // Determine if using App or Email
                            string secret = reader["TwoFactorSecret"].ToString();
                            if (!string.IsNullOrEmpty(secret))
                            {
                                // APP MODE: Just wait for them to enter code
                                Session["Profile_MFA_Mode"] = "App";
                                Session["Profile_Secret"] = secret;
                            }
                            else
                            {
                                // EMAIL MODE: Send Code
                                Session["Profile_MFA_Mode"] = "Email";
                                string otp = new Random().Next(100000, 999999).ToString();
                                Session["Profile_OTP"] = otp;
                                SendOTPEmail(reader["Email"].ToString(), otp);
                            }
                        }
                        else
                        {
                            // --- MFA IS OFF: Reveal immediately ---
                            RevealPassword(realPass);
                        }
                    }
                    else
                    {
                        lblVerifyMsg.Text = "Incorrect Password.";
                    }
                }
            }
        }

        // 3. User Enters MFA Code -> Reveal Password
        protected void btnVerifyMFA_Click(object sender, EventArgs e)
        {
            string mode = Session["Profile_MFA_Mode"] as string;
            string userCode = txtMFA.Text;
            bool isVerified = false;

            if (mode == "App")
            {
                string secret = Session["Profile_Secret"].ToString();
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                isVerified = tfa.ValidateTwoFactorPIN(secret, userCode);
            }
            else
            {
                string serverOtp = Session["Profile_OTP"] as string;
                isVerified = (userCode == serverOtp);
            }

            if (isVerified)
            {
                // Fetch password again safely to reveal it
                string userId = Session["UserId"].ToString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "SELECT Password FROM Users WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Id", userId);
                    conn.Open();
                    string realPass = cmd.ExecuteScalar().ToString();

                    RevealPassword(realPass);
                }
            }
            else
            {
                lblVerifyMsg.Text = "Invalid Code.";
            }
        }

        private void RevealPassword(string password)
        {
            txtPasswordDisplay.TextMode = TextBoxMode.SingleLine; // Unmask
            txtPasswordDisplay.Text = password;
            pnlVerification.Visible = false; // Hide verify box
            btnInitView.Text = "Hide"; // Change button text
            btnInitView.Enabled = false; // Disable button
        }

        private void LoadOrderHistory()
        {
            string userId = Session["UserId"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Simple select for the grid
                string sql = "SELECT Id, OrderDate, TotalAmount FROM Orders WHERE UserId = @UserId ORDER BY OrderDate DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }
        private bool SendOTPEmail(string emailTo, string otp)
        {
            try
            {
                // Configure Gmail SMTP
                // NOTE: If using Gmail with 2FA, you MUST use an App Password, not your normal password.
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;

                // REPLACE THESE WITH YOUR ACTUAL DETAILS
                smtp.Credentials = new NetworkCredential("YOUR_GMAIL@gmail.com", "YOUR_APP_PASSWORD");

                // Create the Email
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("YOUR_GMAIL@gmail.com", "MusicStore Security");
                msg.To.Add(emailTo);
                msg.Subject = "Your Verification Code";
                msg.Body = "Your Security Code is: " + otp + "\n\nDo not share this code with anyone.";
                msg.IsBodyHtml = false;

                // Send
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                // Optional: Log the error to console or file
                // System.Diagnostics.Debug.WriteLine("Email Error: " + ex.Message);
                return false;
            }
        }

        protected void btnEnableMFA_Click(object sender, EventArgs e)
        {
            Response.Redirect("Setup2FA");
        }
    }
}