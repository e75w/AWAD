using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace _240795P_EvanLim
{
    public partial class Admin : System.Web.UI.Page
    {
        // Using MainDBConnection as requested
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. AUTHORISATION (Fixed Feature: Role Security)
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                // If not admin, send them home
                Response.Redirect("/");
            }

            if (!IsPostBack)
            {
                LoadAnalytics();
                LoadInventory();
            }
        }

        // --- COMPLEX FEATURE: ANALYTICS ---
        private void LoadAnalytics()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Metric 1: Total Revenue
                string revenueSql = "SELECT SUM(TotalAmount) FROM Orders";
                SqlCommand cmdRev = new SqlCommand(revenueSql, conn);
                object revResult = cmdRev.ExecuteScalar();
                decimal totalRev = (revResult != DBNull.Value && revResult != null) ? Convert.ToDecimal(revResult) : 0;
                lblTotalRevenue.Text = totalRev.ToString("C"); // Formats as Currency ($)

                // Metric 2: Total Orders
                string countSql = "SELECT COUNT(*) FROM Orders";
                SqlCommand cmdCount = new SqlCommand(countSql, conn);
                int totalOrders = (int)cmdCount.ExecuteScalar();
                lblTotalOrders.Text = totalOrders.ToString();

                // Metric 3: Top Selling Product
                // This complex query joins OrderDetails with Products to find the most popular item
                string topSql = @"
                    SELECT TOP 1 p.Name 
                    FROM OrderDetails od
                    JOIN Products p ON od.ProductId = p.Id
                    GROUP BY p.Name
                    ORDER BY SUM(od.Quantity) DESC";

                SqlCommand cmdTop = new SqlCommand(topSql, conn);
                object topResult = cmdTop.ExecuteScalar();
                lblTopProduct.Text = (topResult != null) ? topResult.ToString() : "No Sales Yet";
            }
        }

        // --- FIXED FEATURE: CRUD (READ) ---
        private void LoadInventory()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Products ORDER BY DateAdded DESC"; // Show newest first

                // Note: If you don't have a DateAdded column yet, just use: ORDER BY Name
                // string sql = "SELECT * FROM Products ORDER BY Name";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAdminProducts.DataSource = dt;
                gvAdminProducts.DataBind();
            }
        }

        // --- FIXED FEATURE: CRUD (CREATE) ---
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Insert query
                string sql = @"INSERT INTO Products (Id, Name, Price, Category, Description, ImageUrl) 
                               VALUES (NEWID(), @Name, @Price, @Category, @Desc, @Img)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
                cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Desc", txtDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@Img", txtImage.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Reset UI
            txtName.Text = "";
            txtPrice.Text = "";
            txtDesc.Text = "";
            txtImage.Text = "";

            lblMessage.Text = "Product added successfully!";
            lblMessage.Visible = true;
            lblMessage.CssClass = "alert alert-success d-block";

            LoadInventory(); // Refresh the grid immediately
        }

        // --- FIXED FEATURE: CRUD (DELETE) ---
        protected void gvAdminProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProduct")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string productId = gvAdminProducts.DataKeys[rowIndex].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Note: In a real app, you can't delete a product if it's in a past Order (Foreign Key error).
                    // We wrap this in a try/catch to warn the Admin.
                    try
                    {
                        string sql = "DELETE FROM Products WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@Id", productId);
                        cmd.ExecuteNonQuery();

                        LoadInventory(); // Success: Refresh grid
                    }
                    catch (SqlException)
                    {
                        // This handles the Foreign Key constraint error
                        ClientScript.RegisterStartupScript(this.GetType(), "alert",
                            "alert('Cannot delete this product because it has been bought by customers. It is part of the Sales History.');",
                            true);
                    }
                }
            }
        }
    }
}