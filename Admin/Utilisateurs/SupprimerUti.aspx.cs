using System;
using System.Data.SqlClient;

namespace PFE.Utilisateurs
{
    public partial class SupprimerUser : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InsertIntoTrace();
                SqlConnection con = new SqlConnection(connetionString);
                con.Open();
                string qry = "DELETE FROM UTI WHERE ID_CODUTI LIKE '" + Request.QueryString["ID_CODUTI"] + "'";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
                string qryInter = "DELETE FROM INTER WHERE ID_CODUTI LIKE '" + Request.QueryString["ID_CODUTI"] + "'";
                SqlCommand cmdInter = new SqlCommand(qryInter, con);
                cmdInter.ExecuteNonQuery();
                MsgResult("Vous avez supprimé l utilisateur " + Request.QueryString["ID_CODUTI"] + " !!", "Information");
                con.Close();
                Response.Redirect("ConsulterUti.aspx");
            }
            else { }

        }

        public void InsertIntoTrace()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string trace = Session["UserName"].ToString() + " a supprimé l'utilisateur ayant ID_CODUTI= " + Request.QueryString["ID_CODUTI"];
            string qry = "INSERT INTO EXTTRACE ( NU_CODUTI, DT_CREATE, ST_OBSTRA) values (@NU_CODUTI, @DT_CREATE, @ST_OBSTRA)";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@NU_CODUTI", Session["UserName"].ToString());
            cmd.Parameters.AddWithValue("@DT_CREATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@ST_OBSTRA", trace);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
    }
}