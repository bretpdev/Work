using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace OPSWebEntry
{
    public static class ErrorHelper
    {
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetFileSystemObject")]
        public static void ErrorProcessingPayment(OPSPayment payment, List<Server.SubmitResults> resultLog)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ActivityLog.LogError(string.Format("Error Processing!  Payment skipped. Row #{0}, SSN {1}, Name {2}", payment.ID, payment.SSN, payment.Name));
            payment.MarkAsSkipped();
            string reportLocation = @"T:\OPSWebEntry\Error\";
            string folderName = string.Format("rowid#{0} - {1:yyyy-MM-dd_HH-mm-ss}", payment.ID, DateTime.Now);
            reportLocation = Path.Combine(reportLocation, folderName);

            WriteResultLog(reportLocation, resultLog);
            ActivityLog.LogNotification("Error Report created at " + reportLocation, reportLocation);

            //using (ManagedReflectionInterface ri = new ManagedReflectionInterface())
            //{
            //    bool success = false;
            //    try
            //    {
            //        success = ri.Atd22AllLoans(payment.SSN, "OPSPE", "Error Processing OPSWebEntry payment.  Row #: " + payment.ID, "", "OPSEntry", false);
            //    }
            //    catch (Exception) { }
            //    finally
            //    {
            //        if (success)
            //            ActivityLog.LogNotification("Error logged at ATD22.");
            //        else
            //            ActivityLog.LogError("Could not log invalid payment in ATD22.");
            //    }
            //}
        }

        public static void SuccessProcessingPayment(OPSPayment payment, List<Server.SubmitResults> resultLog)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string reportLocation = @"T:\OPSWebEntry\Success\";
            string folderName = string.Format("rowid#{0} - {1:yyyy-MM-dd_HH-mm-ss}", payment.ID, DateTime.Now);
            reportLocation = Path.Combine(reportLocation, folderName);

            WriteResultLog(reportLocation, resultLog);
        }

        private static void WriteResultLog(string reportLocation, List<Server.SubmitResults> resultLog)
        {
            if (!Directory.Exists(reportLocation)) Directory.CreateDirectory(reportLocation);
            for (int i = 0; i < resultLog.Count; i++)
            {
                var log = resultLog[i];
                string step = (i + 1).ToString();
                File.WriteAllText(Path.Combine(reportLocation, step + " - request.txt"), 
                    string.Format("Request: {0}\nResponse{1}\nFormData:{2}", 
                        log.RequestUrl, 
                        log.ResponseUri.IsNull(o => o.OriginalString, "[no response]"), 
                        log.FormData
                    ));
                File.WriteAllText(Path.Combine(reportLocation, step + " - response.htm"), log.Response);
                if (log.Exception != null)
                    File.WriteAllText(Path.Combine(reportLocation, step + " - EXCEPTION.html"), log.Exception.ToString());
            }
        }
    }
}
