using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace PFE.Equipements
{
    public partial class ConsulterEq : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                mydiv.Visible = false;
                BindRepeater();
                this.SearchEquipements();
                int n = NbreNotif();
                nbrNotif.Text = n.ToString();

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
        /* Pour l icon de notification dans la navbar a droite
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
        // replir le gidView par les donnees des equipements a partir de la BD
        private void SearchEquipements()
        {

            using (SqlConnection con = new SqlConnection(connetionString))
            {
                int counted = 0;
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ST_CODCOU, ST_DESEQU, ID_CODIMP, CAST(NU_PRIACH AS DECIMAL(16,3)) AS NU_PRIACH ,CAST(NU_PRIACT AS DECIMAL(16,3)) as NU_PRIACT FROM EQU WHERE NU_PRIACT >0 AND NU_PRIACH>0";
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        sql += " AND ST_DESEQU LIKE '%" + txtSearch.Text + "%' or ST_CODCOU LIKE '%" + txtSearch.Text + "%' OR ID_CODIMP LIKE '%" + txtSearch.Text + "%'";
                    }
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvEquipement.DataSource = dt;
                        gvEquipement.DataBind();
                        counted = gvEquipement.Rows.Count;
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
            gvEquipement.PageIndex = e.NewPageIndex;
            this.SearchEquipements();
        }

        /*en entrant une chaine de caractere dans la zone de recherche
         le remplissage de gridview sera controler par cette chaine en cherchant tout les equipement
         ou sa deseignation contient la chaine entree
        */
        protected void Search(object sender, EventArgs e)
        {
            this.SearchEquipements();
        }
    }
}