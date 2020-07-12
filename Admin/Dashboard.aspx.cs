using System;
using System.Data;
using System.Data.SqlClient;

namespace PFE
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string connetionString = "Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        protected void Page_Load(object sender, EventArgs e)
        {
            bindNotifRepeater();
            int n = nombreNotif();
            nbrNotif.Text = n.ToString();
            nombreUti();
            nbrDep();
            revenuesMensuelles();
            nbrEq();
            nbrDemandes();
            nbrFactures();
            nbrReclamations();
            nbrSimulation();
        }
        protected void bindNotifRepeater()
        {
            SqlConnection con = new SqlConnection(connetionString);
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
        public int nombreNotif()
        {
            int nbre = 0;
            SqlConnection con = new SqlConnection(connetionString);
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
        public void revenuesMensuelles()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT SUM (NU_VALEUR) as valeur  FROM EXTSIMULATION WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate())";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["valeur"] == null)
                {
                    countSim.Text = "0";
                }
                else countSim.Text = reader["valeur"].ToString();
            }
            else countSim.Text = "0";
            con.Close();
        }

        public void nbrDemandes()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EXTDEMLOC WHERE ST_STATUS IS NULL";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                demandes.Text = reader.GetInt32(0).ToString();
            }
            else demandes.Text = "0";
            con.Close();
        }
        public void nbrSimulation()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) AS VALUE FROM EXTSIMULATION WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate())";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                nbSim.Text = reader.GetInt32(0).ToString();
            }
            else nbSim.Text = "0";
            con.Close();
        }

        public void nbrFactures()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) AS VALUE FROM EXTFACTURES WHERE Month(DT_CREATE) = Month(getdate()) AND Year(DT_CREATE) = Year(getdate())";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                nbFactures.Text = reader.GetInt32(0).ToString();
            }
            else nbFactures.Text = "0";
            con.Close();
        }
        public void nbrReclamations()
        {
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) AS VALUE FROM EXTRECLAM WHERE EST_TRAITEE IS NULL";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reclamations.Text = reader.GetInt32(0).ToString();
            }
            else reclamations.Text = "0";
            con.Close();
        }
        public void nombreUti()
        {
            int val = 0;
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM UTI";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetInt32(0);
            }
            countUser.Text = val.ToString();
            con.Close();
        }
        public void nbrEq()
        {
            int val = 0;
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EQU";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetInt32(0);
            }
            countEq.Text = val.ToString();
            con.Close();
        }
        public void nbrDep()
        {
            int val = 0;
            SqlConnection con = new SqlConnection(connetionString);
            con.Open();
            string qry = "SELECT COUNT(*) FROM EXTDEPEQU WHERE Month(DT_ENTREE) = Month(getdate()) AND Year(DT_ENTREE) = Year(getdate())";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetInt32(0);
            }
            countDep.Text = val.ToString();
            con.Close();
        }

    }
}