using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace PFE.Controler
{
    public class ExternalCssPdfParser
    {
        public static byte[] createPdf(DataTable dt, Decimal val)
        {
            Document document = new Document(PageSize.A4);
            byte[] result;
            string htmltext = @"<html>
                                <body>
                                    <h1 class='HeaderStyle'> Simulation</h1>
                                    <div class='color_Green></div>
                                    < div class='color_Green></div>
                                    <table border = '1'>
                                    <tr>";
            foreach (DataColumn column in dt.Columns)
            {
                htmltext += "<th align='center' style = 'background-color: Gray;'>";
                htmltext += column.ColumnName;
                htmltext += "</th>";
            }
            htmltext += "</tr>";
            foreach (DataRow row in dt.Rows)
            {
                htmltext += "<tr>";
                foreach (DataColumn column in dt.Columns)
                {
                    htmltext += "<td>";
                    htmltext += row[column];
                    htmltext += "</td>";
                }
                htmltext += "</tr>";
            }
            string lign = "<tr><td align = 'right' colspan = '" + (dt.Columns.Count - 1).ToString() + "'>Total</td><td>" + val.ToString() + "</td></tr></table>";
            htmltext += lign;
            htmltext += "</body></html>";
            List<string> cssFiles = new List<string>();
            cssFiles.Add(@"../assets/css/style.css");
            using (var ms = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                writer.CloseStream = false;
                document.Open();
                HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                cssFiles.ForEach(i => cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath(i), true));

                IPipeline pipeline = new CssResolverPipeline(cssResolver,
                            new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));
                XMLWorker worker = new XMLWorker(pipeline, true);
                XMLParser xmlParser = new XMLParser(worker);
                xmlParser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(htmltext)));// i added the toSting function
                document.Close();
                result = ms.GetBuffer();
            }
            return result;
        }
    }
}