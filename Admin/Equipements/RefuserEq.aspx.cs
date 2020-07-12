using System;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace PFE.Admin.Equipements
{
    public partial class RefuserEq : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID_DEMANDE"] != "")
            {
                SetDemandeSTATUS(Convert.ToInt32(Request.QueryString["ID_DEMANDE"]));
                Response.Redirect("DemandeEq.aspx");
            }

        }
        /* Apres recevoir une demande de location d'un directeur de chantier
         si l'adiministrateur refuse cette demande on passe l ID de la demande acette page
         ici on definit la colonne ST_STATUS correspond a l'ID recu en REFUSED
         */
        public void SetDemandeSTATUS(int iddemande)
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qryMVT = " UPDATE EXTDEMLOC SET ST_STATUS= 'REFUSED', DT_TRAIT=@DT_TRAIT WHERE ID_DEMANDE=" + iddemande;
            SqlCommand cmd = new SqlCommand(qryMVT, con);
            cmd.Parameters.AddWithValue("@DT_TRAIT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void InsertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a réfusée la demande de location de " + Request.QueryString["ST_CODCOU"];
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