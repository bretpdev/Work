using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using RegentsApp.Reporting;
using System.IO;
using CrystalDecisions.Shared;

namespace RegentsApp
{
    public partial class Report : System.Web.UI.Page
    {
        string username;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                username = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                username = "";
            }
            DataAccessLayer dal = new DataAccessLayer();
            //Instantiate a report and set its data source.
            CompletedApplication report = new CompletedApplication();
            try
            {
                report.SetDataSource(dal.GetPdfRecords(username).AsEnumerable());
                //Export the report to a memory stream and send it to the Response object.
                MemoryStream pdfStream = (report.ExportToStream(ExportFormatType.PortableDocFormat) as MemoryStream);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(pdfStream.ToArray());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                // Intentionally left empty to stop the site from crashing
            }
        }
    }
}
