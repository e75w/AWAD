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
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("/");
            }

            if (!IsPostBack)
            {
                LoadAnalytics();
                LoadInventory();
            }
        }

        private void LoadAnalytics()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Quick Analytics
                string revenueSql = "SELECT SUM(TotalAmount) FROM Orders";
                SqlCommand cmdRev = new SqlCommand(revenueSql, conn);
                object revResult = cmdRev.ExecuteScalar();
                decimal totalRev = (revResult != DBNull.Value && revResult != null) ? Convert.ToDecimal(revResult) : 0;
                lblTotalRevenue.Text = totalRev.ToString("C");

                string countSql = "SELECT COUNT(*) FROM Orders";
                SqlCommand cmdCount = new SqlCommand(countSql, conn);
                int totalOrders = (int)cmdCount.ExecuteScalar();
                lblTotalOrders.Text = totalOrders.ToString();

                string topSql = @"SELECT TOP 1 p.Name FROM OrderDetails od 
                          JOIN Products p ON od.ProductId = p.Id 
                          GROUP BY p.Name ORDER BY SUM(od.Quantity) DESC";
                SqlCommand cmdTop = new SqlCommand(topSql, conn);
                object topResult = cmdTop.ExecuteScalar();
                lblTopProduct.Text = (topResult != null) ? topResult.ToString() : "No Sales Yet";

                List<string> stockNames = new List<string>();
                List<string> stockValues = new List<string>();

                // Charts n Graphs
                string stockSql = "SELECT Name, ISNULL(Stock, 0) as Stock FROM Products ORDER BY Stock ASC";
                SqlCommand cmdStock = new SqlCommand(stockSql, conn);
                SqlDataReader rdrStock = cmdStock.ExecuteReader();

                while (rdrStock.Read())
                {
                    string cleanName = rdrStock["Name"].ToString().Replace(",", "").Replace("'", "");
                    stockNames.Add(cleanName);
                    stockValues.Add(rdrStock["Stock"].ToString());
                }
                rdrStock.Close();

                List<string> revDates = new List<string>();
                List<string> revValues = new List<string>();

                string trendSql = @"SELECT FORMAT(OrderDate, 'dd MMM') as Date, SUM(TotalAmount) as Total 
                            FROM Orders 
                            GROUP BY FORMAT(OrderDate, 'dd MMM'), CAST(OrderDate as Date) 
                            ORDER BY CAST(OrderDate as Date)";
                SqlCommand cmdTrend = new SqlCommand(trendSql, conn);
                SqlDataReader rdrTrend = cmdTrend.ExecuteReader();

                while (rdrTrend.Read())
                {
                    revDates.Add(rdrTrend["Date"].ToString());
                    revValues.Add(rdrTrend["Total"].ToString());
                }
                rdrTrend.Close();

                hfStockLabels.Value = string.Join(",", stockNames);
                hfStockData.Value = string.Join(",", stockValues);

                hfRevLabels.Value = string.Join(",", revDates);
                hfRevData.Value = string.Join(",", revValues);
            }
        }

        private void LoadInventory()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Products ORDER BY Category DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAdminProducts.DataSource = dt;
                gvAdminProducts.DataBind();
            }
        }

        private void ResetForm()
        {
            txtName.Text = "";
            txtPrice.Text = "";
            txtDesc.Text = "";
            txtImage.Text = "";
            hfProductId.Value = "";
            txtStock.Text = "";

            btnAdd.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
            lblMessage.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Insert query
                string sql = @"INSERT INTO Products (Id, Name, Price, Category, Description, ImageUrl, Stock) 
                               VALUES (NEWID(), @Name, @Price, @Category, @Desc, @Img, @Stock)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
                cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Desc", txtDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@Img", txtImage.Text.Trim());
                cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));

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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Products 
                       SET Name=@Name, Price=@Price, Category=@Category, Description=@Desc, ImageUrl=@Img, Stock=@Stock
                       WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
                cmd.Parameters.AddWithValue("@Category", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Desc", txtDesc.Text.Trim());
                cmd.Parameters.AddWithValue("@Img", txtImage.Text.Trim());
                cmd.Parameters.AddWithValue("@Id", hfProductId.Value);
                cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Reset UI
            ResetForm();
            lblMessage.Text = "Product updated successfully!";
            lblMessage.Visible = true;
            lblMessage.CssClass = "alert alert-success d-block";
            LoadInventory();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void LoadProductForEdit(string id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Products WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Populate the form fields
                    txtName.Text = reader["Name"].ToString();
                    txtPrice.Text = reader["Price"].ToString(); // Ensure DB is decimal
                    ddlCategory.SelectedValue = reader["Category"].ToString();
                    txtDesc.Text = reader["Description"].ToString();
                    txtImage.Text = reader["ImageUrl"].ToString();
                    txtStock.Text = reader["Stock"].ToString();

                    // Switch to "Edit Mode"
                    hfProductId.Value = id;
                    btnAdd.Visible = false;
                    btnUpdate.Visible = true;
                    btnCancel.Visible = true;

                    // Optional: Scroll to top or show message
                    lblMessage.Text = "Editing product: " + reader["Name"].ToString();
                    lblMessage.Visible = true;
                    lblMessage.CssClass = "alert alert-warning d-block";
                }
            }
        }

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

            if (e.CommandName == "EditProduct")
            {
                // Get the Row Index and ID
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string productId = gvAdminProducts.DataKeys[rowIndex].Value.ToString();

                LoadProductForEdit(productId);
            }
        }
    }
}