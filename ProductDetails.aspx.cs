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
            if (Request.QueryString["id"] != null)
            {
                productId = Request.QueryString["id"];
            }
            else
            {
                Response.Redirect("Products");
            }

            if (!IsPostBack)
            {
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
                    lblPrice.Text = Convert.ToDecimal(reader["Price"]).ToString("N2");
                    lblDescription.Text = reader["Description"].ToString();
                    lblCategory.Text = reader["Category"].ToString();
                    imgProduct.ImageUrl = reader["ImageUrl"].ToString();
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
            int qty = Convert.ToInt32(txtQty.Text);

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