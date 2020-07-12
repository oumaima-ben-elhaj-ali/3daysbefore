using System;
using System.Data;
using System.Data.SqlClient;

namespace PFE.Directeur.Utilisateurs
{
    public partial class ProfilUti : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query_notif = "SELECT TOP(4) * FROM " +
                                "(SELECT CONCAT('votre simultaion de ', ST_DESCH, ' est prête') ST_OBSTRA , CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT 'Votre facture est prête' AS ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"].ToString() + "') AS EXTNOTIF" +
                                " ORDER BY DT_CREATE DESC";
                bindNotifsRepeater(query_notif, dt);
                remplirChamps();
            }
            else { }
        }
        //binding notification repeater
        protected void bindNotifsRepeater(string query, DataTable dt)
        {
            SqlConnection con = new SqlConnection(connetionString);
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
        // remplir par les donnees du directeur connecté
        public void remplirChamps()
        {
            string codeuti = Session["UserName"].ToString();
            SqlConnection con = new SqlConnection(connetionString);
            string qry = "SELECT UTI.ID_CODUTI, PRO.ST_DES, UTI.ST_MOTPAS, UTI.ST_MAIL, UTI.ID_CODINT, INTER.ST_NOM, INTER.ST_PRE, INTER.ST_TELMOB FROM UTI LEFT OUTER JOIN PRO ON UTI.ID_CODPRO = PRO.ID_CODPRO LEFT OUTER JOIN INTER ON UTI.ID_CODINT = INTER.ID_CODINT WHERE UTI.ID_CODUTI LIKE '" + codeuti + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idUser.Text = reader["ID_CODUTI"].ToString();
                nomUser.Text = reader["ST_NOM"].ToString();
                prenomUser.Text = reader["ST_PRE"].ToString();
                mailUser.Text = reader["ST_MAIL"].ToString();
                Tel.Text = reader["ST_TELMOB"].ToString();
                mdpUser.Text = reader["ST_MOTPAS"].ToString();
                codeInt.Text = reader["ID_CODINT"].ToString();
                desPro.Text = reader["ST_DES"].ToString();
            }
            con.Close();
        }
        protected void BTNModif_Click(object sender, EventArgs e)
        {
            Response.Redirect("ModifProfil.aspx?ID_CODUTI=" + Session["UserName"].ToString());
        }
    }
}