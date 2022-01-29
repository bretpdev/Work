using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class SsrsDownloader
    {
        public readonly string ReportServerUrl = "https://uheaassrs.uheaa.org/ReportServer?";
        private List<SsrsParameter> Parameters = new List<SsrsParameter>();
        public SsrsDownloader(string reportUrl, bool testMode = false)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls; //enable HTTPS
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (testMode)
                ReportServerUrl += "/Testing";
            ReportServerUrl += reportUrl;
        }

        public void AddParameter(string name, string value)
        {
            Parameters.Add(new SsrsParameter() { Name = name, Value = value });
        }

        public string GenerateLink()
        {
            string result = ReportServerUrl;
            foreach (var parameter in Parameters)
                result += "&" + parameter.Name + "=" + parameter.Value;
            return result;
        }

        public SsrsDownloadResults Download(string formatCode = "PDF")
        {
            var results = new SsrsDownloadResults();
            try
            {
                using (var client = new WebClient { UseDefaultCredentials = true })
                {
                    string reportUrl = ReportServerUrl + "&rs:Command=Render&rs:Format=" + formatCode;
                    foreach (var parameter in Parameters)
                        reportUrl += "&" + parameter.Name + "=" + parameter.Value;
                    results.ReportData = client.DownloadData(reportUrl);
                }
            }
            catch (Exception ex)
            {
                results.CaughtException = ex;
            }
            return results;
        }
    }
}
