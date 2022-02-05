using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Uheaa.Common.ProcessLogger;

namespace IDRCONRPT
{
    public class XPathHelper
    {
        ProcessLogRun PLR;
        public XPathHelper(ProcessLogRun plr)
        {
            this.PLR = plr;
        }
        public List<Borrower> GetRejects(XPathNavigator nav, string outputPath)
        {
            var borrower = nav.Select("//Borrower");
            List<Borrower> rejects = new List<Borrower>();
            while (borrower.MoveNext())
            {
                var response = borrower.Current.SelectSingleNode("Response/ResponseCode");
                if (response.Value != "Accepted")
                {
                    var b = new Borrower();
                    b.Ssn = borrower.Current.SelectSingleNode("SSN")?.Value;
                    b.FirstName = borrower.Current.SelectSingleNode("*/FirstName")?.Value;
                    b.LastName = borrower.Current.SelectSingleNode("*/LastName")?.Value;
                    b.Path = outputPath;
                    rejects.Add(b);
                }
            }
            return rejects;
        }

        public XPathNavigator GetNavigatorFromContents(string filename, string fileContents)
        {
            XPathDocument doc = null;
            try
            {
                fileContents = fileContents.Substring(fileContents.IndexOf("<?xml")); //strip weird garbage from beginning of file
                fileContents = fileContents.Substring(0, fileContents.IndexOf("</app:ApplicationActivity>") + "</app:ApplicationActivity>".Length); //strip weird garbage from end of file
                doc = new XPathDocument(new StringReader(fileContents));
            }
            catch (Exception ex)
            {
                PLR.AddNotification("Unable to load file as XML: " + filename, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                return null;
            }
            return doc.CreateNavigator();
        }
    }
}
