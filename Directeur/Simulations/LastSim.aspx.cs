using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Directeur.Simulations
{
    public partial class LastSim : System.Web.UI.Page
    {
        string connectionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
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
                ListeSimulation();
            }
            else { }
        }
        // lister les simulations de mois courant du directeur connecté
        private void ListeSimulation()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ST_DESCH, ID_SIM , ST_DESIM, CONVERT(varchar, DT_CREATE,111) AS DT_CREATE, NU_VALEUR FROM EXTSIMULATION WHERE (MONTH(DT_CREATE) = @MOIS) AND (YEAR(DT_CREATE) = @YEAR) AND ID_DIR LIKE @ID_DIR ORDER BY ST_DESCH ASC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@YEAR", DateTime.Now.Year.ToString());
                    cmd.Parameters.AddWithValue("@MOIS", DateTime.Now.Month.ToString());
                    cmd.Parameters.AddWithValue("@ID_DIR", Session["UserName"]);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvSimulation.DataSource = dt;
                        gvSimulation.DataBind();
                    }
                }
                con.Close();
            }
        }
        // binding the notification repeater
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
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvSimulation.PageIndex = e.NewPageIndex;
            ListeSimulation();
        }
    }
}