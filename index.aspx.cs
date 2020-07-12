using System;
using System.Data.SqlClient; //this namespace is for sqlclient server  
using System.Net.NetworkInformation;

namespace PFE
{
    public partial class ControleLogin : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Connexion_Click(object sender, EventArgs e)
        {
            string dateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string qry = "";
            SqlCommand cmd;
            SqlDataReader sdr;
            try
            {
                SqlConnection con = new SqlConnection(connetionString);
                con.Open();
                if(codUti.Text== String.Empty || password.Text == String.Empty)
                {
                    LoginMsg.Visible = true;
                    LoginMsg.Text = "Vous devez remplir les deux champs!!";
                }
                else { 
                    qry = "select * from UTI where ID_CODUTI LIKE '" + codUti.Text + "' and ST_MOTPAS LIKE '" + password.Text + "'";
                    cmd = new SqlCommand(qry, con);
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        LoginMsg.Visible = true;
                        LoginMsg.Text = "Connexion réussite......!!";
                        Session["UserName"] = codUti.Text;
                        Session["passWord"] = password.Text;
                        con.Close();
                        string qrySession = "INSERT INTO EXTSESSION (NU_CODUTI, DT_LOGIN, ADR_MAC) VALUES (@NU_CODUTI, @DT_LOGIN, @ADR_MAC)";
                        con.Open();
                        SqlCommand cmdSession = new SqlCommand(qrySession, con);
                        cmdSession.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
                        cmdSession.Parameters.AddWithValue("@DT_LOGIN", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                        cmdSession.Parameters.AddWithValue("@ADR_MAC", GetMacAddress());
                        cmdSession.ExecuteNonQuery();
                        con.Close();
                        string cod = GetCODPRO();
                        if (cod == "DIR")
                        {
                            Response.Redirect("Directeur/Dashboard.aspx");
                        }
                        else if (cod == "AD")
                        {
                            Response.Redirect("Admin/Dashboard.aspx");
                        }
                        else 
                            {
                            LoginMsg.Visible = true;
                            LoginMsg.Text = "Vous n'avez pas l’accès à l'application!";
                        }
                    }
                    else
                    {
                        LoginMsg.Visible = true;
                        LoginMsg.Text = "Adresse E-mail ou mot de passe inavlide..!";
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        public string GetCODPRO()
        {
            string code = "";
            string qry = " SELECT ID_CODPRO FROM UTI WHERE ID_CODUTI LIKE '" + codUti.Text + "'";
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                code = reader.GetString(0);
                con.Close();
            }
            return code;

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