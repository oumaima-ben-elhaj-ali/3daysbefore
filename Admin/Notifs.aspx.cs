using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
namespace PFE
{
    public partial class Notifs : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                tableNotif();
                bindNotifRepeater();

            }
        }
        protected void bindNotifRepeater()
        {
            SqlConnection con = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("Select TOP(4) CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE, ST_OBSTRA from EXTTRACE ORDER BY DT_CREATE DESC", con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            repNotifs.DataSource = dt;
            repNotifs.DataBind();
            con.Close();
        }

        private void tableNotif()
        {

            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT CONVERT(VARCHAR, DT_CREATE) AS DATETRACE, ST_OBSTRA FROM EXTTRACE ORDER BY DT_CREATE DESC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvNotifs.DataSource = dt;
                        gvNotifs.DataBind();
                    }
                }
                con.Close();
            }
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvNotifs.PageIndex = e.NewPageIndex;
            tableNotif();
        }
    }
}