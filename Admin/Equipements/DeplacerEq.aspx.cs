using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.Services;
using System.Web.UI.WebControls;

// attention: comparer date mvt avec la derniere date de deplacement
namespace PFE.Equipements
{
    public partial class DeplacerEq : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                mydiv.Visible = false;
                MVTdate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cnx.Text = connetionString;
                query.Text = "SELECT ST_CODCOU FROM EQU WHERE ST_DESEQU LIKE ";
                queryCH.Text = "SELECT ST_DESCH FROM EXTCHANTIER WHERE ST_DESCH LIKE ";
                TypeMvtBinding();
                BindRepeater();
                int n = NbreNotif();
                nbrNotif.Text = n.ToString();
                //  ST_CODCOU is sent from ModifierEq.aspx when the admin want to change the location of the equipment
                if (Request.QueryString["ST_CODCOU"] != "")
                {
                    CodcouEQ.Text = Request.QueryString["ST_CODCOU"];
                }
                //  ID_DEMANDE is sent from DemandeEq.aspx When the request of location is ACCEPTED
                if (Request.QueryString["ID_DEMANDE"] != "")
                {
                    getDemandeData(Convert.ToInt32(Request.QueryString["ID_DEMANDE"]));
                    //getInfos();
                }
            }
            else { }

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
        // binding Dropdownlist MVType par les types existant dans la table EXTTYPEMVT
        public void TypeMvtBinding()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string com = "Select * from EXTTYPEMVT";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            MVType.DataSource = dt;
            MVType.DataTextField = "ST_TYPEMVT";
            MVType.DataValueField = "ID_TYPEMVT";
            MVType.DataBind();
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
        /*BTNdeplacer_Click se devise en 3 etapes principaux
         * 1- insertion de mouvement dans la table EXTMVTEQUCH
         * 2- insertion de mouvement dans la table EXTDEPEQU
         * 3- insertion de l'action "deplacement de l'equipement" dans la table EXTTRACE
         */
        protected void BTNdeplacer_Click(object sender, EventArgs e)
        {
            if (texboxesAreEmpty())
            {
                mydiv.Visible = true;
            }
            else if (CHcin.Text.Length < 8)
            {
                errormsg.Text = "La CIN doit contenir 8 chiffres!";
                mydiv.Visible = true;
            }
            else
            {
                try
                {
                    insertIntoMvt();
                    insertIntoCharge();
                    insertIntoDeqEqu();
                    string Trace = Session["UserName"].ToString() + " a déplacé l'équipement " + CodcouEQ.Text;
                    insertIntoTrace(Trace);
                    if (Request.QueryString["ID_DEMANDE"] != "")
                    {
                        setDemandeTraitee(Convert.ToInt32(Request.QueryString["ID_DEMANDE"]));
                    }
                }
                catch (Exception ex)
                {
                    MsgResult(ex.Message, "Information");
                }
            }
        }
        public void insertIntoMvt()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryMVT = "INSERT INTO EXTMVTEQUCH (ST_CODEEQ, ST_DESCH, NU_CIN, ST_TYPEMVT, NU_CPT_CHARGE, DT_MVT, ST_OBS) VALUES (@ST_CODEEQ, @ST_DESCH, @NU_CIN, @ST_TYPEMVT, @NU_CPT_CHARGE, @DT_MVT, @ST_OBS)";
            SqlCommand cmd = new SqlCommand(qryMVT, con);
            cmd.Parameters.AddWithValue("@ST_CODEEQ", CodcouEQ.Text);
            cmd.Parameters.AddWithValue("@ST_DESCH", CHDest.Text);
            cmd.Parameters.AddWithValue("@NU_CIN", CHcin.Text);
            cmd.Parameters.AddWithValue("@ST_TYPEMVT", MVType.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@NU_CPT_CHARGE", Compteur.Text);
            cmd.Parameters.AddWithValue("@DT_MVT", MVTdate.Text);
            cmd.Parameters.AddWithValue("@ST_OBS", MVTobs.Text);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //-------------------- insertion de mouvement dans la table EXTDEPEQU--------------------------
        public void insertIntoDeqEqu()
        {
            int iddep = getIdLastDep();
            if (iddep != -1)
            {
                setDateSortie(iddep);
            }
            string desEq = getEqInfo()["ST_DESEQU"].ToString();
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "INSERT INTO EXTDEPEQU (ST_CODEQ, ST_DESEQ, ST_DESCH, DT_ENTREE, NU_TAUXLOC) VALUES (@ST_CODEQ, @ST_DESEQ, @ST_DESCH, @DT_ENTREE, @NU_TAUX)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@ST_CODEQ", CodcouEQ.Text);
            cmd.Parameters.AddWithValue("@ST_DESEQ", desEq);
            cmd.Parameters.AddWithValue("@ST_DESCH", CHDest.Text);
            cmd.Parameters.AddWithValue("@DT_ENTREE", MVTdate.Text);
            cmd.Parameters.AddWithValue("@NU_TAUX", Convert.ToDecimal(taux.Text));
            cmd.ExecuteNonQuery();
        }
        // recuperer l'ID du dernier deplacement de l'equipement saisie
        public int getIdLastDep()
        {
            int iddep = -1;
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryID = "SELECT TOP(1) ID_DEP FROM EXTDEPEQU WHERE ST_CODEQ LIKE '" + CodcouEQ.Text + "' ORDER BY DT_ENTREE DESC ";
            SqlCommand cmdID = new SqlCommand(qryID, con);
            SqlDataReader reader = cmdID.ExecuteReader();
            while (reader.Read())
            {
                iddep = Int32.Parse(reader["ID_DEP"].ToString());
            }
            con.Close();
            return iddep;
        }
        /*si L'ID de dernier deplacement n'est pas null
        definit la valeur de date sortie par la date de mouvement actuel
        calculer le nombre des jours de location DT_SORTIE-DT_ENTREE
        calculer la valeur = Nombres des jours * Taux de location
             */
        public void setDateSortie(int id)
        {
            Decimal tauxLoc = Convert.ToDecimal(taux.Text);
            string dtEntree = getDateEntree(id);
            DateTime d = Convert.ToDateTime(dtEntree);
            DateTime now = Convert.ToDateTime(MVTdate.Text);
            int nbjrs = (now - d).Days;
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "UPDATE EXTDEPEQU SET DT_SORTIE=@DT_SORTIE, NU_NBJRS=@NU_NBJRS, NU_VALEUR=@NU_VALEUR WHERE ID_DEP=" + id;
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@DT_SORTIE", MVTdate.Text);
            cmd.Parameters.AddWithValue("@NU_NBJRS", nbjrs);
            cmd.Parameters.AddWithValue("@NU_VALEUR", (nbjrs * tauxLoc));
            cmd.ExecuteNonQuery();
            con.Close();
        }
        // recuperer le Date entree de du deplacement ayant l' ID passé en parametre
        public string getDateEntree(int iddep)
        {
            string dateEntree = "";
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT TOP(1) DT_ENTREE FROM EXTDEPEQU WHERE ID_DEP = " + iddep + " ORDER BY DT_ENTREE DESC";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dateEntree = reader["DT_ENTREE"].ToString();
            }
            con.Close();
            return dateEntree;
        }
        // recuperer le taux de location de l'equipement voulait etre deplacer
        public SqlDataReader getEqInfo()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT NU_PRIACT, ST_DESEQU FROM EQU WHERE ST_CODCOU LIKE '" + CodcouEQ.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader;
            }
            else return null;
        }
        // inserer une ligne dans la Table EXTCHARGE
        public void insertIntoCharge()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryCHARGE = "INSERT INTO EXTCHARGE (CPT_CHARGE, NU_CIN) VALUES (@CPT_CHARGE, @NU_CIN)";
            SqlCommand cmd = new SqlCommand(qryCHARGE, con);
            cmd.Parameters.AddWithValue("@CPT_CHARGE", Compteur.Text);
            cmd.Parameters.AddWithValue("@NU_CIN", CHcin.Text);
            int result = cmd.ExecuteNonQuery();
            if (result >= 0)
            {
                MsgResult("Mouvement d equipement " + CodcouEQ.Text + " de " + EmpActuel.Text + " vers " + CHDest.Text, "information");
                listeMouvements();
            }
            con.Close();

        }
        /* Filtre permet de filtrer les donnees dans ce cas:
        recuperer a partir de la table EQU la liste des ST_CODCOU ou la designation de l'equipement
        contient la chaine de caractere saisie par l'utilisateur dans le champs "designation d'equipement=>CodcouEQ.Text"
        de meme pour les CHANTIERS
        recuperer a partir de la table EXTCHANTIER la liste des designations des chantiers qu'elles contiennent
        la chaine de caractere saisie par l'utilisateur dans le champs "Chantier destination=>CHDest.Text"
        */
        [WebMethod]
        public static List<string> Filtre(string eqDes, string query, string cnx)
        {
            List<string> Designation = new List<string>();
            string querys = string.Format(query + "'%{0}%'", eqDes);
            using (SqlConnection con = new SqlConnection(@cnx))
            {
                using (SqlCommand cmd = new SqlCommand(querys, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Designation.Add(reader.GetString(0));
                    }
                    con.Close();
                }
            }
            return Designation;
        }
        /*getEmplacement: recupere la localisation de l'equipement en selectionnant ST_DESCH du dernier mouvement 
         de cet equipement s'elle existe
         et inserer la valeur retournee dans le champs de "Emplacement Actuel" =>EmpActuel.Text
         sinon on recupere le champs emplacement de la table equiepemnet
         */
        protected void getVal(object sender, EventArgs e)
        {
            string query = "SELECT TOP(1) ST_DESCH FROM EXTMVTEQUCH WHERE EXTMVTEQUCH.ST_CODEEQ LIKE '" + CodcouEQ.Text + "' ORDER BY EXTMVTEQUCH.DT_MVT DESC";
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EmpActuel.Text = reader.GetString(0);
                    }
                    if (EmpActuel.Text == "")
                    {
                        EmpActuel.Text = getEmplacement();
                    }
                }
                con.Close();
            }
            taux.Text = getEqInfo()["NU_PRIACT"].ToString();
        }
        protected void getInfos()
        {
            string query = "SELECT TOP(1) ST_DESCH FROM EXTMVTEQUCH WHERE EXTMVTEQUCH.ST_CODEEQ LIKE '" + CodcouEQ.Text + "' ORDER BY EXTMVTEQUCH.DT_MVT DESC";
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EmpActuel.Text = reader.GetString(0);
                    }
                    if (EmpActuel.Text == "")
                    {
                        EmpActuel.Text = getEmplacement();
                    }
                }
                con.Close();
            }
            taux.Text = getEqInfo()["NU_PRIACT"].ToString();
        }

        public string getEmplacement()
        {
            string qry = "SELECT EMP.ST_DES FROM EQU INNER JOIN EMP ON EQU.ID_NUMEMP = EMP.ID_NUMEMP WHERE EQU.ST_CODCOU LIKE '" + CodcouEQ.Text + "'";
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                }
                con.Close();
            }
            return "";
        }

        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }


        /* apres le deplacement de l'equipement
           l'utilisateur peut consulter tout les deplacement de l'equipement deplacé
         */

        private void listeMouvements()
        {
            int counted = 0;
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    string sql = "SELECT ST_DESCH, ST_TYPEMVT, CONVERT(varchar, DT_MVT,111) AS DT_MVT FROM EXTMVTEQUCH WHERE EXTMVTEQUCH.ST_CODEEQ LIKE '" + CodcouEQ.Text + "' ORDER BY DT_MVT DESC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvEquipement.DataSource = dt;
                        gvEquipement.DataBind();
                        counted = gvEquipement.Rows.Count;
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvEquipement.PageIndex = e.NewPageIndex;
        }

        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            CodcouEQ.Text = String.Empty;
            EmpActuel.Text = String.Empty;
            CHDest.Text = String.Empty;
            CHcin.Text = String.Empty;
            MVTdate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Compteur.Text = String.Empty;
            MVTobs.Text = String.Empty;
            taux.Text = String.Empty;
        }
        /* La table EXTTRACE  contient tout les actions des utilisateurs
         * Dans cet etapes on va inserer le faite de deplacement d'equipement par l'utilisateur
         */
        public void insertIntoTrace(string Trace)
        {
            SqlConnection con = new SqlConnection(connetionString);
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


        //-------------------------- Partie de l'acceptation de la demande de location 
        /* Apres recevoir une demande de location d'un directeur de chantier
         si l'adiministrateur accepte cette demande on passe l ID de la demande de la page DemandeEq.aspx
         vers cette page 
         ici on definit la colonne ST_STATUS correspondant a l'ID recu en ACCEPTED
         cette methode sera appelee en cliquant le bouton deplacer*/
        public void setDemandeTraitee(int iddemande)
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryMVT = " UPDATE EXTDEMLOC SET ST_STATUS= 'ACCEPTED', DT_TRAIT=@DT_TRAIT  WHERE ID_DEMANDE=" + iddemande;
            SqlCommand cmd = new SqlCommand(qryMVT, con);
            cmd.Parameters.AddWithValue("@DT_TRAIT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            con.Close();
        }
        /*getDemandeData : permet de recuperer le code de l'equipement et le chantier correspondant a la demande
         * pour remplir les champs CODCOU et CHdest par ces informations
         * cette methode est appelee dans la methode PageLoad
         */
        public void getDemandeData(int idDemande)
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryMVT = " SELECT ST_CODEQ, ST_DESCH FROM EXTDEMLOC WHERE ID_DEMANDE =" + idDemande;
            SqlCommand cmd = new SqlCommand(qryMVT, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                CodcouEQ.Text = reader["ST_CODEQ"].ToString();
                CodcouEQ.ReadOnly = true;
                CHDest.Text = reader["ST_DESCH"].ToString();
                CHDest.ReadOnly = true;
                insertIntoTrace(Session["UserName"].ToString() + " a accepté la demande de location de " + reader["ST_CODEQ"].ToString());

            }
        }
        public Boolean texboxesAreEmpty()
        {
            if (CodcouEQ.Text == String.Empty || CHDest.Text == String.Empty || CHcin.Text == String.Empty || Compteur.Text == String.Empty)
            {
                return true;
            }
            return false;
        }
    }
}
