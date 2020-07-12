using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace PFE.Directeur.Factures
{
    public partial class ConsulterFact : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
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
                query.Text = "SELECT * FROM EXTFACTURES WHERE (MONTH(DT_CREATE) = @MOIS) AND (YEAR(DT_CREATE) = @YEAR) AND (ID_DIR LIKE @ID_DIR)";
                Bind_month_ddl();
                Bind_year_ddl();
            }

        }
        // recuperer et lister les factures correspondant au donnees saisie par le directeur
        private void ListeFacture()
        {
            int counted = 0;
            int m = ddlmonth.SelectedIndex;
            m++;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT * FROM EXTFACTURES WHERE (MONTH(DT_CREATE) = @MOIS) AND (YEAR(DT_CREATE) = @YEAR) AND ID_DIR LIKE '" + Session["UserName"] + "'";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MOIS", m);
                    cmd.Parameters.AddWithValue("@YEAR", Int32.Parse(ddlyear.SelectedValue));

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvFacture.DataSource = dt;
                        gvFacture.DataBind();
                        counted = gvFacture.Rows.Count;
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
        // binding the notification repeater
        protected void BindRepeater(string query, DataTable dt)
        {
            SqlConnection con = new SqlConnection(connectionString);
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

        private void Bind_year_ddl()
        {
            int year = (System.DateTime.Now.Year);
            for (int intCount = year; intCount >= 1980; intCount--)
            {
                ddlyear.Items.Add(intCount.ToString());
            }
        }

        private void Bind_month_ddl()
        {
            for (int i = 1; i <= 12; i++)
            {
                ddlmonth.Items.Add(new System.Web.UI.WebControls.ListItem(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), i.ToString()));
            }
            ddlmonth.SelectedValue = DateTime.Now.Month.ToString();
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvFacture.PageIndex = e.NewPageIndex;
            ListeFacture();
        }

        protected void Consulter_Click(object sender, EventArgs e)
        {
            ListeFacture();
        }

    }
}