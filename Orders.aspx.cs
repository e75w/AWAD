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
    public partial class Orders : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            List<string> cart = Session["Cart"] as List<string>;
            if (cart != null && cart.Count > 0)
            {
                // Convert list of IDs '1,2,3' into a SQL IN clause
                // Note: In production, use parameters/Stored Procedures to prevent Injection here
                string ids = string.Join(",", cart);

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // This query is simplified for the example. Be careful with SQL Injection on 'ids'.
                    // Better approach: parameterized loop or TVP.
                    string sql = $"SELECT * FROM Products WHERE Id IN ({ids})";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvCart.DataSource = dt;
                    gvCart.DataBind();

                    decimal total = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        total += Convert.ToDecimal(row["Price"]);
                    }
                    lblTotal.Text = total.ToString("C");
                    ViewState["TotalAmount"] = total;
                }
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Requirement: Complex Feature (Checkout)
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = "INSERT INTO Orders (Id, UserId, TotalAmount, OrderDate) VALUES (@Id, @UserId, @Total, @Date)";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@UserId", Session["UserId"]);
                    cmd.Parameters.AddWithValue("@Total", ViewState["TotalAmount"]);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

                // Clear cart and show success
                Session["Cart"] = null;
                Response.Write("<script>alert('Payment Successful!'); window.location='mainPg.aspx';</script>");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error processing order: " + ex.Message;
            }
        }
    }
}