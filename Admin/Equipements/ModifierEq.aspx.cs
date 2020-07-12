using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace PFE.Equipements
{
    public partial class ModifierEq : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cnx.Text = connetionString;
                queryMag.Text = "SELECT ID_CODMAG FROM MAG WHERE ST_DES LIKE ";
                queryFour.Text = "SELECT ST_FOU FROM EXTFOURNISSEUR WHERE ST_FOU LIKE ";
                queryCateg.Text = "SELECT ST_CATEGORIE FROM EXTCATEGORIE WHERE ST_CATEGORIE LIKE ";
                ddlEtatBinding();
                remplirChamps();
                bindRepeater();
                int n = nombreNotifs();
                nbrNotif.Text = n.ToString();


            }
            else { }

        }
        // bind la liste des etats d'un equipement
        public void ddlEtatBinding()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string com = "Select * from EXTETATEQU";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            etat.DataSource = dt;
            etat.DataTextField = "ST_ETAT";
            etat.DataValueField = "ID_ETAT";
            etat.DataBind();
            con.Close();
        }
        // binding the notification repeater
        protected void bindRepeater()
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
        // recuperer l'emplacement de l'equipement
        public string getEmpEq()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry_emplacement = "SELECT TOP(1) ST_DESCH FROM EXTMVTEQUCH WHERE ST_CODEEQ LIKE '" + Request.QueryString["ST_CODCOU"] + "' ORDER BY DT_MVT DESC";
            SqlCommand cmd_emplacement = new SqlCommand(qry_emplacement, con);
            SqlDataReader readerEmp = cmd_emplacement.ExecuteReader();
            while (readerEmp.Read())
            {
                return readerEmp.GetString(0).ToUpper();
            }
            return String.Empty;
        }
        // remplir les champs initialement par les donnees de l'equipement voulait etre modifié
        public void remplirChamps()
        {
            SqlConnection con = new SqlConnection(connetionString);
            string qry = "SELECT EQU.ID_NUMEQU, EMP.ST_DES, EQU.ID_CODIMP, MAG.ST_DES AS MAGASIN, EQU.NU_NIV, EQU.NU_ORD, EQU.ST_CODCOU, EQU.ST_DESEQU, EQU.ST_NOMFOU, EQU.DT_ACH, EQU.NU_PRIACH AS ACHAT, EQU.ST_OBS, EQU.NU_PRIACT AS TAUX, EQU.DT_FINVIE, EXTETATEQU.ID_ETAT, EXTETATEQU.ST_ETAT FROM EQU LEFT OUTER JOIN MAG ON EQU.ID_CODGES = MAG.ID_CODMAG LEFT OUTER JOIN EXTETATEQU ON EQU.ST_ETA = EXTETATEQU.ID_ETAT LEFT OUTER JOIN EMP ON EQU.ID_NUMEMP = EMP.ID_NUMEMP WHERE EQU.ST_CODCOU LIKE '" + Request.QueryString["ST_CODCOU"] + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idEQ.Text = reader["ID_NUMEQU"].ToString();
                CodCouEQ.Text = reader["ST_CODCOU"].ToString();
                Categorie.Text = reader["ID_CODIMP"].ToString();
                DesEQ.Text = reader["ST_DESEQU"].ToString();
                codeMag.Text = reader["MAGASIN"].ToString();
                fournisseur.Text = reader["ST_NOMFOU"].ToString();
                dtFinVie.Text = reader["DT_FINVIE"].ToString();
                dtAch.Text = reader["DT_ACH"].ToString();
                PrixEQ.Text = reader["ACHAT"].ToString();
                TauxLoc.Text = reader["TAUX"].ToString();
                numOrd.Text = reader["NU_ORD"].ToString();
                numNiv.Text = reader["NU_NIV"].ToString();
                EQobs.Text = reader["ST_OBS"].ToString();
                if (reader["ST_ETAT"].ToString() == "")
                {
                    etat.SelectedIndex = 4;
                }
                else { etat.SelectedValue = reader["ST_ETAT"].ToString(); }
                string emp = getEmpEq();
                if (emp == String.Empty)
                {
                    EmpEQ.Text = "";
                }
                else EmpEQ.Text = emp;

            }
            reader.Close();
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
        // update the equipment data
        protected void BTNvaliderModif_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry_Update = "Update EQU SET NU_ORD=@NU_ORD, NU_NIV=@NU_NIV, ST_DESEQU=@ST_DESEQU, ID_CODGES=@ID_CODGES, ST_NOMFOU=@ST_NOMFOU, DT_ACH=@DT_ACH, ST_ETA=@ST_ETA, NU_PRIACH=@NU_PRIACH, NU_PRIACT=@NU_PRIACT, DT_FINVIE=@DT_FINVIE, ST_OBS=@ST_OBS WHERE ST_CODCOU LIKE '" + Request.QueryString["ST_CODCOU"] + "'";
            SqlCommand cmd_Update = new SqlCommand(qry_Update, con);
            cmd_Update.Parameters.AddWithValue("@NU_PRIACT", Decimal.Parse(TauxLoc.Text));
            cmd_Update.Parameters.AddWithValue("@ST_OBS", EQobs.Text.ToUpper());
            cmd_Update.Parameters.AddWithValue("@NU_PRIACH", Decimal.Parse(PrixEQ.Text));
            cmd_Update.Parameters.AddWithValue("@ST_DESEQU", DesEQ.Text.ToUpper());
            cmd_Update.Parameters.AddWithValue("@NU_ORD", Int32.Parse(numOrd.Text));
            cmd_Update.Parameters.AddWithValue("@NU_NIV", Int32.Parse(numNiv.Text));
            cmd_Update.Parameters.AddWithValue("@ID_CODGES", codeMag.Text.ToUpper());
            cmd_Update.Parameters.AddWithValue("@ST_NOMFOU", fournisseur.Text);
            cmd_Update.Parameters.AddWithValue("@ST_ETA", etat.SelectedItem.Text);
            cmd_Update.Parameters.AddWithValue("@DT_FINVIE", Convert.ToDateTime(dtFinVie.Text));
            cmd_Update.Parameters.AddWithValue("@DT_ACH", Convert.ToDateTime(dtAch.Text));
            cmd_Update.ExecuteNonQuery();
            insertIntoTrace();
            MsgResult("Modification validée!!", "Information");
            insertIntoTrace();
            con.Close();
            Categorie.ReadOnly = true;
            DesEQ.ReadOnly = true;
            EmpEQ.ReadOnly = true;
            codeMag.ReadOnly = true;
            PrixEQ.ReadOnly = true;
            TauxLoc.ReadOnly = true;
            numOrd.ReadOnly = true;
            numNiv.ReadOnly = true;
            EQobs.ReadOnly = true;
            fournisseur.ReadOnly = true;
            dtAch.ReadOnly = true;
            dtFinVie.ReadOnly = true;

        }
        // si l'utilisateur veut modifier l 'emplacemnt de l'equipement on le redirecte vers la page de DeplacerEq.aspx
        protected void BTNdeplacer_Click(object sender, EventArgs e)
        {
            Response.Redirect("DeplacerEq.aspx?ST_CODCOU=" + CodCouEQ.Text);
        }
        /* La table EXTTRACE  contient tout les actions des utilisateurs
         * Dans cet etapes on va inserer le faite de modofiaction de l'equipement par l'utilisateur
         */
        public void insertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a modifié l'équipement " + DesEQ.Text;
            string qry = "INSERT INTO EXTTRACE ( NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) values (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", trace);
            cmd.Parameters.AddWithValue("@ADR_MAC", getMacAddress());
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsulterEq.aspx");
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