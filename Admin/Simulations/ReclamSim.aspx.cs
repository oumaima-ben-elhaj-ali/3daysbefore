using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Simulations
{
    public partial class ReclamSim : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                mydiv.Visible = false;
                listeRecalamtions();
                bindNotifsRepeater();
                nbrNotif.Text = nombreNotifs().ToString();

            }
        }
        // selectionner et afficher la liste des simulations correspondants aux donnees selectionnees par l'utilisateur
        public void listeRecalamtions()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                int counted = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ID_CODUTI, ID_SIM , ST_RECLAM, DT_RECLAM FROM EXTRECLAM WHERE EST_TRAITEE IS NULL";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvReclamations.DataSource = dt;
                        gvReclamations.DataBind();
                        counted = gvReclamations.Rows.Count;
                        if (counted == 0)
                        {
                            mydiv.Visible = true;
                        }
                        else mydiv.Visible = false;
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvReclamations.PageIndex = e.NewPageIndex;
            listeRecalamtions();
        }
        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
        protected void bindNotifsRepeater()
        {
            SqlConnection con = new SqlConnection(connectionString);
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
        // selectionner le nbr des notif pou le definir dand l icon dans la navbar droite
        public int nombreNotifs()
        {
            int nbre = 0;
            SqlConnection con = new SqlConnection(connectionString);
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


    }
}