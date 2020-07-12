using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;

namespace PFE.Equipements
{
    public partial class AjouterEq : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlEtatBinding();
                dtAch.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dtFinVie.Text = DateTime.Now.ToString("yyyy-MM-dd");
                cnx.Text = connetionString;
                query.Text = "SELECT ST_DES FROM EMP WHERE ST_DES LIKE ";
                queryMag.Text = "SELECT ID_CODMAG FROM MAG WHERE ST_DES LIKE ";
                queryFour.Text = "SELECT ST_FOU FROM EXTFOURNISSEUR WHERE ST_FOU LIKE ";
                queryCateg.Text = "SELECT ST_CATEGORIE FROM EXTCATEGORIE WHERE ST_CATEGORIE LIKE ";
                bindRepeater();
                mydiv.Visible = false;
                int n = nombreNotifs();
                nbrNotif.Text = n.ToString();
            }
            else { }

        }
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
        private static Random random = new Random();
        public static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        protected void BTNajouter_CLICK(object sender, EventArgs e)
        {
            if (textboxesAreEmpty() == true)
            {
                mydiv.Visible = true;
            }
            else if (codeEquExist() == true || idEqExist() == true)
            {
                if (codeEquExist() && idEqExist())
                {
                    errormsg.Text = "Le code et l'ID de l'équipement que vous avez entré déjà existent!";
                    mydiv.Visible = true;
                }
                else if (codeEquExist() && !idEqExist())
                {
                    errormsg.Text = "Le code de l'équipement que vous avez entré déjà existe!";
                    mydiv.Visible = true;
                }
                else if (!codeEquExist() && idEqExist())
                {
                    errormsg.Text = "L'ID de l'équipement que vous avez entré déjà existe!";
                    mydiv.Visible = true;
                }
            }
            else
            {
                try
                {
                    string codlon = randomString(20);
                    SqlConnection con = new SqlConnection(connetionString);
                    string iii = "INSERT INTO EQU (ID_NUMEQU,NU_ORD,NU_NIV, ST_CODCOU, ID_CODIMP, ST_CODLON, ST_DESEQU,ID_NUMEMP, ID_CODGES, ST_NOMFOU, DT_ACH, NU_PRIACH, NU_PRIACT, DT_FINVIE, ST_OBS, ST_ETA) " +
                        "VALUES (" + Convert.ToInt32(idEQ.Text) + ",1,2,'" + CodCouEQ.Text + "','" + Categorie.Text + "','" + codlon + "','" + DesEQ.Text + "'," + Convert.ToInt32(getIdEmp(EmpEQ.Text)) + ",'" + codeMag.Text + "','" + fournisseur.Text + "','" + dtAch.Text + "'," + Decimal.Parse(PrixEQ.Text) + "," + Decimal.Parse(TauxLoc.Text) + ",'" + dtFinVie.Text + "','" + EQobs.Text + "', '" + etat.SelectedValue.ToString() + "')";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(iii, con);
                    int result = cmd.ExecuteNonQuery();
                    if (result >= 0)
                    {
                        insertIntoTrace();
                        MsgResult("Vous avez bien inseré l équipement " + DesEQ.Text, "Information");
                        viderTexboxes();
                    }
                    con.Close();

                }
                catch (Exception ex)
                {
                    MsgResult(ex.Message, "Information");
                }
            }

        }
        /*
         public int getIdCategorie()
        {
            string qry = "SELECT ID_CATEGORIE FROM EXTCATEGORIE WHERE ST_CATEGORIE LIKE '" + Categorie.Text + "'";
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                return reader.GetInt32(0);
            return 1;
        }*/
        /*getIdEmp: Dans la formulaire d'ajout equipement l'utilisateur va entrer la designation de l'emplacement
         mais la valeur qu'on doit inserer dans la table EQu est l' ID de cet emplacement
         cette fonction permet de recuperer l ID de l'emplacment saisie
         */
        public Boolean codeEquExist()
        {
            string qry = "SELECT ST_CODCOU FROM EQU WHERE ST_CODCOU LIKE '" + CodCouEQ.Text + "'";
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            return false;
        }
        public Boolean idEqExist()
        {
            string qry = "SELECT ID_NUMEQU FROM EQU WHERE ST_CODCOU LIKE '" + CodCouEQ.Text + "'";
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            return false;

        }
        public string getIdEmp(string empEqu)
        {
            string idEmp = "";
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryEmp = "SELECT ID_NUMEMP FROM EMP WHERE ST_DES LIKE '" + empEqu + "'";
            SqlCommand cmdEmp = new SqlCommand(qryEmp, con);
            SqlDataReader readerEmp = cmdEmp.ExecuteReader();
            while (readerEmp.Read())
            {
                idEmp = readerEmp["ID_NUMEMP"].ToString();
            }
            con.Close();
            return idEmp;
        }
        public void viderTexboxes()
        {
            CodCouEQ.Text = String.Empty;
            Categorie.Text = String.Empty;
            DesEQ.Text = String.Empty;
            EmpEQ.Text = String.Empty;
            PrixEQ.Text = String.Empty;
            TauxLoc.Text = String.Empty;
            EQobs.Text = String.Empty;
            dtAch.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtFinVie.Text = DateTime.Now.ToString("yyyy-MM-dd");
            codeMag.Text = String.Empty;
        }
        // verifer si tout les chaps sont remplis
        public Boolean textboxesAreEmpty()
        {
            if (CodCouEQ.Text == String.Empty || Categorie.Text == String.Empty || DesEQ.Text == String.Empty || PrixEQ.Text == String.Empty || TauxLoc.Text == String.Empty || dtFinVie.Text == String.Empty || idEQ.Text == String.Empty || dtAch.Text == String.Empty || codeMag.Text == String.Empty)
            {
                return true;
            }
            else return false;
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
            string trace = Session["UserName"].ToString() + " a ajouté l'équipement " + DesEQ.Text;
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