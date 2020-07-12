using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PFE.Simulations
{
    public partial class DetailsSim : System.Web.UI.Page
    {
        string connectionString = @"Data Source=LINK-PC;Initial Catalog=PFE;User ID=sa;Password=admin123";
        string xml_file = "E:\\pfe\\ProjetFinEtude\\Admin\\Simulations\\AdmSim\\simulation.xml";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindRepeater();
                if (simEstValide())
                {
                    gvSimDetails.Visible = false;
                    gvSimulationValide.Visible = true;
                    detailsSimulationValide();
                }
                else
                {
                    gvSimDetails.Visible = true;
                    gvSimulationValide.Visible = false;
                    detailsSimulationModifiable();
                }
            }
        }
        // verifier si la simulation est validee ou pas
        public Boolean simEstValide()
        {
            string qry = "SELECT SIM_STATE FROM EXTSIMULATION WHERE ID_SIM =" + Convert.ToInt32(Request.QueryString["ID_SIM"]);
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //si la simulation n'est pas encore valider alors l'admin peut effectuer qlq changements
                if (reader["SIM_STATE"].ToString() == "UNVALIDATED")
                {
                    return false;
                }
                // sinon il peut juste visualiser le detais de la simulation
                else
                {
                    return true;
                }
            }
            return false;
        }
        // binding the gridview gvSimulationValide par les donnees apartir de la table EXTDEPEQU
        private void detailsSimulationValide()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SELECT  ST_CODEQ, ST_DESEQ, ST_DESCH, CONVERT(varchar, DT_ENTREE, 1) AS DT_ENTREE, CONVERT(varchar, DT_SORTIE, 1) AS DT_SORTIE, NU_NBJRS, NU_TAUXLOC, NU_VALEUR FROM EXTDEPEQU WHERE ID_SIM=" + Convert.ToInt32(Request.QueryString["ID_SIM"]);
                    cmd.CommandText = sql;
                    cmd.Connection = con;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvSimulationValide.DataSource = dt;
                        gvSimulationValide.DataBind();
                    }
                }
                con.Close();
                SimTotal.Text = "TOTAL= " + calculValeurSim().ToString() + "TND";
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvSimulationValide.PageIndex = e.NewPageIndex;
            detailsSimulationValide();
        }
        //--------------- BINDING THE gvSimDetails with the multiple fuction CRUD------------------------
        void detailsSimulationModifiable()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string qry = "SELECT ID_DEP, ST_CODEQ, ST_DESEQ, ST_DESCH, CONVERT(varchar, DT_ENTREE, 1) AS DT_ENTREE, CONVERT(varchar, DT_SORTIE, 1) AS DT_SORTIE, NU_NBJRS, NU_TAUXLOC, NU_VALEUR FROM EXTDEPEQU WHERE ID_SIM =" + Convert.ToInt32(Request.QueryString["ID_SIM"]);
                SqlDataAdapter sqlDa = new SqlDataAdapter(qry, sqlCon);
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvSimDetails.DataSource = dtbl;
                gvSimDetails.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvSimDetails.DataSource = dtbl;
                gvSimDetails.DataBind();
                gvSimDetails.Rows[0].Cells.Clear();
                gvSimDetails.Rows[0].Cells.Add(new TableCell());
                gvSimDetails.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvSimDetails.Rows[0].Cells[0].Text = "Aucun enregistrement trouvé..!";
                gvSimDetails.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            SimTotal.Text = "TOTAL= " + calculValeurSim().ToString() + "TND";

        }
        // add a row
        protected void gvSimDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        DateTime DE = Convert.ToDateTime((gvSimDetails.FooterRow.FindControl("txtDT_ENTREEFooter") as TextBox).Text);
                        DateTime DS = Convert.ToDateTime((gvSimDetails.FooterRow.FindControl("txtDT_SORTIEFooter") as TextBox).Text);
                        int nbjrs = (DS - DE).Days;
                        Decimal taux = Convert.ToDecimal((gvSimDetails.FooterRow.FindControl("txtNU_TAUXLOCFooter") as TextBox).Text);
                        sqlCon.Open();
                        string query = "INSERT INTO EXTDEPEQU (ST_CODEQ, ST_DESEQ, ST_DESCH, DT_ENTREE, DT_SORTIE, NU_NBJRS, NU_TAUXLOC, NU_VALEUR, ID_SIM) VALUES (@ST_CODEQ,@ST_DESEQ,@ST_DESCH,@DT_ENTREE, @DT_SORTIE, @NU_NBJRS, @NU_TAUXLOC, @NU_VALEUR, @ID_SIM)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@ST_CODEQ", (gvSimDetails.FooterRow.FindControl("txtST_CODEQFooter") as TextBox).Text);
                        sqlCmd.Parameters.AddWithValue("@ST_DESEQ", (gvSimDetails.FooterRow.FindControl("txtST_DESEQFooter") as TextBox).Text);
                        sqlCmd.Parameters.AddWithValue("@ST_DESCH", (gvSimDetails.FooterRow.FindControl("txtST_DESCHFooter") as TextBox).Text);
                        sqlCmd.Parameters.AddWithValue("@DT_ENTREE", DE);
                        sqlCmd.Parameters.AddWithValue("@DT_SORTIE", DE);
                        sqlCmd.Parameters.AddWithValue("@NU_NBJRS", nbjrs);
                        sqlCmd.Parameters.AddWithValue("@NU_TAUXLOC", taux);
                        sqlCmd.Parameters.AddWithValue("@NU_VALEUR", taux * nbjrs);
                        sqlCmd.Parameters.AddWithValue("@ID_SIM", Convert.ToInt32(Request.QueryString["ID_SIM"]));
                        sqlCmd.ExecuteNonQuery();
                        detailsSimulationModifiable();
                        lblSuccessMessage.Text = "Nouveau enregistrement ajouté";
                        lblErrorMessage.Text = "";
                        SimTotal.Text = "TOTAL= " + calculValeurSim().ToString() + "TND";
                    }
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }
        // edit a row
        protected void gvSimDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSimDetails.EditIndex = e.NewEditIndex;
            detailsSimulationModifiable();
        }
        // cancel edinting
        protected void gvSimDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSimDetails.EditIndex = -1;
            detailsSimulationModifiable();
        }
        // update a row
        protected void gvSimDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DateTime DE = Convert.ToDateTime((gvSimDetails.Rows[e.RowIndex].FindControl("txtDT_ENTREE") as TextBox).Text);
                DateTime DS = Convert.ToDateTime((gvSimDetails.Rows[e.RowIndex].FindControl("txtDT_SORTIE") as TextBox).Text);
                int nbjrs = (DS - DE).Days;
                Decimal taux = Convert.ToDecimal((gvSimDetails.Rows[e.RowIndex].FindControl("txtNU_TAUXLOC") as TextBox).Text);
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "UPDATE EXTDEPEQU SET ST_CODEQ=@ST_CODEQ, ST_DESEQ=@ST_DESEQ, ST_DESCH=@ST_DESCH, DT_ENTREE=@DT_ENTREE, DT_SORTIE=@DT_SORTIE, NU_NBJRS=@NU_NBJRS, NU_TAUXLOC=@NU_TAUXLOC, NU_VALEUR=@NU_VALEUR WHERE ID_DEP = @ID_DEP";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@ST_CODEQ", (gvSimDetails.Rows[e.RowIndex].FindControl("txtST_CODEQ") as TextBox).Text);
                    sqlCmd.Parameters.AddWithValue("@ST_DESEQ", (gvSimDetails.Rows[e.RowIndex].FindControl("txtST_DESEQ") as TextBox).Text);
                    sqlCmd.Parameters.AddWithValue("@ST_DESCH", (gvSimDetails.Rows[e.RowIndex].FindControl("txtST_DESCH") as TextBox).Text);
                    sqlCmd.Parameters.AddWithValue("@DT_ENTREE", DE);
                    sqlCmd.Parameters.AddWithValue("@DT_SORTIE", DS);
                    sqlCmd.Parameters.AddWithValue("@NU_NBJRS", nbjrs);
                    sqlCmd.Parameters.AddWithValue("@NU_TAUXLOC", taux);
                    sqlCmd.Parameters.AddWithValue("@NU_VALEUR", nbjrs * taux);
                    sqlCmd.Parameters.AddWithValue("@ID_DEP", Convert.ToInt32(gvSimDetails.DataKeys[e.RowIndex].Value.ToString()));

                    sqlCmd.ExecuteNonQuery();
                    gvSimDetails.EditIndex = -1;
                    detailsSimulationModifiable();
                    lblSuccessMessage.Text = "L'enregistrement selectionné est modifié";
                    lblErrorMessage.Text = "";
                    SimTotal.Text = "TOTAL= " + calculValeurSim().ToString() + "TND";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }
        // delete a row
        protected void gvSimDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM EXTDEPEQU WHERE ID_DEP = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvSimDetails.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    detailsSimulationModifiable();
                    lblSuccessMessage.Text = "L'enregistrement selectionné est supprimé";
                    lblErrorMessage.Text = "";
                    SimTotal.Text = "TOTAL= " + calculValeurSim().ToString() + "TND";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }

        /* binding the notification repeater
         Selectionner les top(4) ligne de la table EXTTRACE  qui contient les actions des utilisateurs
         */
        protected void bindRepeater()
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
        public int nombreNotif()
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
        //*--------------------- Recalculation de la valeur de la simulation----------------

        public Decimal calculValeurSim()
        {
            Decimal val = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "SELECT SUM(NU_VALEUR) FROM EXTDEPEQU WHERE ID_SIM = @ID_SIM";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@ID_SIM", Convert.ToInt32(Request.QueryString["ID_SIM"]));
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                val = reader.GetDecimal(0);
            }
            setValeurSim(val);
            con.Close();
            return val;
        }
        public void setValeurSim(Decimal val)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string qry = "UPDATE EXTSIMULATION SET NU_VALEUR = @valeur WHERE ID_SIM = @idsim";
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@valeur", val);
            cmd.Parameters.AddWithValue("@idsim", Convert.ToInt32(Request.QueryString["ID_SIM"]));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // exporter la simulation en format excel
        protected void ExportExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvSimDetails.AllowPaging = false;
                this.detailsSimulationModifiable();
                gvSimDetails.FooterRow.Visible = false;

                gvSimDetails.HeaderRow.BackColor = Color.Teal;
                foreach (TableCell cell in gvSimDetails.HeaderRow.Cells)
                {
                    cell.BackColor = gvSimDetails.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvSimDetails.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = Color.WhiteSmoke;
                        }
                        else
                        {
                            cell.BackColor = Color.LightGray;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvSimDetails.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        // exporter la simulation en format PDF
        public void MsgResult(string myStringVariable, string alertkind)
        {
            ClientScript.RegisterStartupScript(this.GetType(), alertkind, "alert('" + myStringVariable + "');", true);
        }
        /*------------------------------------------------------------------------------------------------------------------------
         --------------------------------------------exporter la facture en PDF---------------------------------------------------*/
        BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //1- Mettre les details de la facture dans un fichier xml
        public void WriteInXML()
        {
            string qry = "SELECT EXTSIMULATION.ID_SIM, EXTSIMULATION.ST_DESCH, EXTSIMULATION.DT_CREATE, EXTSIMULATION.SIM_STATE, EXTSIMULATION.ID_DIR, EXTSIMULATION.NU_VALEUR, INTER.ST_NOM, INTER.ST_PRE, INTER.ID_CODINT FROM EXTSIMULATION INNER JOIN INTER ON EXTSIMULATION.ID_DIR = INTER.ID_CODUTI WHERE ID_SIM=" + Convert.ToInt32(Request.QueryString["ID_SIM"]);
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int row = 0;
                SqlDataReader details = GetSIMDetails();
                using (XmlWriter writer = XmlWriter.Create(xml_file))
                {
                    writer.WriteStartElement("Invoice");
                    writer.WriteStartElement("invoice_header");
                    writer.WriteElementString("invoiceNumber", reader["ID_SIM"].ToString());
                    writer.WriteElementString("invoiceIdDir", reader["ID_DIR"].ToString());
                    writer.WriteElementString("invoiceNameDir", reader["ST_NOM"].ToString() + " " + reader["ST_PRE"].ToString());
                    writer.WriteElementString("cin", reader["ID_CODINT"].ToString());
                    writer.WriteElementString("chantier", reader["ST_DESCH"].ToString());
                    writer.WriteElementString("invoiceDtCreate", reader["DT_CREATE"].ToString());
                    writer.WriteElementString("invoiceValue", reader["NU_VALEUR"].ToString());
                    writer.WriteElementString("Status", reader["SIM_STATE"].ToString());
                    writer.WriteEndElement();

                    while (details.Read())
                    {
                        writer.WriteStartElement("invoice_rows");
                        writer.WriteElementString("rowId", row.ToString());
                        writer.WriteElementString("desCh", details["ST_DESCH"].ToString());
                        writer.WriteElementString("codeEq", details["ST_CODEQ"].ToString());
                        writer.WriteElementString("desEq", details["ST_DESEQ"].ToString());
                        writer.WriteElementString("dtEntree", details["ENTREE"].ToString());
                        writer.WriteElementString("dtSortie", details["SORTIE"].ToString());
                        writer.WriteElementString("nbJrs", details["NU_NBJRS"].ToString());
                        writer.WriteElementString("tauxLoc", details["NU_TAUXLOC"].ToString());
                        writer.WriteElementString("valeur", details["ValeurDep"].ToString());
                        writer.WriteEndElement();
                        row++;
                    }
                    writer.WriteStartElement("invoice_companyinfo");
                    writer.WriteElementString("id", "BMI ");
                    writer.WriteElementString("nom", "Bureau de management industriel");
                    writer.WriteElementString("adresse", "Route de sousse Km 5.5");
                    writer.WriteElementString("adresseContinue", "Imm chérif 2ème étage");
                    writer.WriteElementString("tel", "(+216) 71 388 000");
                    writer.WriteElementString("fax", "(+216) 71 387 200");
                    writer.WriteElementString("email", "bmi@bmi.com.tn");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }

        }
        // les details de simulations : les locations
        public SqlDataReader GetSIMDetails()
        {
            string qry = "SELECT EXTSIMULATION.ID_SIM, EXTSIMULATION.ST_DESCH, EXTDEPEQU.NU_VALEUR AS ValeurDep, EXTDEPEQU.NU_TAUXLOC, EXTDEPEQU.NU_NBJRS, CONVERT(varchar, EXTDEPEQU.DT_ENTREE, 1) as ENTREE,CONVERT(varchar, EXTDEPEQU.DT_SORTIE, 1) as SORTIE, EXTDEPEQU.ST_DESEQ, EXTDEPEQU.ST_CODEQ, EXTDEPEQU.ID_DEP FROM EXTSIMULATION INNER JOIN INTER ON EXTSIMULATION.ID_DIR = INTER.ID_CODUTI INNER JOIN EXTDEPEQU ON EXTSIMULATION.ID_SIM = EXTDEPEQU.ID_SIM WHERE EXTSIMULATION.ID_SIM=" + Convert.ToInt32(Request.QueryString["ID_SIM"]) + " ORDER BY DT_ENTREE ASC";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        protected void ExportPDF_Click(object sender, EventArgs e)
        {
            string desEq = "";
            WriteInXML();
            try
            {
                invoice facture = new invoice(Server.MapPath("AdmSim") + "\\simulation.xml");
                DataRow drHead = facture.GetInvoiceHeader().Rows[0];
                DataRow drCompany = facture.GetInvoicePayeeInfo().Rows[0];
                using (System.IO.FileStream fs = new FileStream(Server.MapPath("AdmSim") + drHead["invoiceNumber"].ToString() + ".pdf", FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 25, 25, 30, 1);
                    PdfWriter writer = PdfWriter.GetInstance(document, fs);

                    // Add meta information to the document
                    document.AddAuthor("Oumaima Ben Elhaj Ali");
                    document.AddCreator("Application de facturation de location des engins");
                    document.AddKeywords("PDF facture directeurs");
                    document.AddSubject("realiser une facture");
                    document.AddTitle("Modele de la facture -   PDF version");

                    // Open the document to enable you to write to the document
                    document.Open();

                    // Makes it possible to add text to a specific place in the document using 
                    // a X & Y placement syntax.
                    PdfContentByte cb = writer.DirectContent;
                    // Add a footer template to the document
                    cb.AddTemplate(PdfFooter(cb, drCompany), 30, 1);

                    // Add a logo to the invoice
                    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance("E:\\pfe\\ProjetFinEtude\\images\\bmi.png");
                    png.ScaleAbsolute(140, 100);
                    png.SetAbsolutePosition(40, 725);
                    cb.AddImage(png);

                    // First we must activate writing
                    cb.BeginText();



                    // First we write out the header information

                    // Start with the invoice type header
                    writeText(cb, drCompany["id"].ToString(), 350, 820, f_cb, 14);
                    // HEader details; invoice number, invoice date, due date and customer Id
                    writeText(cb, "Société:", 350, 800, f_cb, 10);
                    writeText(cb, drCompany["nom"].ToString(), 420, 800, f_cn, 10);
                    writeText(cb, "Adresse:", 350, 788, f_cb, 10);
                    writeText(cb, drCompany["adresse"].ToString(), 420, 788, f_cn, 10);
                    writeText(cb, "", 350, 776, f_cb, 10);
                    writeText(cb, drCompany["adresseContinue"].ToString(), 420, 776, f_cn, 10);
                    writeText(cb, "Tél:", 350, 764, f_cb, 10);
                    writeText(cb, drCompany["tel"].ToString(), 420, 764, f_cn, 10);
                    writeText(cb, "Fax:", 350, 752, f_cb, 10);
                    writeText(cb, drCompany["fax"].ToString(), 420, 752, f_cn, 10);
                    writeText(cb, "Email:", 350, 740, f_cb, 10);
                    writeText(cb, drCompany["email"].ToString(), 420, 740, f_cn, 10);


                    // Delivery address details
                    int left_margin = 40;
                    int top_margin = 680;
                    writeText(cb, "Simulation", left_margin, top_margin, f_cb, 10);
                    writeText(cb, "Numéro: " + drHead["invoiceNumber"].ToString(), left_margin, top_margin - 12, f_cn, 10);
                    writeText(cb, "Date de Création" + drHead["invoiceDtCreate"].ToString(), left_margin, top_margin - 24, f_cn, 10);
                    writeText(cb, "Valeur totale :" + drHead["invoiceValue"].ToString(), left_margin, top_margin - 36, f_cn, 10);
                    writeText(cb, "Status :" + drHead["Status"].ToString(), left_margin, top_margin - 48, f_cn, 10);

                    // Invoice address
                    left_margin = 350;
                    writeText(cb, "Directeur", left_margin, top_margin, f_cb, 10);
                    writeText(cb, "Code directeur: " + drHead["invoiceIdDir"].ToString(), left_margin, top_margin - 12, f_cn, 10);
                    writeText(cb, "Nom et prénom: " + drHead["invoiceNameDir"].ToString(), left_margin, top_margin - 24, f_cn, 10);
                    writeText(cb, "CIN: " + drHead["cin"].ToString(), left_margin, top_margin - 36, f_cn, 10);
                    writeText(cb, "Chantier: " + drHead["chantier"].ToString(), left_margin, top_margin - 48, f_cn, 10);

                    // NOTE! You need to call the EndText() method before we can write graphics to the document!
                    cb.EndText();
                    // Separate the header from the rows with a line
                    // Draw a line by setting the line width and position
                    cb.SetLineWidth(0f);
                    cb.MoveTo(20, 570);
                    cb.LineTo(580, 570);
                    cb.Stroke();
                    // Don't forget to call the BeginText() method when done doing graphics!
                    cb.BeginText();

                    // Before we write the lines, it's good to assign a "last position to write"
                    // variable to validate against if we need to make a page break while outputting.
                    // Change it to 510 to write to test a page break; the fourth line on a new page
                    int lastwriteposition = 100;

                    // Loop thru the rows in the rows table
                    // Start by writing out the line headers
                    top_margin = 550;
                    left_margin = 40;
                    // Line headers
                    writeText(cb, "Code équipement", left_margin, top_margin, f_cb, 10);
                    writeText(cb, "Désignation", left_margin + 90, top_margin, f_cb, 10);
                    writeText(cb, "Dt.entrée", left_margin + 200, top_margin, f_cb, 10);
                    writeText(cb, "dt.sortie", left_margin + 270, top_margin, f_cb, 10);
                    writeText(cb, "Nb.jours", left_margin + 350, top_margin, f_cb, 10);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Taux", left_margin + 440, top_margin, 0);
                    writeText(cb, "Valeur", left_margin + 460, top_margin, f_cb, 10);
                    cb.EndText();
                    cb.SetLineWidth(0f);
                    cb.MoveTo(20, 545);
                    cb.LineTo(580, 545);
                    cb.Stroke();
                    cb.BeginText();
                    // First item line position starts here
                    top_margin = 528;

                    // Loop thru the table of items and set the linespacing to 12 points.
                    // Note that we use the -= operator, the coordinates goes from the bottom of the page!
                    foreach (DataRow drItem in facture.GetInvoiceRows().Rows)
                    {
                        int n = get3rdSpaceIndex(drItem["desEq"].ToString());
                        if (n != -1)
                        {
                            desEq = drItem["desEq"].ToString().Substring(0, n);
                        }
                        else
                        {
                            desEq = drItem["desEq"].ToString();
                        }
                        writeText(cb, drItem["codeEq"].ToString(), left_margin, top_margin, f_cb, 10);
                        writeText(cb, desEq, left_margin + 60, top_margin, f_cb, 10);
                        writeText(cb, drItem["dtEntree"].ToString(), left_margin + 200, top_margin, f_cb, 10);
                        writeText(cb, drItem["dtSortie"].ToString(), left_margin + 270, top_margin, f_cb, 10);
                        writeText(cb, drItem["nbJrs"].ToString(), left_margin + 350, top_margin, f_cb, 10);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, drItem["tauxLoc"].ToString(), left_margin + 440, top_margin, 0);
                        writeText(cb, drItem["valeur"].ToString(), left_margin + 460, top_margin, f_cb, 10);
                        // This is the line spacing, if you change the font size, you might want to change this as well.
                        top_margin -= 12;

                        // Implement a page break function, checking if the write position has reached the lastwriteposition
                        if (top_margin <= lastwriteposition)
                        {
                            // We need to end the writing before we change the page
                            cb.EndText();
                            // Make the page break
                            document.NewPage();
                            // Start the writing again
                            cb.BeginText();
                            // Assign the new write location on page two!
                            // Here you might want to implement a new header function for the new page
                            top_margin = 780;
                        }
                    }

                    // Okay, write out the totals table
                    // Here you might want to do some page break scenarios, as well:
                    // Example:
                    // Calculate how many rows you are about to print and see if they fit before the lastwriteposition, 
                    // then decide how to do; write some on first page, then the rest on second page or perhaps all the 
                    // total lines after the page break.
                    // We are not doing this here, we just write them out 80 points below the last writed item row

                    top_margin -= 80;
                    left_margin = 350;
                    float total = Convert.ToSingle(Convert.ToDecimal(calculValeurSim()));
                    float TVA = calculTvaTtc(total)[0];
                    float TTC = calculTvaTtc(total)[1];
                    // First the headers
                    writeText(cb, "Total de la simulation en chiffre :", 40, top_margin, f_cb, 10);
                    writeText(cb, converti(Convert.ToSingle(calculValeurSim())), 40, top_margin - 12, f_cn, 10);
                    writeText(cb, " Dinar tunisien", 250, top_margin - 24, f_cb, 10);
                    writeText(cb, "Total HT", left_margin, top_margin, f_cb, 10);
                    writeText(cb, "TVA", left_margin, top_margin - 12, f_cb, 10);
                    writeText(cb, "TTC", left_margin, top_margin - 48, f_cb, 10);
                    writeText(cb, "Signature:", 40, top_margin - 68, f_cb, 10);
                    cb.EndText();
                    cb.SetLineWidth(0f);
                    cb.MoveTo(30, top_margin - 80);
                    cb.LineTo(150, top_margin - 80);
                    cb.Stroke();
                    cb.BeginText();
                    cb.EndText();
                    cb.SetLineWidth(0f);
                    cb.MoveTo(30, top_margin - 150);
                    cb.LineTo(150, top_margin - 150);
                    cb.Stroke();
                    cb.BeginText();

                    // Move right to write out the values
                    left_margin = 540;
                    // Write out the invoice currency and values in regular text
                    cb.SetFontAndSize(f_cn, 10);
                    string curr = "DTN";
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, curr, left_margin, top_margin, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, curr, left_margin, top_margin - 12, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, curr, left_margin, top_margin - 48, 0);
                    left_margin = 535;
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, drHead["invoiceValue"].ToString(), left_margin, top_margin, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, TVA.ToString() + ",000", left_margin, top_margin - 12, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, TTC.ToString() + ",000", left_margin, top_margin - 48, 0);

                    // End the writing of text
                    cb.EndText();

                    // Close the document, the writer and the filestream!
                    document.Close();
                    writer.Close();
                    fs.Close();

                    MsgResult("la simulation est enregistrée dans le dossier AdmSim!", "information");
                }
            }
            catch (Exception rror)
            {
                MsgResult(rror.Message, "information");
            }
        }

        // This is the method writing text to the content byte
        private void writeText(PdfContentByte cb, string Text, int X, int Y, BaseFont font, int Size)
        {
            cb.SetFontAndSize(font, Size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, X, Y, 0);
        }


        private PdfTemplate PdfFooter(PdfContentByte cb, DataRow drFoot)
        {
            // Create the template and assign height
            PdfTemplate tmpFooter = cb.CreateTemplate(580, 80);
            // Move to the bottom left corner of the template
            tmpFooter.MoveTo(1, 1);
            // Place the footer content
            tmpFooter.Stroke();
            // Begin writing the footer
            tmpFooter.BeginText();
            // Set the font and size
            tmpFooter.SetFontAndSize(f_cn, 8);
            // Write out details from the payee table
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, drFoot["id"].ToString(), 0, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, drFoot["nom"].ToString(), 0, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, drFoot["adresse"].ToString(), 0, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, drFoot["tel"].ToString(), 0, 29, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, drFoot["email"].ToString() + " ", 0, 21, 0);
            // Bold text for ther headers
            tmpFooter.SetFontAndSize(f_cb, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 215, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 215, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Web : ", 215, 37, 0);
            // Regular text for infomation fields
            tmpFooter.SetFontAndSize(f_cn, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 265, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 265, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "http://www.bmi.com.tn/", 265, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "", 400, 45, 0);
            // End text
            tmpFooter.EndText();
            // Stamp a line above the page footer
            cb.SetLineWidth(0f);
            cb.MoveTo(30, 60);
            cb.LineTo(570, 60);
            cb.Stroke();
            // Return the footer template
            return tmpFooter;
        }
        //--------------------------------------------Covertir chiffre en lettre---------------------------------
        public string converti(float chiffre)
        {
            int centaine, dizaine, unite, reste, y;
            bool dix = false;
            string lettre = "";
            //strcpy(lettre, "");

            reste = (int)chiffre / 1;

            for (int i = 1000000000; i >= 1; i /= 1000)
            {
                y = reste / i;
                if (y != 0)
                {
                    centaine = y / 100;
                    dizaine = (y - centaine * 100) / 10;
                    unite = y - (centaine * 100) - (dizaine * 10);
                    switch (centaine)
                    {
                        case 0:
                            break;
                        case 1:
                            lettre += "cent ";
                            break;
                        case 2:
                            if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
                            else lettre += "deux cent ";
                            break;
                        case 3:
                            if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
                            else lettre += "trois cent ";
                            break;
                        case 4:
                            if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
                            else lettre += "quatre cent ";
                            break;
                        case 5:
                            if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
                            else lettre += "cinq cent ";
                            break;
                        case 6:
                            if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
                            else lettre += "six cent ";
                            break;
                        case 7:
                            if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
                            else lettre += "sept cent ";
                            break;
                        case 8:
                            if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
                            else lettre += "huit cent ";
                            break;
                        case 9:
                            if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
                            else lettre += "neuf cent ";
                            break;
                    }// endSwitch(centaine)

                    switch (dizaine)
                    {
                        case 0:
                            break;
                        case 1:
                            dix = true;
                            break;
                        case 2:
                            lettre += "vingt ";
                            break;
                        case 3:
                            lettre += "trente ";
                            break;
                        case 4:
                            lettre += "quarante ";
                            break;
                        case 5:
                            lettre += "cinquante ";
                            break;
                        case 6:
                            lettre += "soixante ";
                            break;
                        case 7:
                            dix = true;
                            lettre += "soixante ";
                            break;
                        case 8:
                            lettre += "quatre-vingt ";
                            break;
                        case 9:
                            dix = true;
                            lettre += "quatre-vingt ";
                            break;
                    } // endSwitch(dizaine)

                    switch (unite)
                    {
                        case 0:
                            if (dix) lettre += "dix ";
                            break;
                        case 1:
                            if (dix) lettre += "onze ";
                            else lettre += "un ";
                            break;
                        case 2:
                            if (dix) lettre += "douze ";
                            else lettre += "deux ";
                            break;
                        case 3:
                            if (dix) lettre += "treize ";
                            else lettre += "trois ";
                            break;
                        case 4:
                            if (dix) lettre += "quatorze ";
                            else lettre += "quatre ";
                            break;
                        case 5:
                            if (dix) lettre += "quinze ";
                            else lettre += "cinq ";
                            break;
                        case 6:
                            if (dix) lettre += "seize ";
                            else lettre += "six ";
                            break;
                        case 7:
                            if (dix) lettre += "dix-sept ";
                            else lettre += "sept ";
                            break;
                        case 8:
                            if (dix) lettre += "dix-huit ";
                            else lettre += "huit ";
                            break;
                        case 9:
                            if (dix) lettre += "dix-neuf ";
                            else lettre += "neuf ";
                            break;
                    } // endSwitch(unite)

                    switch (i)
                    {
                        case 1000000000:
                            if (y > 1) lettre += "milliards ";
                            else lettre += "milliard ";
                            break;
                        case 1000000:
                            if (y > 1) lettre += "millions ";
                            else lettre += "million ";
                            break;
                        case 1000:
                            lettre += "mille ";
                            break;
                    }
                } // end if(y!=0)
                reste -= y * i;
                dix = false;
            } // end for
            if (lettre.Length == 0) lettre += "zero";

            return lettre;
        }
        public int get3rdSpaceIndex(string s)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    count++;
                    if (count == 3)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        //---------------------------CALCUL TVA TTC-------------------------------------------
        public List<float> calculTvaTtc(float total)
        {
            List<float> calcul = new List<float>();
            float TVA = (total * 20) / 100;
            float TTC = total + TVA;
            calcul.Add(TVA);
            calcul.Add(TTC);
            return calcul;
        }
    }
}
