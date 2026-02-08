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
        public partial class MainPage : System.Web.UI.Page
        {
            string connStr = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;

            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    LoadCarousel();
                }
            }

            private void LoadCarousel()
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "SELECT TOP 3 * FROM Products ORDER BY Price DESC";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptCarousel.DataSource = dt;
                    rptCarousel.DataBind();
                }
            }
        }
}