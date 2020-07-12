using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace PFE.Directeur.Utilisateurs
{
    public partial class ModifProfil : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query_notif = "SELECT TOP(4) * FROM " +
                                "(SELECT CONCAT('votre simultaion de ', ST_DESCH, ' est prête') ST_OBSTRA , CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT 'Votre facture est prête' AS ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"].ToString() + "') AS EXTNOTIF" +
                                " ORDER BY DT_CREATE DESC";
                bindNotifsRepeater(query_notif, dt);
                remplirChamps();
            }

        }
        // binding notif repeater
        protected void bindNotifsRepeater(string query, DataTable dt)
        {
            SqlConnection con = new SqlConnection(connetionString);
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
        // remplir les champs par les donnees correspondant a l CODUTI recue
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
                desPro.Text = reader["ST_DES"].ToString();
            }
            con.Close();
        }
        // Valider les modification
        // updating table UTI and INTER
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
            string qry = "UPDATE UTI SET ST_MAIL=@ST_MAIL, ST_MOTPAS = @ST_MOTPAS WHERE ID_CODUTI LIKE @ID_CODUTI";
            con.Open();
            SqlCommand cmd_uti = new SqlCommand(qry, con);
            cmd_uti.Parameters.AddWithValue("@ST_MAIL", mailUser.Text);
            cmd_uti.Parameters.AddWithValue("@ID_CODUTI", idUser.Text.ToUpper());
            cmd_uti.Parameters.AddWithValue("@ST_MOTPAS", mdpUser.Text);
            cmd_uti.ExecuteNonQuery();
            con.Close();
        }
        // afficher une alerte
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProfilUti.aspx");
        }
        // inserer l'action de modification
        public void insertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a modifié son profil";
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