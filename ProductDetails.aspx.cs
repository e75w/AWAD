using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _240795P_EvanLim
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;
        string productId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string productId = Request.QueryString["id"];
                if (string.IsNullOrEmpty(productId))
                {
                    Response.Redirect("Products");
                }

                LoadProductDetails();
            }
        }

        private void LoadProductDetails()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Products WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", productId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblName.Text = reader["Name"].ToString();
                    lblDescription.Text = reader["Description"].ToString();
                    lblPrice.Text = "$" + Convert.ToDecimal(reader["Price"]).ToString("N2");
                    imgProduct.ImageUrl = reader["ImageUrl"].ToString();

                    int stock = Convert.ToInt32(reader["Stock"]);

                    if (stock > 0)
                    {
                        lblStock.Text = stock + " items left";
                        lblStock.ForeColor = System.Drawing.Color.Green;
                        btnAddToCart.Enabled = true;

                        txtQty.Attributes.Add("max", stock.ToString());
                    }
                    else
                    {
                        lblStock.Text = "Out of Stock";
                        lblStock.ForeColor = System.Drawing.Color.Red;
                        btnAddToCart.Enabled = false;
                        btnAddToCart.Text = "Sold Out";
                        btnAddToCart.CssClass = "btn btn-secondary";
                        txtQty.Enabled = false;
                    }
                }
                else
                {
                    Response.Redirect("Products");
                }
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("login");
                return;
            }

            string userId = Session["UserId"].ToString();
            int qty = 0;
            int.TryParse(txtQty.Text, out qty);

            string productId = Request.QueryString["id"];

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check stock
                string checkSql = "SELECT Stock FROM Products WHERE Id = @Id";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Id", productId);

                object result = checkCmd.ExecuteScalar();
                int currentStock = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                if (currentStock <= 0)
                {
                    lblError.Text = "Sorry, this item is out of stock";
                    lblError.Visible = true;
                    return;
                }
                else if (qty > currentStock)
                {
                    lblError.Text = "Sorry, we only have " + currentStock + " left.";
                    lblError.Visible = true;
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string checkSql = "SELECT COUNT(*) FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@UserId", userId);
                checkCmd.Parameters.AddWithValue("@ProductId", productId);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    string updateSql = "UPDATE CartItems SET Quantity = Quantity + @Qty WHERE UserId = @UserId AND ProductId = @ProductId";
                    SqlCommand updateCmd = new SqlCommand(updateSql, conn);
                    updateCmd.Parameters.AddWithValue("@UserId", userId);
                    updateCmd.Parameters.AddWithValue("@ProductId", productId);
                    updateCmd.Parameters.AddWithValue("@Qty", qty);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    string insertSql = "INSERT INTO CartItems (Id, UserId, ProductId, Quantity) VALUES (NEWID(), @UserId, @ProductId, @Qty)";
                    SqlCommand insertCmd = new SqlCommand(insertSql, conn);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@ProductId", productId);
                    insertCmd.Parameters.AddWithValue("@Qty", qty);
                    insertCmd.ExecuteNonQuery();
                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Added to cart!');", true);
        }
    }
}