using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Admin.Equipements
{
    public partial class DemandeEq : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.demandesNonTraitees();
                BindRepeater();
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
            SqlCommand cmd = new SqlCommand("Select TOP(4) DT_CREATE, ST_OBSTRA FROM EXTTRACE ORDER BY DT_CREATE DESC", con);
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
        /*
         Selectionner les demandes de location non encore traitee a partir de la tables EXTDEMLOC
         et remplir le gridView par ces demandes
         */
        private void demandesNonTraitees()
        {
            int counted = 0;
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ID_DEMANDE, ID_CODUTI, ST_CODEQ, ST_DESCH, CONVERT(varchar, DT_CREATE,111) AS DT_CREATE, CONVERT(varchar, DT_DEBUT,111) AS DT_DEBUT,CONVERT(varchar, DT_FIN,111) AS DT_FIN FROM EXTDEMLOC WHERE ST_STATUS IS NULL ORDER BY DT_CREATE DESC ";

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
            this.demandesNonTraitees();
        }

        protected void demAccepted_Click(object sender, EventArgs e)
        {
            Response.Redirect("DemandesAcceptees.aspx");
        }

        protected void demRefused_Click(object sender, EventArgs e)
        {
            Response.Redirect("DemandesRefusees.aspx");
        }
    }
}