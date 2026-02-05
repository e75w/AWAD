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

            // Check if cart is empty to avoid SQL errors
            if (cart != null && cart.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    List<string> paramNames = new List<string>();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    for (int i = 0; i < cart.Count; i++)
                    {
                        string paramName = "@id" + i;
                        paramNames.Add(paramName);
                        cmd.Parameters.AddWithValue(paramName, cart[i]);
                    }

                    string inClause = string.Join(",", paramNames);
                    string sql = $"SELECT * FROM Products WHERE Id IN ({inClause})";

                    cmd.CommandText = sql;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
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
            else
            {
                // Handle empty cart (optional: hide grid or show message)
                lblTotal.Text = "$0.00";
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            List<string> cart = Session["Cart"] as List<string>;
            if (cart == null || cart.Count == 0) return;

            decimal totalAmount = 0;
            if (ViewState["TotalAmount"] != null)
            {
                totalAmount = Convert.ToDecimal(ViewState["TotalAmount"]);
            }

            Guid newOrderId = Guid.NewGuid();
            string userId = Session["UserId"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                // Start a Transaction (Ensures Orders and Items are saved together)
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // INSERT INTO Orders TABLE
                    string orderSql = "INSERT INTO Orders (Id, UserId, TotalAmount, OrderDate) VALUES (@Id, @UserId, @Total, @Date)";
                    SqlCommand cmdOrder = new SqlCommand(orderSql, conn, transaction);
                    cmdOrder.Parameters.AddWithValue("@Id", newOrderId);
                    cmdOrder.Parameters.AddWithValue("@UserId", userId);
                    cmdOrder.Parameters.AddWithValue("@Total", totalAmount);
                    cmdOrder.Parameters.AddWithValue("@Date", DateTime.Now);
                    cmdOrder.ExecuteNonQuery();

                    // INSERT INTO OrderDetails TABLE (Loop through cart) --> 1 item per row
                    foreach (string productId in cart)
                    {
                        string itemSql = @"INSERT INTO OrderDetails (Id, OrderId, ProductId, PriceAtPurchase, Quantity) 
                                           VALUES (NEWID(), @OrderId, @ProductId, 
                                           (SELECT Price FROM Products WHERE Id = @ProductId), 1)";

                        SqlCommand cmdItem = new SqlCommand(itemSql, conn, transaction);
                        cmdItem.Parameters.AddWithValue("@OrderId", newOrderId);
                        cmdItem.Parameters.AddWithValue("@ProductId", productId);

                        cmdItem.ExecuteNonQuery();
                    }

                    // COMMIT TRANSACTION (Save everything)
                    transaction.Commit();

                    // Clear cart and redirect
                    Session["Cart"] = null;
                    Response.Write("<script>alert('Order placed successfully!'); window.location='mainPg.aspx';</script>");
                }
                catch (Exception ex)
                {
                    // If error, undo everything
                    transaction.Rollback();
                    lblMessage.Text = "Transaction Failed: " + ex.Message;
                }
            }
        }
    }
}