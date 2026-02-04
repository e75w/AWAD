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
    public partial class products : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts(string search = "", string category = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Products WHERE Name LIKE @Search";
                if (!string.IsNullOrEmpty(category))
                {
                    sql += " AND Category = @Category";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                if (!string.IsNullOrEmpty(category))
                {
                    cmd.Parameters.AddWithValue("@Category", category);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptProducts.DataSource = dt;
                rptProducts.DataBind();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim(), ddlCategory.SelectedValue);
        }

        protected void rptProducts_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                // Simple Session-based Cart logic
                List<string> cart = Session["Cart"] as List<string>;
                if (cart == null) cart = new List<string>();

                cart.Add(e.CommandArgument.ToString());
                Session["Cart"] = cart;

                // Optional: Show alert
                Response.Write("<script>alert('Item added to cart!');</script>");
            }
        }
    }
}