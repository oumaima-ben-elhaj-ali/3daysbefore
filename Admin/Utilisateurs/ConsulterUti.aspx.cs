using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Utilisateurs
{
    public partial class ConsulterUser : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.SearchUsers();
                BindRepeater();
                nbrNotif.Text = NbreNotif().ToString();

            }
        }
        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
        protected void BindRepeater()
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
        public int NbreNotif()
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
        //recuperer ett afficher la lise des utilisateur
        private void SearchUsers()
        {
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT UTI.ID_CODUTI, PRO.ST_DES, UTI.ST_MOTPAS, UTI.ST_MAIL, INTER.ST_NOM, INTER.ST_PRE, INTER.ST_TELMOB FROM UTI LEFT OUTER JOIN INTER ON UTI.ID_CODINT = INTER.ID_CODINT LEFT OUTER JOIN PRO ON UTI.ID_CODPRO = PRO.ID_CODPRO";
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        sql += " WHERE UTI.ID_CODUTI LIKE '%" + txtSearch.Text + "%' or INTER.ST_NOM LIKE '%" + txtSearch.Text + "%' or INTER.ST_PRE LIKE '%" + txtSearch.Text + "%'";
                    }
                    sql += "  ORDER BY ID_CODUTI DESC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvUser.DataSource = dt;
                        gvUser.DataBind();
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvUser.PageIndex = e.NewPageIndex;
            this.SearchUsers();
        }
        protected void Search(object sender, EventArgs e)
        {
            this.SearchUsers();
        }
    }
}