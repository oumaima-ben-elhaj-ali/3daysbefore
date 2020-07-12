using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Admin.Visiteur
{
    public partial class ConsulterEq : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SearchEquipements();
            }
        }
        // selectionner les equipements de la table EQU et les afficher dans le gridaView
        private void SearchEquipements()
        {

            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ST_CODCOU, ST_DESEQU, ID_CODIMP, NU_PRIACH, NU_PRIACT FROM EQU";
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        sql += " WHERE ST_DESEQU LIKE @ST_DESEQU + '%'";
                        cmd.Parameters.AddWithValue("@ST_DESEQU", txtSearch.Text);
                    }
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvEquipement.DataSource = dt;
                        gvEquipement.DataBind();
                    }
                }
                con.Close();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvEquipement.PageIndex = e.NewPageIndex;
            this.SearchEquipements();
        }
        protected void Search(object sender, EventArgs e)
        {
            this.SearchEquipements();
        }
    }
}