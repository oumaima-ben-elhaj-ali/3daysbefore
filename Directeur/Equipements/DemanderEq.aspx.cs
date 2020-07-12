using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.UI.WebControls;

namespace PFE.Directeur.Equipements
{
    public partial class DemanderEq : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                alert.Visible = false;
                mydiv.Visible = false;
                cnx.Text = connectionString;
                queryCH.Text = "SELECT ST_DESCH FROM EXTCHANTIER WHERE ST_DESCH LIKE ";
                query.Text = "SELECT ST_CODCOU FROM EQU WHERE ST_DESEQU LIKE ";
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                string query_notif = "SELECT TOP(4) * FROM " +
                                "(SELECT CONCAT('votre simultaion de ', ST_DESCH, ' est prête') ST_OBSTRA , CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT 'Votre facture est prête' AS ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"].ToString() + "') AS EXTNOTIF" +
                                " ORDER BY DT_CREATE DESC";
                bindRepeater(query_notif, dt);
                demandesNonTraitees();
                CodcouEQ.Text = Request.QueryString["ST_CODCOU"];
                dtDebut.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dtFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
                if (Request.QueryString["ID_DEMANDE"] != "")
                {
                    annulerDemande(Convert.ToInt32(Request.QueryString["ID_DEMANDE"]));
                }
            }
        }
        // binding the notification repeater
        protected void bindRepeater(string query, DataTable dt)
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
        /* envoyer la demande de location
         * inserer une nouvelle ligne dans la table EXTDEMLOC par les donnees saisie par l'utilisateur
         * */
        protected void BTNenvoyer_Click(object sender, EventArgs e)
        {
            if (textboxesAreEmpty())
            {
                alert.Visible = true;
            }
            else
            {
                DateTime DE = Convert.ToDateTime(dtDebut.Text);
                DateTime DS = Convert.ToDateTime(dtFin.Text);
                if (DateTime.Compare(DE, DS) >= 0)
                {
                    errormsg.Text = "La date début doit être antérieure à la date fin";
                    alert.Visible = true;
                }
                else
                {
                    alert.Visible = false;
                    string a = Session["UserName"].ToString();
                    string b = chantier.Text;
                    string c = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
                    string d = dtDebut.Text;
                    string ee = dtFin.Text;
                    string qry = "INSERT INTO EXTDEMLOC(ID_CODUTI, ST_CODEQ, ST_DESCH, DT_CREATE, DT_DEBUT, DT_FIN) VALUES (@ID_CODUTI, @ST_CODEQ,@ST_DESCH, @DT_CREATE, @DT_DEBUT, @DT_FIN)";
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@ST_CODEQ", CodcouEQ.Text);
                    cmd.Parameters.AddWithValue("@ID_CODUTI", Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@ST_DESCH", chantier.Text);
                    cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@DT_DEBUT", dtDebut.Text);
                    cmd.Parameters.AddWithValue("@DT_FIN", dtFin.Text);
                    cmd.ExecuteNonQuery();
                    demandesNonTraitees();
                    con.Close();
                    insertIntoTrace();
                    MsgResult("Votre demande est envoyée", "information");
                }
            }

        }
        //tester que les champs sont remplis
        public Boolean textboxesAreEmpty()
        {
            if (CodcouEQ.Text == String.Empty || chantier.Text == String.Empty)
            {
                return true;
            }
            return false;
        }
        // Remplir le gridView par les demande de l'utilisateur non encore traitee
        private void demandesNonTraitees()
        {
            int counted = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ID_DEMANDE, ST_CODEQ, ST_DESCH, convert(varchar,DT_CREATE) AS date  FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS IS NULL ORDER BY DT_CREATE DESC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvDemandes.DataSource = dt;
                        gvDemandes.DataBind();
                        counted = gvDemandes.Rows.Count;
                        if (counted == 0)
                        {
                            mydiv.Visible = true;
                        }
                    }
                }
                con.Close();
            }
        }
        // afficher une alerte
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
        // Supprimer la demande de la table EXTDEMLOC
        public void annulerDemande(int id)
        {
            string qry = "DELETE FROM EXTDEMLOC WHERE ID_DEMANDE =" + id;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            con.Close();
            demandesNonTraitees();
        }
        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            CodcouEQ.Text = String.Empty;
            dtDebut.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        //inserer l'action de demande de location dans EXTTRACE
        public void insertIntoTrace()
        {
            string trace = Session["UserName"].ToString() + " demande la location de l'équipement " + CodcouEQ.Text;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = " INSERT INTO EXTTRACE (NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) VALUES (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", trace);
            cmd.Parameters.AddWithValue("@ADR_MAC", GetMacAddress());
            cmd.ExecuteNonQuery();
            con.Close();
        }
        // Recuperer @ MAC de la machine
        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvDemandes.PageIndex = e.NewPageIndex;
            this.demandesNonTraitees();
        }

    }
}