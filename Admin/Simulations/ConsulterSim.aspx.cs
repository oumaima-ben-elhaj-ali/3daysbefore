using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace PFE.Simulations
{
    public partial class ConsulterSim : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mydiv.Visible = false;
                BindRepeater();
                Bind_month_ddl();
                Bind_year_ddl();
                Bind_ddl_chantier();
            }

        }
        // selectionner et afficher les simulation correspondantes au donnees selectionnees par l'utilisateur
        private void listeSimulations()
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
                    string sql = "SELECT ST_DESCH, ID_SIM , DT_CREATE, NU_VALEUR FROM EXTSIMULATION WHERE (MONTH(DT_CREATE) = @MOIS) AND (YEAR(DT_CREATE) = @YEAR) ORDER BY ST_DESCH ASC";
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@MOIS", m);
                    cmd.Parameters.AddWithValue("@YEAR", Int32.Parse(ddlyear.SelectedValue));
                    if (ddlChantier.SelectedItem.Text != "Tous les chantiers")
                    {
                        cmd.Parameters.AddWithValue("@ST_DESCH", ddlChantier.SelectedValue);
                    }
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvSimulation.DataSource = dt;
                        gvSimulation.DataBind();
                        counted = gvSimulation.Rows.Count;
                        if (counted == 0)
                        {
                            mydiv.Visible = true;
                        }
                        else mydiv.Visible = false;
                    }
                }
                else if (ddlChantier.SelectedItem.Text != "Tous les chantiers")
                {
                    SqlCommand cmd = new SqlCommand();
                    string sql = "SELECT ST_DESCH, ID_SIM , DT_CREATE, NU_VALEUR FROM EXTSIMULATION WHERE (MONTH(DT_CREATE) = @MOIS) AND (YEAR(DT_CREATE) = @YEAR) AND (ST_DESCH LIKE @ST_DESCH)";
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
                            mydiv.Visible = true;
                        }
                        else mydiv.Visible = false;
                    }
                }
                con.Close();
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
        /* Pour l'icon de notification dans la navbar a droite
         * on definit sa valeur par le nombre des ligne de la table EXTTRACE
         */
        public int NbreNotif()
        {
            int nbre = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EXTTRACE";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nbre = reader.GetInt32(0);
            }
            return nbre;
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

        public void Bind_ddl_chantier()
        {
            SqlConnection con = new SqlConnection(connectionString);
            string com = "SELECT ST_DESCH FROM EXTCHANTIER";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlChantier.Items.Clear();
            ddlChantier.DataSource = dt;
            ddlChantier.DataTextField = "ST_DESCH";
            ddlChantier.DataValueField = "ST_DESCH";
            ddlChantier.DataBind();
            ddlChantier.Items.Add("Tous les chantiers");
            ddlChantier.SelectedValue = "Tous les chantiers";

        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvSimulation.PageIndex = e.NewPageIndex;
            listeSimulations();
        }

        protected void Consulter_Click(object sender, EventArgs e)
        {
            listeSimulations();
        }
    }
}