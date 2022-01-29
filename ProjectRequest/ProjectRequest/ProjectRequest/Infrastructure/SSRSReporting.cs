using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.IO;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System.Text;

namespace ProjectRequest.Infrastructure
{
    public static class SSRSReporting
    {
        public static SsrsDownloadResults GetAllProjects()
        {
            //string reportServerUrl = "https://uheaassrs.uheaa.org/ReportServer?";
            string report = "/KPI/SupportServices/Project Request Report"; ///Home

            SsrsDownloader dl = new SsrsDownloader(report, DataAccessHelper.TestMode);
            SsrsDownloadResults result = dl.Download("Excel");
            if (!result.ReportFound)
            {
                return result;
            }
            else
            {
                return result;
            }
        }
    }
}