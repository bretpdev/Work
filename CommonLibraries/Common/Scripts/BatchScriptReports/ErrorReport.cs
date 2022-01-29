using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
    //UNDONE this is not ready to be made obsolete
    //[Obsolete("This has been replaced with the ProcessLogger notification functionality.", false)]
    public class ErrorReport : ErrorReport<Object>
    {
        public ErrorReport(string reportName, string fileSystemKey) : base(reportName, fileSystemKey, "") { }
        public ErrorReport(string reportName, string fileSystemKey, string userId = "")
            : base(reportName, fileSystemKey, userId) { }
    }

    public class ErrorReport<T>
    {
        private readonly string ReportName;
        private readonly string DataFile;
        private readonly string PublicationDirectory;
        public bool HasErrors
        {
            get
            {
                return File.Exists(DataFile);
            }
        }

        public ErrorReport(string reportName, string fileSystemKey, string userId = "")
        {
            DataFile = userId.IsNullOrEmpty() ? string.Format("{0}ERR_{1}.txt", EnterpriseFileSystem.TempFolder, reportName) : string.Format("{0}ERR_{1}_{2}.txt", EnterpriseFileSystem.TempFolder, reportName, userId);
            PublicationDirectory = EnterpriseFileSystem.GetPath(fileSystemKey);
            ReportName = reportName;
        }

        public void AddRecord(string message, T item)
        {
            //Reflect into the object to get its property names, which we'll need in one or two places.
            IEnumerable<string> itemProperties = item.GetType().GetProperties().Select(p => p.Name);

            if (!File.Exists(DataFile))
            {
                List<string> headers = new List<string>();
                //Start with the error message.
                headers.Add("Message");
                //Add the properties of the object.
                headers.AddRange(itemProperties);
                using (StreamWriter sw = new StreamW(DataFile))
                {
                    sw.WriteCommaDelimitedLine(headers.ToArray());
                }
            }

            //Write out a data line from the message and the object's properties.
            List<string> values = new List<string>();
            values.Add(message);

            foreach (string propertyName in itemProperties)
            {
                Object itemProperty = item.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, item, new List<Object>().ToArray());
                if (itemProperty.GetType() == typeof(List<int>))
                {
                    PropertyInfo info = item.GetType().GetProperty(propertyName);
                    dynamic val = info.GetValue(item);

                    values.Add(string.Join(",", val));
                }
                else
                    values.Add(itemProperty == null ? "" : itemProperty.ToString());
            }

            using (StreamWriter sw = new StreamW(DataFile, true))
            {
                sw.WriteCommaDelimitedLine(values.ToArray());
            }
        }

        public void Publish()
        {
            //See if there's anything to publish.
            if (!File.Exists(DataFile)) { return; }

            //In case this method gets called more than once per application, use a new file name each time.
            //Slap a counter on the end if needed (i.e., Publish() gets called more than once in a given minute).

            int reportNumber = 1;

            string htmlFile = string.Format("{0}{1} Error Report {2:MM-dd-yyyy HH.mm}.html", PublicationDirectory, ReportName, DateTime.Now);
            while (File.Exists(htmlFile))
            {
                string oldExtension = reportNumber == 1 ? ".html" : string.Format(".{0}.html", reportNumber);
                reportNumber++;
                string newExtension = string.Format(".{0}.html", reportNumber);
                htmlFile = htmlFile.Replace(oldExtension, newExtension);
            }

            using (StreamWriter sw = new StreamW(htmlFile))
            {
                //Start the HTML file and write out the report headers.
                sw.WriteLine("<html>");
                sw.WriteLine("<head>");
                sw.WriteLine("<style type='text/css'>");
                sw.WriteLine("body{font-family:Arial,Helvetica,sans-serif;}table{border-collapse:collapse;}td,th{padding:2px 10px;}tr.oddrow{background-color:#EEE;}h1{font-size:20px;}h2{font-size:16px;}");
                sw.WriteLine("</style>");
                sw.WriteLine("</head>");
                sw.WriteLine("<body>");
                sw.WriteLine("<h1>" + ReportName + "<h1>");
                sw.WriteLine("<h2>Error Report</h2>");
                sw.WriteLine("<h2>" + htmlFile + "</h2>");
                //Convert the data file's contents to an HTML table and write it out.

                using (StreamReader sr = new StreamR(DataFile))
                {
                    List<string> dataFromFile = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        dataFromFile.Add(sr.ReadLine());
                    }

                    foreach (string item in dataFromFile.ToHtmlLines("	"))
                    {
                        sw.WriteLine(item);
                    }

                    //close out the HTML.
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                }
            }
            FS.Delete(DataFile);
        }
    }
}
