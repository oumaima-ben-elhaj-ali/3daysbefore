using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace PFE.Equipements
{
    public partial class AllDepEq : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                query.Text = "SELECT * FROM EXTDEPEQU ORDER BY ST_CODEQ ASC, DT_ENTREE DESC";
                BindRepeater();
                Bind_month_ddl();
                Bind_year_ddl();
                Bind_ddl_chantier();

            }
        }

        // PopulateGridview: sert a remplir en 1er lieu le gridView givAllDepEq par tout les deplacements existants correspondant au donnees selectionnees
        private void ListeDeplacements()
        {
            int counted = 0;
            int m = ddlmonth.SelectedIndex;
            m++;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                if (ddlChantier.SelectedItem.Text == "Tous les chantiers")
                {
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * FROM EXTDEPEQU WHERE (MONTH(DT_ENTREE) = @MOIS) AND (YEAR(DT_ENTREE) = @YEAR) ORDER BY ST_DESCH";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MOIS", m);
                    cmd.Parameters.AddWithValue("@YEAR", Int32.Parse(ddlyear.SelectedValue));
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvSimulation.DataSource = dt;
                        gvSimulation.DataBind();
                        counted = gvSimulation.Rows.Count;
                        if (counted == 0)
                        {
                            mydiv.Visible = true;// mydiv contient l'info que n'existe aucun deplacement
                        }
                        else mydiv.Visible = false;
                    }
                }
                if (ddlChantier.SelectedItem.Text != "Tous les chantiers")
                {
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT * FROM EXTDEPEQU WHERE (MONTH(DT_ENTREE) = @MOIS) AND (YEAR(DT_ENTREE) = @YEAR) AND (ST_DESCH LIKE @ST_DESCH) ORDER BY DT_ENTREE DESC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MOIS", m);
                    cmd.Parameters.AddWithValue("@YEAR", Int32.Parse(ddlyear.SelectedValue));
                    cmd.Parameters.AddWithValue("@ST_DESCH", ddlChantier.SelectedValue);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvSimulation.DataSource = dt;
                        gvSimulation.DataBind();
                        counted = gvSimulation.Rows.Count;
                        if (counted == 0)
                        {
                            mydiv.Visible = true;// mydiv contient l'info que n'existe aucun deplacement
                        }
                        else mydiv.Visible = false;
                    }
                }
            }
        }
        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
        protected void BindRepeater()
        {
            SqlConnection con = new SqlConnection(connectionString);
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
        private void Bind_year_ddl()
        {
            int year = (System.DateTime.Now.Year);
            for (int intCount = year; intCount >= 1980; intCount--)
            {
                ddlyear.Items.Add(intCount.ToString());
            }

            ddlyear.SelectedValue = DateTime.Now.Year.ToString();
        }

        private void Bind_month_ddl()
        {
            for (int i = 1; i <= 12; i++)
            {
                ddlmonth.Items.Add(new System.Web.UI.WebControls.ListItem(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), i.ToString()));
            }

            ddlmonth.SelectedValue = DateTime.Now.Month.ToString();
        }

        public void Bind_ddl_chantier()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string com = "SELECT ST_DESCH FROM EXTCHANTIER WHERE ST_DESCH NOT LIKE 'PARC'";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlChantier.Items.Clear();
            ddlChantier.DataSource = dt;
            ddlChantier.DataTextField = "ST_DESCH";
            ddlChantier.DataValueField = "ST_DESCH";
            ddlChantier.DataBind();
            ddlChantier.Items.Add("Tous les chantiers");
            con.Close();
            ddlChantier.SelectedValue = "Tous les chantiers";

        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvSimulation.PageIndex = e.NewPageIndex;
            ListeDeplacements();
        }

        protected void Consulter_Click(object sender, EventArgs e)
        {
            ListeDeplacements();
        }
    }
}