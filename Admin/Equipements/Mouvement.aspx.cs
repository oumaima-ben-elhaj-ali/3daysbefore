using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace PFE.Equipements
{
    public partial class Mouvement : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Write("Code Equipement =" + Request.QueryString["ST_CODCOU"]);
                SearchMouvements();
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
        // Recuperer et afficher les mouvement de l'equipement
        private void SearchMouvements()
        {

            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                int counted = 0;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string code = Request.QueryString["ST_CODCOU"];
                    string sql = "SELECT ID_MVT, ST_CODEEQ, ST_DESCH, DT_MVT, NU_CIN, ST_TYPEMVT FROM EXTMVTEQUCH WHERE ST_CODEEQ LIKE '" + code + "' ORDER BY DT_MVT DESC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvMouvement.DataSource = dt;
                        gvMouvement.DataBind();
                        counted = gvMouvement.Rows.Count;
                    }
                }
                div_ctrl.Text = counted.ToString();
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvMouvement.PageIndex = e.NewPageIndex;
            SearchMouvements();
        }
        protected void GoToDeplacer(object sender, EventArgs e)
        {
            Response.Redirect("DeplacerEq.aspx");
        }
    }
}