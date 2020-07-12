using System;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Web.UI;

namespace PFE.Equipements
{
    public partial class SupprimerEq : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlConnection con = new SqlConnection(connetionString);
                con.Open();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showPopup", "if (!confirm('êtes-vous sûr?')) return false;", true);
                string qry = "DELETE FROM EQU WHERE ST_CODCOU LIKE '" + Request.QueryString["ST_CODCOU"] + "'";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
                MsgResult("Vous avez supprimé l équipement " + Request.QueryString["ST_CODCOU"] + " !!", "Information");
                InsertIntoTrace();
                con.Close();
                Response.Redirect("ConsulterEq.aspx");
            }
            else { }
        }
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }


        // inserer l'action de suppression dans EXTTRACE
        public void InsertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a supprimé l'équipement " + Request.QueryString["ST_CODCOU"];
            string qry = "INSERT INTO EXTTRACE ( NU_CODUTI, DT_CREATE, ST_OBSTRA, ADR_MAC) values (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA, @ADR_MAC)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
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

    }
}