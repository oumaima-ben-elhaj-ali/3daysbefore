using System;
using System.Data;
using System.Data.SqlClient;

namespace PFE.Admin.Utilisateurs
{
    public partial class ProfilUser : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindNotifRepeater();
                remplirChamps();
                nbrNotif.Text = nombreNotifs().ToString();

            }
            else { }

        }
        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
        protected void bindNotifRepeater()
        {
            SqlConnection con = new SqlConnection(connetionString);
            SqlCommand cmd = new SqlCommand("Select TOP(4) DT_CREATE, ST_OBSTRA from EXTTRACE ORDER BY DT_CREATE DESC", con);
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
        /* Pour l'icon de notification dans la navbar a droite
         * on definit sa valeur par le nombre des ligne de la table EXTTRACE
         */
        public int nombreNotifs()
        {
            int nbre = 0;
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EXTTRACE";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nbre = reader.GetInt32(0);
            }
            return nbre;
        }

        // remplir les champs intitialement par les donnees de l'equipement voulu etre modifié
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
            Response.Redirect("ModifierUti.aspx?ID_CODUTI=" + Session["UserName"].ToString());
        }
    }
}