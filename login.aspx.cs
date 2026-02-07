using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;

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
                string sql = "SELECT Id, Role, Email, IsTwoFactorEnabled, TwoFactorSecret FROM Users WHERE Email = @Email AND Password = @Password";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPass.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Session["TempUserId"] = reader["Id"];
                    Session["TempRole"] = reader["Role"];

                    bool isAppEnabled = reader["IsTwoFactorEnabled"] != DBNull.Value && Convert.ToBoolean(reader["IsTwoFactorEnabled"]);
                    Session["NeedsAppVerification"] = isAppEnabled;

                    string otp = new Random().Next(100000, 999999).ToString();
                    Session["EmailOTP"] = otp;
                    SendOTPEmail(reader["Email"].ToString(), otp);

                    Response.Redirect("MFA");
                }

                else
                {
                    lblError.Text = "Invalid email or password.";
                }
            }
        }

        private bool SendOTPEmail(string emailTo, string otp)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = new NetworkCredential("awadassnevanlim@gmail.com", "tqfc olgs ozra apls");

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("awadassnevanlim@gmail.com", "MusicStore Security");
                msg.To.Add(emailTo);
                msg.Subject = "Your Login Verification Code";
                msg.Body = "Your OTP Code is: " + otp + "\n\nDo not share this code with anyone.";
                msg.IsBodyHtml = false;

                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Email Error: " + ex.ToString());

                throw new Exception("Email Failed: " + ex.Message);

                return false;
            }
        }
    }
}