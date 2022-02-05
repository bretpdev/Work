using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using System.Reflection;

namespace ADUPCNTEML
{
    class Program
    {
        public static ProcessLogRun LogRun;
        public static DataAccess DA;
        public static ReflectionInterface RI;
        public static string ScriptId = "ADUPCNTEML";
        static int successCount = 0;
        static int failureCount = 0;

        static int Main(string[] args)
        {
            if (!ValidExecutionCheck(args)) //required arguments and sproc access check
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            DA = new DataAccess(LogRun);
            List<BorrowerData> Borrowers = DA.IdentifyBorrowers();
            if (!Login()) /*Log in to the session*/
                return 1;

            foreach (BorrowerData bor in Borrowers)
            {
                AddOrModifyEmail(bor);
            }

            LogRun.AddNotification(string.Format("Total Borrowers Processed: {0}; Total Successful: {1}; Total Failures: {2}", successCount + failureCount, successCount, failureCount), NotificationType.EndOfJob, NotificationSeverityType.Informational);
            LogRun.LogEnd();
            RI.CloseSession();
            return 0;
        }

        /// <summary>
        /// Log into a session using a batch ID
        /// </summary>
        /// <returns>true if login to session is successful</returns>
        private static bool Login()
        {
            BatchProcessingHelper loginCredentials = BatchProcessingHelper.GetNextAvailableId(ScriptId, "BatchUheaa");
            RI = new ReflectionInterface();
            if (!RI.Login(loginCredentials.UserName, loginCredentials.Password))
            {
                LogRun.AddNotification(string.Format("Failed to log in to UHEAA session using batch user: {0}.  Please resolve access issue and rerun job.", loginCredentials.UserName), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates the email address in the session to the address provided in the BorrowerData object.  Counts successes and failures
        /// </summary>
        /// <param name="bor"></param>
        private static void AddOrModifyEmail(BorrowerData bor)
        {
            RI.FastPath("TX3ZCTX1J;" + bor.SSN);
            if (!RI.CheckForText(1, 71, "TXX1R-01"))/*Check screen is accurate if not process log*/
            {
                LogRun.AddNotification(string.Format("Did not reach TXX1R for borrower: {0}, unable to update email information {1}", bor.AccountNumber, bor.EmailAddress), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                failureCount++;
                return;
            }
            RI.Hit(ReflectionInterface.Key.F2);
            RI.Hit(ReflectionInterface.Key.F10);/*Get to email page*/
            if (UpdateEmail(bor) != "01005") /*01005 is successfully added*/
            {
                LogRun.AddNotification(string.Format("Failed to update email for borrower: {0}, Error Message: {1}", bor.AccountNumber, RI.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                failureCount++;
            }
            else
            {
                LogRun.AddNotification(string.Format("Updated email for borrower: {0}", bor.AccountNumber), NotificationType.EndOfJob, NotificationSeverityType.Informational);
                successCount++;
            }
        }

        /// <summary>
        /// Puts in the necessary updates on the email page in the session.  
        /// </summary>
        /// <param name="bor"></param>
        /// <returns>Returns the session message code.</returns>
        private static string UpdateEmail(BorrowerData bor)
        {
            RI.PutText(9, 20, bor.SourceCode, true); /*Email Source*/
            RI.PutText(11, 17, bor.ValidityDate.ToDate().Month.ToString().PadLeft(2, '0'), true);/*Email update date*/
            RI.PutText(11, 20, bor.ValidityDate.ToDate().Day.ToString().PadLeft(2, '0'), true);
            RI.PutText(11, 23, bor.ValidityDate.ToDate().Year.ToString().SafeSubString(2, 2), true);
            RI.PutText(12, 14, "Y", true);
            RI.PutText(14, 10, bor.EmailAddress, ReflectionInterface.Key.Enter, true); /*Update the email*/
            return RI.MessageCode;
        }

        /// <summary>
        /// Checks for access to stored procedures and that a mode is selected(Dev or live)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool ValidExecutionCheck(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "Add / Update contact email", false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return false;
            return true;
        }
    }
}
