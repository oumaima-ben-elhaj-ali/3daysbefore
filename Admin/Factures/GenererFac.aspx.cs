using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.UI.WebControls;

namespace PFE.Factures
{
    public partial class GenererFac : System.Web.UI.Page
    {
        string connectionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int n = nombresNotifs();
                nbrNotif.Text = n.ToString();
                activeGenerateButton();
                string query_notif = "Select TOP(4) DT_CREATE, ST_OBSTRA from EXTTRACE ORDER BY DT_CREATE DESC";
                bindRepeater(query_notif, dt);
            }
        }

        /* activeGenerateButton():
          => si la date de creation de la derniere facture appartient au moins courant alors le bouton 
          Generer est DISABLED 
          sinon si la date de creation de la derniere facture m'appartient au moins courant et
          c'est le 5 ou plus du mois courant alors l'administrateur peut generer le facture 
          est apres la generation le bouton revient DISABLED 
             */
        public void activeGenerateButton()
        {
            string qry = "SELECT TOP(1) DT_CREATE FROM EXTFACTURES ORDER BY DT_CREATE DESC ";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(qry, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                DateTime dateLastFact = reader.GetDateTime(0);
                if (dateLastFact.Month == DateTime.Now.Month)
                {
                    BTNgenerer.Enabled = false;
                    mydiv.Visible = true;
                }
                else if ((dateLastFact.Month != DateTime.Now.Month) && DateTime.Now.Day > 5)
                {
                    BTNgenerer.Enabled = true;
                    info.Text = "Vouz pouver générer les factures maintenant!";
                }
            }
            con.Close();
        }

        protected void BTNgenerer_Click(object sender, EventArgs e)
        {
            validerSimulations();
            generateFact();
            updateIdFactures();
            listerFactures();
            insertIntoTrace();
            BTNgenerer.Enabled = false;
        }
        //valider les simulations
        public void validerSimulations()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT ID_SIM FROM EXTSIMULATION WHERE (MONTH(DT_CREATE) = MONTH(GETDATE())) AND (YEAR(DT_CREATE) = YEAR(GETDATE()) AND SIM_STATE LIKE 'UNVALIDATED')";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                setStatusOnValidated(reader.GetInt32(0));
            }
            con.Close();
        }
        public void setStatusOnValidated(int id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "UPDATE EXTSIMULATION SET SIM_STATE='VALIDATED' WHERE ID_SIM=" + id;
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        /* generateFact():
         * 1-selectionner les directeurs qui ont des simulation pendant le mois courant
         * 2-Recuperer le nom complet du chaque directeur
         * 3-calculer la valeur de la facture de chaque directeur 
         * 4-inserer une nouvelle ligne dans la table EXTFACTURES pour chaque directeur
         */
        public void generateFact()
        {
            string NOMDIR = "";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT DISTINCT ID_DIR FROM EXTSIMULATION WHERE (MONTH(DT_CREATE) = MONTH(GETDATE())) AND (YEAR(DT_CREATE) = YEAR(GETDATE()))";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                NOMDIR = getDirecteur(reader["ID_DIR"].ToString());
                Decimal value = calculValeurFacture(reader["ID_DIR"].ToString());
                insertIntoFact(reader["ID_DIR"].ToString(), NOMDIR, value);

            }
            con.Close();
        }
        // Recuperer le nom et prenom correspondant a L'ID_DIR passé en parametre
        public string getDirecteur(string ch)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT ST_NOM, ST_PRE FROM INTER WHERE ID_CODUTI LIKE '" + ch + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string nom = reader["ST_NOM"].ToString() + " " + reader["ST_PRE"].ToString();
                return nom;
            }
            con.Close();
            return "";
        }
        // calculer la valeur de la facture => la sommes des valeur des simulation ayant l ID_DIR passé en parametre
        public Decimal calculValeurFacture(string iddir)
        {
            Decimal val = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT SUM(NU_VALEUR) FROM EXTSIMULATION WHERE ID_DIR LIKE '" + iddir + "' AND Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate())";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetDecimal(0);
            }
            con.Close();
            return val;
        }
        // insErer le Id_dir, son nom complet, la valeur de la facture et la date de creation dans la table EXTFACTURE
        public void insertIntoFact(string iddir, string nomdir, Decimal value)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "INSERT INTO EXTFACTURES (ID_DIR, ST_NOMDIR, DT_CREATE, NU_VALEUR, ID_CREATOR) VALUES (@ID_DIR, @ST_NOMDIR, @DT_CREATE, @NU_VALEUR, @ID_CREATOR)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@ID_DIR", iddir);
            cmd.Parameters.AddWithValue("@ST_NOMDIR", nomdir);
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@NU_VALEUR", value);
            cmd.Parameters.AddWithValue("@ID_CREATOR", Session["UserName"].ToString());
            cmd.ExecuteNonQuery();
            con.Close();
        }
        /* Update EXTSIMULATION (part2)
                1- Selectionner les FACTURES du mois courant 
                2- Pour chaque FACTURE:
                     a) Selectionner les SIMULATION Correspendant selectSimCorrespendantes()
                     b) pour chaque SIMULATION set ID_FACT par l id de cette FACTURE => UpdateID_SIM()
         */
        public void updateIdFactures()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT * from EXTFACTURES WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate()) ";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                selectSimCorrespendantes(reader["ID_DIR"].ToString(), Convert.ToInt32(reader["ID_FACT"].ToString()));
            }
            con.Close();
        }
        public void selectSimCorrespendantes(string iddir, int idFACT)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT * from EXTSIMULATION WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate()) AND ID_DIR LIKE '" + iddir + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                updateIdFact(Convert.ToInt32(reader["ID_SIM"].ToString()), idFACT);
            }
            con.Close();

        }
        public void updateIdFact(int idSIM, int idFACT)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "UPDATE EXTSIMULATION SET ID_FACT = " + idFACT + " WHERE ID_SIM = " + idSIM;
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Afficher la liste des simulations generees
        private void listerFactures()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    string sql = "SELECT * FROM EXTFACTURES WHERE (MONTH(DT_CREATE) = MONTH(GETDATE())) AND (YEAR(DT_CREATE) = YEAR(GETDATE()))";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvFactures.DataSource = dt;
                        gvFactures.DataBind();
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvFactures.PageIndex = e.NewPageIndex;
            listerFactures();
        }
        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
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
        // selectionner le nbr des notif pou le definir dand l icon dans la navbar droite
        public int nombresNotifs()
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

        // inserer la action de generation
        public void insertIntoTrace()
        {
            string Trace = Session["UserName"].ToString() + " a généré les factures";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = " INSERT INTO EXTTRACE (NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) VALUES (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", Trace);
            cmd.Parameters.AddWithValue("@ADR_MAC", getMacAddress());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private string getMacAddress()
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
    }
}