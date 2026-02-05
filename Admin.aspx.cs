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
    public partial class Admin : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Check: Only allow Admins (Optional but recommended)
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                // Response.Redirect("login.aspx"); // Uncomment to lock this page
            }

            if (!IsPostBack)
            {
                LoadAnalytics();
                LoadProducts();
            }
        }

        // --- ANALYTICS LOGIC ---
        private void LoadAnalytics()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                // 1. Get Total Revenue
                SqlCommand cmdRevenue = new SqlCommand("SELECT SUM(TotalAmount) FROM Orders", conn);
                object resultRevenue = cmdRevenue.ExecuteScalar();
                lblRevenue.Text = resultRevenue != DBNull.Value ? string.Format("{0:C}", resultRevenue) : "$0.00";

                // 2. Get Total Orders Count
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Orders", conn);
                lblOrdersCount.Text = cmdCount.ExecuteScalar().ToString();
            }
        }

        // --- CRUD READ LOGIC ---
        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        // --- CRUD CREATE LOGIC ---
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "INSERT INTO Products (Id, Name, Price, Category, ImageUrl) VALUES (NEWID(), @Name, @Price, @Category, @Img)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", txtNewName.Text);
                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtNewPrice.Text));
                cmd.Parameters.AddWithValue("@Category", ddlNewCat.SelectedValue);
                cmd.Parameters.AddWithValue("@Img", txtNewImg.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            LoadProducts(); // Refresh Grid
        }

        // --- CRUD UPDATE LOGIC ---
        protected void gvProducts_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            // Get the ID of the row being edited
            string id = gvProducts.DataKeys[e.RowIndex].Value.ToString();

            // Get new values from the textboxes in the GridView
            string name = ((System.Web.UI.WebControls.TextBox)gvProducts.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string price = ((System.Web.UI.WebControls.TextBox)gvProducts.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string category = ((System.Web.UI.WebControls.TextBox)gvProducts.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Products SET Name=@Name, Price=@Price, Category=@Category WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(price));
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvProducts.EditIndex = -1; // Exit edit mode
            LoadProducts();
        }

        // --- CRUD DELETE LOGIC ---
        protected void gvProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            string id = gvProducts.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Products WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            LoadProducts();
        }

        // Helper: Cancel Edit Mode
        protected void gvProducts_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            LoadProducts();
        }

        protected void gvProducts_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            LoadProducts();
        }
    }
}