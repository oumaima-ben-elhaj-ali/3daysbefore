using System;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace PFE.Admin.Visiteur
{
    public partial class DemanderEq : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CodcouEQ.Text = Request.QueryString["ST_CODCOU"];
                dtDebut.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dtFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
                mydiv.Visible = false;
            }
        }
        protected void BTNenvoyer_Click(object sender, EventArgs e)
        {
            DateTime DE = Convert.ToDateTime(dtDebut);
            DateTime DS = Convert.ToDateTime(dtFin);
            if (TextboxesAreEmpty())
            {
                mydiv.Visible = true;
            }
            else if (DateTime.Compare(DE, DS) >= 0)
            {
                errormsg.Text = "La date début doit être antérieure à la date fin";
                mydiv.Visible = true;
            }
            else
            {
                string b = chantier.Text;
                string c = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
                string d = dtDebut.Text;
                string ee = dtFin.Text;
                string qry = "INSERT INTO EXTDEMLOC(ST_CODEQ, ST_DESCH, DT_CREATE, DT_DEBUT, DT_FIN, ST_NOM, ST_PRENOM, NU_TEL, NU_CIN, ST_MAIL) VALUES (@ST_CODEQ, @ST_DESCH, @DT_CREATE, @DT_DEBUT, @DT_FIN, @ST_NOM, @ST_PRENOM, @NU_TEL, @NU_CIN, @ST_MAIL)";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@ST_CODEQ", CodcouEQ.Text);
                cmd.Parameters.AddWithValue("@ST_DESCH", chantier.Text);
                cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@DT_DEBUT", dtDebut.Text);
                cmd.Parameters.AddWithValue("@DT_FIN", dtFin.Text);
                cmd.Parameters.AddWithValue("@ST_NOM", nom.Text);
                cmd.Parameters.AddWithValue("@ST_PRENOM", prenom.Text);
                cmd.Parameters.AddWithValue("@NU_TEL", Tel.Text);
                cmd.Parameters.AddWithValue("@NU_CIN", cin.Text);
                cmd.Parameters.AddWithValue("@ST_MAIL", mail.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                insertIntoTrace();
                MsgResult("Votre demande est envoyée", "information");
                EmptyTexboxes();
            }

        }
        protected void BTNannuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsulterEq.aspx");
        }
        public void insertIntoTrace()
        {
            string trace = "Le visiteur " + nom.Text + " " + prenom.Text + " demande la location de l'équipement " + CodcouEQ.Text;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = " INSERT INTO EXTTRACE (NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) VALUES (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", "V." + nom.Text + " " + prenom.Text);
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", trace);
            cmd.Parameters.AddWithValue("@ADR_MAC", GetMacAddress());
            cmd.ExecuteNonQuery();
            con.Close();
        }
        // afficher une alerte
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
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
        // verifer si tout les chaps sont remplis
        public Boolean TextboxesAreEmpty()
        {
            if (CodcouEQ.Text == String.Empty || nom.Text == String.Empty || prenom.Text == String.Empty || mail.Text == String.Empty || Tel.Text == String.Empty || chantier.Text == String.Empty || cin.Text == String.Empty)
            {
                return true;
            }
            else return false;
        }
        // vider tout lrs textboxes
        public void EmptyTexboxes()
        {
            CodcouEQ.Text = String.Empty;
            nom.Text = String.Empty;
            prenom.Text = String.Empty;
            mail.Text = String.Empty;
            Tel.Text = String.Empty;
            chantier.Text = String.Empty;
            cin.Text = String.Empty;
            dtDebut.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}