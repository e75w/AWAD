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

        protected void rptProducts_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                List<string> cart = Session["Cart"] as List<string>;
                if (cart == null) cart = new List<string>();

                cart.Add(e.CommandArgument.ToString());
                Session["Cart"] = cart;

                Response.Write("<script>alert('Item added to cart!');</script>");
            }
        }
    }
}