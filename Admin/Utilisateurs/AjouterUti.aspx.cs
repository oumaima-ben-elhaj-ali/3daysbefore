using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace PFE.Utilisateurs
{
    public partial class AjouterUser : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                mydiv.Visible = false;
                CHaddSuccess.Visible = false;
                chantierInfo.Visible = false;
                desProBinding();
                bindNotifRepeater();
                nbrNotif.Text = nombreNotifs().ToString();
            }
        }
        /*
         * insertion des donnees dans la table UTI
         * insertion des donnees dans la table INTER
         * insertion l'action d'ajout de l'utilisateur dans la table EXTTRACE
         */
        protected void BTNajouter_Click(object sender, EventArgs e)
        {
            if (!verifierSaisies())
            {
            }
            else
            {
                try
                {
                    insertIntoUti();
                    insertIntoInter();
                    insertIntoTrace();
                    viderTextboxes();
                    mydiv.Visible = false;
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }
        public Boolean verifierSaisies()
        {
            if (textboxesAreEmpty() == true)
            {
                mydiv.Visible = true;
                return false;
            }
            else if (idUserExisted() || cinExist())
            {
                if (idUserExisted() && cinExist())
                {
                    errormsg.Text = "La CIN et l'ID utilisateur que vous avez entré déjà existent!";
                    mydiv.Visible = true;
                    return false;
                }
                else if (idUserExisted() && !cinExist())
                {
                    errormsg.Text = "L'ID utilisateur que vous avez entré déjà existe!";
                    mydiv.Visible = true;
                    return false;
                }
                else if (!idUserExisted() && cinExist())
                {
                    errormsg.Text = "La CIN que vous avez entré déjà existe!";
                    mydiv.Visible = true;
                    return false;
                }
                else if (codeInt.Text.Length < 8)
                {
                    errormsg.Text = " La CIN doit contenir 8 chiffres!";
                    mydiv.Visible = true;
                    return false;
                }
                else if (Tel.Text.Length < 8)
                {
                    errormsg.Text = " La numéro de téléphone doit contenir 8 chiffres!";
                    mydiv.Visible = true;
                    return false;
                }
            }
            else return true;
            return true;
        }
        // binding le dropdownlist desPro a patir de la table PRO
        public void desProBinding()
        {
            SqlConnection con = new SqlConnection(connetionString);
            string com = "SELECT * FROM PRO";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            desPro.Items.Clear();
            desPro.DataSource = dt;
            desPro.DataTextField = "ST_DES";
            desPro.DataValueField = "ID_CODPRO";
            desPro.DataBind();
        }
        protected void desPro_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTNajouter.Enabled = true;
            if (desPro.SelectedItem.ToString() == "DIRECTEUR CHANTIER")
            {
                chantierInfo.Visible = true;
            }
            else chantierInfo.Visible = false;
        }

        /*---------------------------- Si l'utilisateur est un directeur chantier---------------
         si l'utilisateur ajouté est un directeur chantier on peut lui affecter des chantier
         * si le chantier existe deja on mise a jour son ID_CODUTIDIR par ID_CODUTI de cet utilisateur
         * sinon on insere un nouvelle chantier ayant ID_CODUTIDIR= ID_CODUTI de cet utilisateur
         */
        protected void ConfirmCH_Click(object sender, EventArgs e)
        {
            confirmerChantier();
        }
        public void confirmerChantier()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = " SELECT ID_CHANTIER FROM EXTCHANTIER WHERE ST_DESCH LIKE '" + chantier.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                updateChantier(reader.GetInt32(0));
                CHaddSuccess.Visible = true;
            }
            else
            {
                insertChantier();
                CHaddSuccess.Visible = true;
            }
            chantier.Text = String.Empty;
            con.Close();
        }

        public void updateChantier(int id)
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = " UPDATE EXTCHANTIER SET ID_CODUTIDIR = '" + idUser.Text + "' WHERE ID_CHANTIER =" + id;
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void insertChantier()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "INSERT INTO EXTCHANTIER (ST_DESCH, DT_CREATE, ID_CODUTIDIR) VALUES (@ST_DESCH, @DT_CREATE, @ID_CODUTIDIR)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@ST_DESCH", chantier.Text);
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            cmd.Parameters.AddWithValue("@ID_CODUTIDIR", idUser.Text);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //------------------ inserer le nouveau user dans la table UTI----------------------------
        // CODINT = la coulonne ou j'ai enregistre les CIN
        public void insertIntoUti()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry_uti = "INSERT INTO UTI (ID_CODUTI, ID_CODPRO, ID_CODLAN, ST_MOTPAS, ID_CODINT, ST_MAIL) VALUES (@ID_CODUTI, @ID_CODPRO, @ID_CODLAN, @ST_MOTPAS, @ID_CODINT, @ST_MAIL)";
            SqlCommand cmd_uti = new SqlCommand(qry_uti, con);
            cmd_uti.Parameters.AddWithValue("@ID_CODINT", codeInt.Text.ToUpper());
            cmd_uti.Parameters.AddWithValue("@ID_CODUTI", idUser.Text.ToUpper());
            cmd_uti.Parameters.AddWithValue("@ID_CODPRO", desPro.SelectedValue.ToString());
            cmd_uti.Parameters.AddWithValue("@ID_CODLAN", "FRA");
            cmd_uti.Parameters.AddWithValue("@ST_MOTPAS", mdpUser.Text);
            cmd_uti.Parameters.AddWithValue("@ST_MAIL", mailUser.Text);
            cmd_uti.ExecuteNonQuery();
            con.Close();
            MsgResult("Vous avez ajouté " + nomUser.Text + " " + prenomUser.Text + " comme étant un " + desPro.SelectedItem.ToString().ToLower(), "information");
        }
        //Verifer s'il existe un champs vide ou pas
        public Boolean textboxesAreEmpty()
        {
            if (codeInt.Text == String.Empty || idUser.Text == String.Empty || mdpUser.Text == String.Empty || mailUser.Text == String.Empty || Tel.Text == String.Empty || nomUser.Text == String.Empty || prenomUser.Text == String.Empty)
            {
                return true;
            }
            return false;
        }
        //Verifer si le IDutilisateur et le code int saisies existent deja dans la BD
        public Boolean idUserExisted()
        {
            string qry = "SELECT ID_CODUTI FROM UTI WHERE ID_CODUTI LIKE '" + idUser.Text + "'";
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
        public Boolean cinExist()
        {
            string qry = "SELECT ID_CODINT FROM INTER WHERE ID_CODINT LIKE '" + codeInt.Text + "'";
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

        //------------------ inserer le nouveau user dans la table INTER----------------------------
        public void insertIntoInter()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry_inter = "INSERT INTO INTER (ID_CODINT, ST_NOM, ST_PRE, ST_TELMOB, ID_CODUTI, ST_MAIL) values (@ID_CODINT, @ST_NOM, @ST_PRE, @ST_TELMOB, @ID_CODUTI, @ST_MAIL)";
            SqlCommand cmd_inter = new SqlCommand(qry_inter, con);
            cmd_inter.Parameters.AddWithValue("@ID_CODUTI", idUser.Text.ToUpper());
            cmd_inter.Parameters.AddWithValue("@ID_CODINT", codeInt.Text.ToUpper());
            cmd_inter.Parameters.AddWithValue("@ST_NOM", nomUser.Text.ToUpper());
            cmd_inter.Parameters.AddWithValue("@ST_PRE", prenomUser.Text.ToUpper());
            cmd_inter.Parameters.AddWithValue("@ST_TELMOB", Tel.Text);
            cmd_inter.Parameters.AddWithValue("@ST_MAIL", mailUser.Text);
            cmd_inter.ExecuteNonQuery();
            con.Close();
            MsgResult("Vous avez bien ajoute " + idUser.Text + " a la table INTER ", "information");
        }
        //------------------ inserer L'ACTION D' INSERTION DU le nouveau user dans la table EXTTRACE----------------------------
        public void insertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a ajouté l'utilisateur " + nomUser.Text + " " + prenomUser.Text;
            string qry = "INSERT INTO EXTTRACE ( NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) values (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", trace);
            cmd.Parameters.AddWithValue("@ADR_MAC", GetMacAddress());
            cmd.ExecuteNonQuery();
            con.Close();
        }
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
        public void viderTextboxes()
        {
            idUser.Text = String.Empty;
            mdpUser.Text = String.Empty;
            nomUser.Text = String.Empty;
            prenomUser.Text = String.Empty;
            mailUser.Text = String.Empty;
            codeInt.Text = String.Empty;
            Tel.Text = String.Empty;
            chantierInfo.Visible = false;
        }

        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            viderTextboxes();
        }
        // afficher alerte 
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
        protected void bindNotifRepeater()
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
    }
}