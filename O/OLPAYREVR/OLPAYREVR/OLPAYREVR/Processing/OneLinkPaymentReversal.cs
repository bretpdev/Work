using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace OLPAYREVR
{
    public class OneLinkPaymentReversal : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }

        public OneLinkPaymentReversal(ReflectionInterface ri) : base(ri, "OLPAYREVR", DataAccessHelper.Region.Uheaa)
        {
            LogRun = RI.LogRun ?? new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            DA = new DataAccess(LogRun);
        }

        public override void Main()
        {
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            Run();

            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
        }

        public int Run()
        {
            if (!new FileLoader(LogRun, DA).ReadAndLoadFile())
                return 1;

            if (!GatherDbTransactionInfo())
                return 1;

            List<Payment> payments = DA.GetUnprocessedRecords();
            if (payments == null)
            {
                LogRun.AddNotification("Unable to retrieve data from the database", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok("Unable to retrieve data from the database. Please consult System Support.");
                return 1;
            }

            if (payments.Count() == 0)
            {
                Dialog.Info.Ok("There are no unworked records to process. Script run complete.");
                return 0;
            }

            if (!new PaymentProcessor(LogRun, DA, RI).ProcessPaymentReversals(payments))
            {
                LogRun.AddNotification($"Payment reversal processing encountered issues. Please contact System Support concerning Process Log Id # {LogRun.ProcessLogId}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok("Not all payments were reversed successfully. Please click OK to see the payments that were not successfully reversed by the script in the run report.");
                ProvideRunResults();
                
                return 1;
            }

            Dialog.Info.Ok("Script successfully completed. Please click OK to see the run results in the error report.");
            ProvideRunResults();
            return 0;
        }

        private bool GatherDbTransactionInfo()
        {
            if (!DA.AddDbTransactionInfo())
            {
                string error = $"Database error encountered. Please reach out to System Support concerning PL # {LogRun.ProcessLogId}.";
                Dialog.Error.Ok(error);
                return false;
            }
            return true;
        }

        private void ProvideRunResults()
        {
            string recipients = EnterpriseFileSystem.GetPath("olpayrevrrecipients", DataAccessHelper.Region.Uheaa);
            string reportLink = EnterpriseFileSystem.GetPath("olpayrevrreport", DataAccessHelper.Region.Uheaa);
            SendEmail(recipients, reportLink);
            System.Diagnostics.Process.Start(reportLink);
        }

        private void SendEmail(string recipients, string reportLink)
        {
            string message = "Please use the following link to navigate to an error report.";
            message += $"<br /><br /><a href='{reportLink}'>{reportLink}</a>";
            EmailHelper.SendMail(
                    DataAccessHelper.TestMode,
                    recipients,
                    Environment.UserName + "@uheaa.org",
                    "OneLink Payment Reversal Run Results",
                    message,
                    "jkieschnick@uheaa.org", EmailHelper.EmailImportance.Normal, true
            );
        }


    }
}
