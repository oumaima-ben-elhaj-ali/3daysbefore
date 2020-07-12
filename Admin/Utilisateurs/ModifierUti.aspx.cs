using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace PFE.Utilisateurs
{
    public partial class ModifierUser : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                desProBinding();
                bindNotifRepeater();
                remplirChamps();
                nbrNotif.Text = nombreNotifs().ToString();

            }
            else { }

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
        // remplir les champs intitialement par les donnees de l'equipement voulu etre modifié
        public void remplirChamps()
        {
            string codeuti = Request.QueryString["ID_CODUTI"];
            SqlConnection con = new SqlConnection(connetionString);
            string qry = "SELECT UTI.ID_CODUTI, PRO.ST_DES, UTI.ST_MOTPAS, UTI.ST_MAIL, UTI.ID_CODINT, INTER.ST_NOM, INTER.ST_PRE, INTER.ST_TELMOB FROM UTI LEFT OUTER JOIN PRO ON UTI.ID_CODPRO = PRO.ID_CODPRO LEFT OUTER JOIN INTER ON UTI.ID_CODINT = INTER.ID_CODINT WHERE UTI.ID_CODUTI LIKE '" + codeuti + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idUser.Text = reader["ID_CODUTI"].ToString();
                nomUser.Text = reader["ST_NOM"].ToString();
                prenomUser.Text = reader["ST_PRE"].ToString();
                mailUser.Text = reader["ST_MAIL"].ToString();
                Tel.Text = reader["ST_TELMOB"].ToString();
                mdpUser.Text = reader["ST_MOTPAS"].ToString();
                codeInt.Text = reader["ID_CODINT"].ToString();
                desPro.SelectedItem.Text = reader["ST_DES"].ToString();
            }
            con.Close();
        }
        protected void BTNvaliderModif_Click(object sender, EventArgs e)
        {
            updateUti();
            updateInter();
            insertIntoTrace();
            MsgResult("Modification validée!!", "Information");
            mdpUser.ReadOnly = true;
            nomUser.ReadOnly = true;
            prenomUser.ReadOnly = true;
            mailUser.ReadOnly = true;
            Tel.ReadOnly = true;
            desPro.Enabled = false;
        }
        public void updateInter()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry_inter = "UPDATE INTER SET ST_NOM = @ST_NOM , ST_PRE = @ST_PRE, ST_MAIL = @ST_MAIL, ST_TELMOB = @ST_TELMOB WHERE ID_CODUTI LIKE @ID_CODUTI";
            SqlCommand cmd = new SqlCommand(qry_inter, con);
            cmd.Parameters.AddWithValue("@ID_CODUTI", idUser.Text.ToUpper());
            cmd.Parameters.AddWithValue("@ST_NOM", nomUser.Text.ToUpper());
            cmd.Parameters.AddWithValue("@ST_PRE", prenomUser.Text.ToUpper());
            cmd.Parameters.AddWithValue("@ST_MAIL", mailUser.Text);
            cmd.Parameters.AddWithValue("@ST_TELMOB", Tel.Text);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void updateUti()
        {
            SqlConnection con = new SqlConnection(connetionString);
            string qry = "UPDATE UTI SET ST_MAIL=@ST_MAIL, ST_MOTPAS = @ST_MOTPAS, ID_CODPRO = @ID_CODPRO WHERE ID_CODUTI LIKE @ID_CODUTI";
            con.Open();
            SqlCommand cmd_uti = new SqlCommand(qry, con);
            cmd_uti.Parameters.AddWithValue("@ST_MAIL", mailUser.Text);
            cmd_uti.Parameters.AddWithValue("@ID_CODUTI", idUser.Text.ToUpper());
            cmd_uti.Parameters.AddWithValue("@ST_MOTPAS", mdpUser.Text);
            cmd_uti.Parameters.AddWithValue("@ID_CODPRO", desPro.SelectedValue.ToString());
            cmd_uti.ExecuteNonQuery();
            con.Close();
        }

        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsulterUti.aspx");
        }
        // inserer l'action de modification
        public void insertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a modifié l'utilisateur " + nomUser.Text + " " + prenomUser.Text;
            string qry = "INSERT INTO EXTTRACE ( NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) values (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", trace);
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