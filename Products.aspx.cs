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
    public partial class Products : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            string searchTerm = txtSearch.Text.Trim();
            string category = ddlCategory.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Products WHERE 1=1";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND (Name LIKE @Search OR Description LIKE @Search)";
                }

                if (category != "All")
                {
                    sql += " AND Category = @Category";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@Search", "%" + searchTerm + "%");
                }
                if (category != "All")
                {
                    cmd.Parameters.AddWithValue("@Category", category);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // check for if no results found
                if (dt.Rows.Count == 0)
                {
                    rptProducts.Visible = false;
                    pnlNoResults.Visible = true;
                }
                else
                {
                    rptProducts.Visible = true;
                    pnlNoResults.Visible = false;
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlCategory.SelectedValue = "All";
            LoadProducts();
        }

        private void AddToCart(string productId, int qty)
        {
            string userId = Session["UserId"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if item exists
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
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (e.CommandName == "AddToCart")
            {
                string productId = e.CommandArgument.ToString();
                TextBox txtQty = (TextBox)e.Item.FindControl("txtQuantity");

                int quantity = 1;
                int.TryParse(txtQty.Text, out quantity);

                AddToCart(productId, quantity);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Added " + quantity + " item(s) to cart!');", true);
            }
        }
    }
}