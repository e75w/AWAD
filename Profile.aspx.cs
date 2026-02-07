using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace _240795P_EvanLim
{
    public partial class Profile : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("login");
            }

            if (!IsPostBack)
            {
                LoadUserProfile();
                LoadOrderHistory();
            }
        }

        private void LoadUserProfile()
        {
            string userId = Session["UserId"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT Email, Password, IsTwoFactorEnabled FROM Users WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", userId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtEmail.Text = reader["Email"].ToString();

                    txtPassword.Attributes.Add("value", reader["Password"].ToString());

                    bool isMfaEnabled = reader["IsTwoFactorEnabled"] != DBNull.Value && Convert.ToBoolean(reader["IsTwoFactorEnabled"]);

                    if (isMfaEnabled)
                    {
                        lblMFAStatus.Text = "✅ Two-Factor Authentication is Enabled";
                        lblMFAStatus.CssClass = "text-success fw-bold";
                        btnEnableMFA.Visible = false;
                    }
                    else
                    {
                        lblMFAStatus.Text = "⚠️ Your account is not secure.";
                        lblMFAStatus.CssClass = "text-danger mb-2";
                        btnEnableMFA.Visible = true;
                    }
                }
            }
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

        protected void btnEnableMFA_Click(object sender, EventArgs e)
        {
            Response.Redirect("Setup2FA");
        }
    }
}