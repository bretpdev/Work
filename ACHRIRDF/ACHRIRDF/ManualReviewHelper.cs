using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACHRIRDF
{
    public class ManualReviewHelper
    {
        private DataAccess DA { get; set; }
        private ProcessLogRun logRun { get; set; }

        private static readonly string ReviewArc = "BSARR";

        public ManualReviewHelper(DataAccess DA, ProcessLogRun logRun)
        {
            this.DA = DA;
            this.logRun = logRun;
        }

        public void FlagForManualReview(string accountNumber, string errorLog)
        {
            long? arcAddProcessingId = DA.CheckExistingArc(accountNumber, ReviewArc, errorLog);
            if(errorLog.Length > 1232)
            {
                logRun.AddNotification($"Error log exceeded 1232 characters, account number: {accountNumber}, error log: {errorLog}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            //add new arc if there isn't one out there right now
            if (!arcAddProcessingId.HasValue)
            {
                arcAddProcessingId = AddManualReviewArc(accountNumber, errorLog);
            }
            else
            {
                logRun.AddNotification($"Account already has manual review arc, not adding a new arc. Account Number: {accountNumber} Error Message: {errorLog}", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        private int? AddManualReviewArc(string accountNumber, string errorMessage = null)
        {
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = accountNumber,
                Arc = "BSARR", 
                Comment = errorMessage,
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                errorMessage = "Unable to add manual review arc for Account Number: " + accountNumber;
                logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            return result.ArcAddProcessingId;
        }

    }
}
