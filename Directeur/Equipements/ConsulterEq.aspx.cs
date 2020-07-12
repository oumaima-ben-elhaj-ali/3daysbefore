using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PFE.Directeur.Equipements
{
    public partial class ConsulterEq : System.Web.UI.Page
    {
        string connetionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mydiv.Visible = false;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                string query_notif = "SELECT TOP(4) * FROM " +
                                "(SELECT CONCAT('votre simultaion de ', ST_DESCH, ' est prête') ST_OBSTRA , CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE from EXTSIMULATION where ID_DIR like '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Vous avez demander la location de ',ST_CODEQ) ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est acceptée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'ACCEPTED' UNION " +
                                "SELECT CONCAT('Votre demande de location de ', ST_CODEQ, ' est refusée') ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTDEMLOC WHERE ID_CODUTI LIKE '" + Session["UserName"].ToString() + "' AND ST_STATUS LIKE 'REFUSED' UNION " +
                                "SELECT 'Votre facture est prête' AS ST_OBSTRA, CONVERT(VARCHAR, DT_CREATE) AS DT_CREATE FROM EXTFACTURES WHERE ID_DIR LIKE '" + Session["UserName"].ToString() + "') AS EXTNOTIF" +
                                " ORDER BY DT_CREATE DESC";
                BindRepeater(query_notif, dt);
                SearchEquipements();
            }
        }
        // selectionner les equipements de la table EQU et les afficher dans le gridView
        private void SearchEquipements()
        {
            int counted = 0;
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT ST_CODCOU, ST_DESEQU, ID_CODIMP, CAST(NU_PRIACH AS DECIMAL(16,3)) AS NU_PRIACH ,CAST(NU_PRIACT AS DECIMAL(16,3)) as NU_PRIACT FROM EQU WHERE NU_PRIACT >0 AND NU_PRIACH>0";
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        sql += " AND ST_DESEQU LIKE '%" + txtSearch.Text + "%' or ST_CODCOU LIKE '%" + txtSearch.Text + "%' OR ID_CODIMP LIKE '%" + txtSearch.Text + "%'";
                    }
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvEquipement.DataSource = dt;
                        gvEquipement.DataBind();
                        counted = gvEquipement.Rows.Count;
                        if (counted == 0)
                        {
                            mydiv.Visible = true;
                        }
                        else mydiv.Visible = false;
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
        // binding the notification repeater
        protected void BindRepeater(string query, DataTable dt)
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
    }
}