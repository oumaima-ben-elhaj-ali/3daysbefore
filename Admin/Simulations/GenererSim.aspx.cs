using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.UI.WebControls;

namespace PFE.Simulations
{
    public partial class GenererSim : System.Web.UI.Page
    {
        string connectionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                activeGenerateButton();
                bindNotifsRepeater();
                nombreNotif();
            }
            else { }


        }
        /* Pour l'icon de notification dans la navbar a droite
         * on definit sa valeur par le nombre des ligne de la table EXTTRACE
         */
        public int nombreNotif()
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
        /* activeGenerateButton():
          => si la date de creation de la derniere simulation appartient au moins courant alors le bouton 
          Generer est DISABLED 
          sinon si la date de creation de la derniere simulation m'appartient au moins courant alors 
          l'administrateur peut generer le facture 
          est apres la generation le bouton revient DISABLED 
             */
        public void activeGenerateButton()
        {
            string qry = "SELECT TOP(1) DT_CREATE FROM EXTSIMULATION ORDER BY DT_CREATE DESC ";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(qry, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                DateTime dateLastSim = reader.GetDateTime(0);
                if (dateLastSim.Month == DateTime.Now.Month)
                {
                    BTNgenerer.Enabled = false;
                    msgsim.Text = "la prochaine génération des simulations commence le 1er du mois prochain..";
                    mydiv.Visible = true;
                }
                else
                {
                    BTNgenerer.Enabled = true;
                    msgsim.Text = "Vous pouvez générer les simulations maintenant";
                    mydiv.Visible = true;
                }
            }
            con.Close();
        }

        /*
         Etapes de generation:
         ---------------------------------------------------------
            I- Update EXTDEPEQU (Part1) --> UpdateEXTDEPEQUCH()
            II- Generer les simulation --> generateSim()
            III- Update EXTDEPEQU (part2) --> updateDeplacementsIdSim
            IV- Caluculer les valeurs des Simulation: updateGeneratedSimValues()
            V- gvSimulation BINDING : listeSimulation()
         ---------------------------------------------------------
         */
        protected void BTNgenerer_Click(object sender, EventArgs e)
        {
            updateDeplacements();
            generateSim();
            updateDeplacementsIdSim();
            updateGeneratedSimValues();
            string trace = Session["UserName"].ToString() + " a généré les simulations";
            insertIntoTrace(trace);
            msgsim.Text = "la prochaine génération des simulations commence le 1er du mois prochain..";
            listeSimulation();
            BTNgenerer.Enabled = false;

        }


        /*I- Update EXTDEPEQU (Part1) --> UpdateDeplacements()
            1- Selectionner tous les deplacements du mois PRECEDENT
            2- Si la date de sortie est nulle (l'equipement est toujours dans ce chantier) alors
                a) Remplacer la par la date courante --> setDeplacementInfo()
                b) Inserer une nouvelle ligne pour le prochain mois
        */

        public void updateDeplacements()
        {
            int iddep = 0;
            Decimal taux = 0;
            string DE = "", desEq = "", desCh = "", codeEq = "";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT * FROM EXTDEPEQU WHERE (DATEPART(m, DT_ENTREE) = DATEPART(m, DATEADD(m, - 1, GETDATE()))) AND (DATEPART(yyyy, DT_ENTREE) = DATEPART(yyyy, DATEADD(m, - 1, GETDATE()))) AND DT_SORTIE IS NULL";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                iddep = Convert.ToInt32(reader["ID_DEP"].ToString());
                taux = Convert.ToDecimal(reader["NU_TAUXLOC"].ToString());
                DE = reader["DT_ENTREE"].ToString();
                codeEq = reader["ST_CODEQ"].ToString();
                desEq = reader["ST_DESEQ"].ToString();
                desCh = reader["ST_DESCH"].ToString();

                setDeplacementInfo(iddep, taux, DE);
                insertNewDep(codeEq, desEq, desCh, taux);
            }

            con.Close();
        }

        public void setDeplacementInfo(int iddep, Decimal taux, string DE)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var last = month.AddDays(-1);
            DateTime d = Convert.ToDateTime(DE);
            DateTime now = DateTime.Now;
            int nbjrs = (now - d).Days;
            string qry = "UPDATE EXTDEPEQU SET DT_SORTIE=@DT_SORTIE, NU_NBJRS=@NU_NBJRS, NU_VALEUR=@NU_VALEUR WHERE ID_DEP=" + iddep;
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@DT_SORTIE", last.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@NU_NBJRS", nbjrs);
            cmd.Parameters.AddWithValue("@NU_VALEUR", (nbjrs * taux));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void insertNewDep(string codeEq, string desEq, string desCh, Decimal taux)
        {
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "INSERT INTO EXTDEPEQU (ST_CODEQ, ST_DESEQ, ST_DESCH, DT_ENTREE, NU_TAUXLOC) VALUES (@ST_CODEQ, @ST_DESEQ, @ST_DESCH, @DT_ENTREE, @NU_TAUXLOC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@ST_CODEQ", codeEq);
            cmd.Parameters.AddWithValue("@ST_DESEQ", desEq);
            cmd.Parameters.AddWithValue("@ST_DESCH", desCh);
            cmd.Parameters.AddWithValue("@DT_ENTREE", firstDayOfMonth.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@NU_TAUXLOC", taux);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //------------------------ fin I: Update EXTDEPEQU (Part1) ---------------------------------

        /* II- Generer les simulation
                1) selectionner les distinct chantier appartenant a EXTDEPEQU (qui ont loue des equipement pendant ce mois) => GenerateSim()
                2) our chaque chantier 
                    a) get directeur de chaque chantier
                    b) insere une nouvelle ligne dans EXTSIMULATION => insertIntoSim()
         */
        public void generateSim()
        {
            string iddir = "";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT DISTINCT ST_DESCH FROM EXTDEPEQU WHERE ST_DESCH NOT LIKE 'PARC' AND (DATEPART(m, DT_ENTREE) = DATEPART(m, DATEADD(m, - 1, GETDATE()))) AND (DATEPART(yyyy, DT_ENTREE) = DATEPART(yyyy, DATEADD(m, - 1, GETDATE()))) ";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                iddir = getDirecteur(reader["ST_DESCH"].ToString());
                insertIntoSim(reader["ST_DESCH"].ToString(), iddir);

            }
            con.Close();
        }

        public string getDirecteur(string ch)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT ID_CODUTIDIR FROM EXTCHANTIER WHERE ST_DESCH LIKE '" + ch + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader.GetString(0);
            }
            con.Close();
            return "";
        }


        public void insertIntoSim(string chantier, string iddir)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string des = "Simulation de " + chantier;
            string qry = "INSERT INTO EXTSIMULATION (ST_DESIM, ST_DESCH, DT_CREATE, CODUTI_CREATE, ID_DIR, SIM_STATE) VALUES (@ST_DESIM, @ST_DESCH, @DT_CREATE, @CODUTI_CREATE, @ID_DIR, 'UNVALIDATED')";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@ST_DESIM", des);
            cmd.Parameters.AddWithValue("@ST_DESCH", chantier);
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@CODUTI_CREATE", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@ID_DIR", iddir);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //---------------------------------------- fin II- Generer les simulation--------------------------------------------------

        /*III- Update EXTDEPEQU (part2)
                1- Selectionner les simulation du mois courant 
                2- Pour chaque simulation:
                     a) Selectionner les deplacement Correspendant a chaque simulation => selectDepCorrespendant()
                     b) pour chaque deplacement set ID_SIM par l id de cette simualtion => UpdateID_SIM()
         */
        public void updateDeplacementsIdSim()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT * from EXTSIMULATION WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate()) ";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                selectDepCorrespendant(reader);
            }
            con.Close();
        }
        public void selectDepCorrespendant(SqlDataReader readerSim)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT * from EXTDEPEQU WHERE (DATEPART(m, DT_ENTREE) = DATEPART(m, DATEADD(m, - 1, GETDATE()))) AND (DATEPART(yyyy, DT_ENTREE) = DATEPART(yyyy, DATEADD(m, - 1, GETDATE()))) AND ST_DESCH LIKE '" + readerSim["ST_DESCH"].ToString() + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UpdateID_SIM(Convert.ToInt32(reader["ID_DEP"].ToString()), Convert.ToInt32(readerSim["ID_SIM"].ToString()));
            }
            con.Close();

        }
        public void UpdateID_SIM(int idDEP, int idSIM)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "UPDATE EXTDEPEQU SET ID_SIM = " + idSIM + " WHERE ID_DEP = " + idDEP;
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //--------------------------------- FIN III- Update EXTDEPEQU (part2) ------------------------------------------------
        /* IV- Caluculer les valeurs des Simulation:
                1- Selectionner tous les simulations de mois courant => updateGeneratedSimValues()()
                2- Calculer la valeur => calculValeurSim()
                3- Update NU_VALEUR => setValeurSim()
         */

        public void updateGeneratedSimValues()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT * FROM EXTSIMULATION WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate()) ";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                calculValeurSim(Convert.ToInt32(reader["ID_SIM"].ToString()));
            }

        }
        public void calculValeurSim(int idSIM)
        {

            Decimal val = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT SUM(NU_VALEUR) FROM EXTDEPEQU WHERE ID_SIM = " + idSIM;
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetDecimal(0);
            }

            setValeurSim(val, idSIM);
            con.Close();

        }
        public void setValeurSim(Decimal val, int idSIM)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "UPDATE EXTSIMULATION SET NU_VALEUR = @valeur WHERE ID_SIM = @idsim";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@valeur", val);
            cmd.Parameters.AddWithValue("@idsim", idSIM);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //------------------------------FIN IV- Caluculer les valeurs des Simulation -------------------------------------------
        // V- gvSimulations BINDING : listeSimulation() 
        private void listeSimulation()
        {
            int counted = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    string sql = "SELECT ST_DESCH, ID_SIM , DT_CREATE, NU_VALEUR FROM EXTSIMULATION WHERE (MONTH(DT_CREATE) = MONTH(GETDATE())) AND (YEAR(DT_CREATE) = YEAR(GETDATE()))";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvSimulation.DataSource = dt;
                        gvSimulation.DataBind();
                        counted = gvSimulation.Rows.Count;
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvSimulation.PageIndex = e.NewPageIndex;
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
        // inserer l'action de generation des simulation
        public void insertIntoTrace(string Trace)
        {
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

        //------------------------------------ END :) -----------------------------------------------
    }
}