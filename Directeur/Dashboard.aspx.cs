using System;
using System.Data;
using System.Data.SqlClient;

namespace PFE.Directeur
{
    public partial class Dashboard : System.Web.UI.Page
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
                                "(SELECT CONCAT('votre simultaion de ', ST_DESCH, ' est prête') ST_OBSTRA , CONVERT(varchar,DT_CREATE,101) as DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(varchar,DT_CREATE,101) as DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT  CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(varchar,DT_CREATE,101) as DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT  CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(varchar,DT_CREATE,101) as DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT 'Votre facture est prête' AS ST_OBSTRA, CONVERT(varchar,DT_CREATE,101) as DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"].ToString() + "') AS EXTNOTIF" +
                                " ORDER BY DT_CREATE DESC";
                BindRepeater(query_notif, dt);
                NbrDep();
                NbDemandes();
                NbReclamations();
                NbSimulation();
            }
        }
        public void NbDemandes()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EXTDEMLOC WHERE ST_STATUS IS NULL AND ID_CODUTI LIKE '" + Session["UserName"].ToString() + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                demandes.Text = reader.GetInt32(0).ToString();
            }
            else demandes.Text = "0";
            con.Close();
        }
        public void NbReclamations()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT COUNT(*) AS VALUE FROM EXTRECLAM WHERE EST_TRAITEE IS NULL AND ID_CODUTI LIKE '" + Session["UserName"].ToString() + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reclamations.Text = reader.GetInt32(0).ToString();
            }
            else reclamations.Text = "0";
            con.Close();
        }
        public void NbSimulation()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT COUNT(*) AS VALUE FROM EXTSIMULATION WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate()) AND ID_DIR LIKE '" + Session["UserName"].ToString() + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                nbSim.Text = reader.GetInt32(0).ToString();
            }
            else nbSim.Text = "0";
            con.Close();
        }
        public void NbrDep()
        {
            int val = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EXTDEPEQU WHERE Month(DT_ENTREE) = Month(getdate()) AND Year(DT_ENTREE) = Year(getdate()) AND ST_DESCH IN (SELECT ST_DESCH FROM EXTCHANTIER WHERE ID_CODUTIDIR LIKE '" + Session["UserName"].ToString() + "')";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetInt32(0);
            }
            countEq.Text = val.ToString();
            con.Close();
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

    }
}