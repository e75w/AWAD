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
                Response.Redirect("login");
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
                // UPDATED SQL: Added 'c.ProductId' at the start
                string sql = @"
                              SELECT 
                                  c.ProductId, 
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

                gvCart.DataSource = dt;
                gvCart.DataBind();

                decimal grandTotal = 0;
                foreach (DataRow row in dt.Rows)
                {
                    grandTotal += Convert.ToDecimal(row["TotalItemPrice"]);
                }
                lblGrandTotal.Text = "$" + grandTotal.ToString("N2");
            }
        }

        protected void gvCart_Remove(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveItem")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                string productId = gvCart.DataKeys[rowIndex].Value.ToString();
                string userId = Session["UserId"].ToString();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "DELETE FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadCart();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Payment");
        }
    }
}