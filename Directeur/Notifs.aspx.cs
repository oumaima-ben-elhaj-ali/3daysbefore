using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Directeur
{
    public partial class Notifs : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                string query_notif = "SELECT TOP(4) * FROM " +
                                "(SELECT CONCAT('votre simultaion de ', ST_DESCH, ' est prête') ST_OBSTRA , CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT 'Votre facture est prête' AS ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"].ToString() + "') AS EXTNOTIF" +
                                " ORDER BY DT_CREATE DESC";
                BindRepeater(query_notif, dt);
                ListeNotifs();


            }

        }
        protected void BindRepeater(string query, DataTable dt)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            repNotifs.DataSource = dt;
            repNotifs.DataBind();
            con.Close();
        }


        private void ListeNotifs()
        {
            int counted = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "(select CONCAT(CODUTI_CREATE , ' a générer votre simultaion de ', ST_DESCH) ST_OBSTRA , CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"] + "' UNION " +
                                "select CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"] + "' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"] + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"] + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT CONCAT(ID_CREATOR, ' a généré Votre facture') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"] + "')" +
                                "ORDER BY DT_CREATE DESC";
                    cmd.CommandText = query;
                    cmd.Connection = con;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvNotifs.DataSource = dt;
                        gvNotifs.DataBind();
                        counted = gvNotifs.Rows.Count;
                        if (counted == 0)
                        {
                            gvNotifs.Visible = true;
                        }
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvNotifs.PageIndex = e.NewPageIndex;
            ListeNotifs();
        }

    }
}