using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _240795P_EvanLim
{
    public partial class Payment : System.Web.UI.Page
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
                DisplayTotalAmount();
            }
        }

        private void DisplayTotalAmount()
        {
            string userId = Session["UserId"].ToString();
            decimal total = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
                    SELECT SUM(p.Price * c.Quantity) 
                    FROM CartItems c
                    INNER JOIN Products p ON c.ProductId = p.Id
                    WHERE c.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    total = Convert.ToDecimal(result);
                }
            }

            // If total is 0, they shouldn't be here
            if (total == 0)
            {
                Response.Redirect("Products");
            }

            lblTotalAmount.Text = "$" + total.ToString("N2");
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string userId = Session["UserId"].ToString();
            Guid newOrderId = Guid.NewGuid(); // Generate new Order ID

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string cartSql = @"
                                      SELECT c.ProductId, c.Quantity, p.Price 
                                      FROM CartItems c
                                      INNER JOIN Products p ON c.ProductId = p.Id
                                      WHERE c.UserId = @UserId";

                    SqlCommand cartCmd = new SqlCommand(cartSql, conn, transaction);
                    cartCmd.Parameters.AddWithValue("@UserId", userId);

                    DataTable cartTable = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cartCmd))
                    {
                        da.Fill(cartTable);
                    }

                    if (cartTable.Rows.Count == 0)
                    {
                        transaction.Rollback();
                        return;
                    }

                    decimal totalAmount = 0;
                    foreach (DataRow row in cartTable.Rows)
                    {
                        decimal price = Convert.ToDecimal(row["Price"]);
                        int qty = Convert.ToInt32(row["Quantity"]);
                        totalAmount += (price * qty);
                    }

                    string orderSql = "INSERT INTO Orders (Id, UserId, TotalAmount, OrderDate) VALUES (@Id, @UserId, @Total, GETDATE())";
                    SqlCommand orderCmd = new SqlCommand(orderSql, conn, transaction);
                    orderCmd.Parameters.AddWithValue("@Id", newOrderId);
                    orderCmd.Parameters.AddWithValue("@UserId", userId);
                    orderCmd.Parameters.AddWithValue("@Total", totalAmount);
                    orderCmd.ExecuteNonQuery();

                    string detailSql = @"
                                        INSERT INTO OrderDetails (Id, OrderId, ProductId, PriceAtPurchase, Quantity) 
                                        VALUES (NEWID(), @OrderId, @ProductId, @Price, @Qty)";

                    foreach (DataRow row in cartTable.Rows)
                    {
                        SqlCommand detailCmd = new SqlCommand(detailSql, conn, transaction);
                        detailCmd.Parameters.AddWithValue("@OrderId", newOrderId);
                        detailCmd.Parameters.AddWithValue("@ProductId", row["ProductId"]); // Foreign Key
                        detailCmd.Parameters.AddWithValue("@Price", row["Price"]);        // Price at the moment of purchase
                        detailCmd.Parameters.AddWithValue("@Qty", row["Quantity"]);
                        detailCmd.ExecuteNonQuery();
                    }

                    string deleteSql = "DELETE FROM CartItems WHERE UserId = @UserId";
                    SqlCommand deleteCmd = new SqlCommand(deleteSql, conn, transaction);
                    deleteCmd.Parameters.AddWithValue("@UserId", userId);
                    deleteCmd.ExecuteNonQuery();

                    transaction.Commit();

                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Payment Successful! Order #" + newOrderId.ToString().Substring(0, 8) + " created.'); window.location='/';",
                        true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblTotalAmount.Text = "Error: " + ex.Message;
                    lblTotalAmount.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}