using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class SsrsDownloadResults
    {
        public Exception CaughtException { get; set; }
        public byte[] ReportData { get; set; }
        private bool DownloadSuccessful => CaughtException == null;
        private WebException CaughtAsWebException => CaughtException as WebException;
        private HttpWebResponse CaughtResponse => CaughtAsWebException?.Response as HttpWebResponse;
        public bool UrlFound
        {
            get
            {
                if (DownloadSuccessful)
                    return true;
                if (CaughtAsWebException == null)
                    return true;
                if (CaughtAsWebException.Status != WebExceptionStatus.NameResolutionFailure)
                    return true;
                return false;
            }
        }
        public bool AuthenticationSuccessful
        {
            get
            {
                if (DownloadSuccessful)
                    return true;
                if (CaughtResponse == null)
                    return true;
                if (CaughtResponse.StatusCode != HttpStatusCode.Unauthorized)
                    return true;
                return false;
            }
        }
        public bool ReportFound
        {
            get
            {
                if (!DownloadSuccessful)
                    return false;
                byte[] reportBrowserHeader = Encoding.UTF8.GetBytes("<!DOCTYPE HTML PUBLIC");
                return !ReportData.Take(reportBrowserHeader.Length).SequenceEqual(reportBrowserHeader);
            }
        }

        public void SaveDataAsFile(string fileName)
        {
            File.WriteAllBytes(fileName, ReportData);
        }

        public override string ToString()
        {
            return $"Url Found {UrlFound}; Authentication Successful {AuthenticationSuccessful}; ReportFound {ReportFound};";
        }
    }
}
