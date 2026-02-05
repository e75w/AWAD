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
        string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            string userId = Session["UserId"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL QUERY EXPLAINED:
                // We JOIN CartItems with Products to get the Name, Image, and Price.
                // We calculate (Price * Quantity) to get the "TotalItemPrice" for that row.
                string sql = @"
                    SELECT 
                        p.Name, 
                        p.ImageUrl, 
                        p.Price, 
                        c.Quantity, 
                        (p.Price * c.Quantity) AS TotalItemPrice 
                    FROM CartItems c
                    INNER JOIN Products p ON c.ProductId = p.Id
                    WHERE c.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind to GridView
                gvCart.DataSource = dt;
                gvCart.DataBind();

                // Calculate Grand Total manually from the DataTable
                decimal grandTotal = 0;
                foreach (DataRow row in dt.Rows)
                {
                    grandTotal += Convert.ToDecimal(row["TotalItemPrice"]);
                }

                lblGrandTotal.Text = "$" + grandTotal.ToString("N2");
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Future Logic: Move items from 'CartItems' to an 'OrderHistory' table
            // For now, we can just show a success message.
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Proceeding to payment...');", true);
        }
    }
}