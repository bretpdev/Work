using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public class ManualReviewHelper
    {
        private DataAccess DA;
        private ProcessLogRun logRun;

        private static readonly string ReviewArc = "PCONT";

        public ManualReviewHelper(DataAccess DA, ProcessLogRun logRun)
        {
            this.DA = DA;
            this.logRun = logRun;
        }

        public void FlagForManualReview(List<Error> errorLog, string ssn = null)
        {
            string accountNumber = DA.GetAccountNumberFromSsn(ssn);
            bool hasBalance = DA.CheckLoansHaveBalance(accountNumber);

            //First check that some loan has a balance
            if (hasBalance)
            {
                //We need to remove errors tied to a first repayment id that is not tied to a loan with a balance on compass
                List<Error> filteredErrors = new List<Error>();
                var groupedErrors = errorLog.GroupBy(p => p.BorrowerInformationId);
                foreach (var group in groupedErrors)
                {
                    bool hasLoan = true;
                    List<Error> errors = group.ToList();
                    if (errors.FirstOrDefault() != null && errors.FirstOrDefault().BorrowerInformationId.HasValue)
                    {
                        hasLoan = DA.CheckBorrowerInformationIdHasLoan(errors.FirstOrDefault().BorrowerInformationId.Value);
                    }
                    if (hasLoan)
                    {
                        filteredErrors.AddRange(errors);
                    }
                }

                //Only add an arc and database record if there are remaining errors after filtering
                if (filteredErrors.Count > 0)
                {
                    string stringErrorLog = "";
                    foreach (Error error in filteredErrors)
                    {
                        stringErrorLog = (stringErrorLog + " " + error.ErrorMessage).Trim();
                    }

                    long? arcAddProcessingId = DA.CheckExistingArc(accountNumber, ReviewArc);
                    if (!arcAddProcessingId.HasValue)
                    {
                        arcAddProcessingId = AddManualReviewArc(accountNumber, DA.BorrowerInformationIds, stringErrorLog);
                    }
                    AddManualReviewDatabaseRecord(arcAddProcessingId, stringErrorLog, ssn);
                }
            }
        }

        private int? AddManualReviewArc(string accountNumber, List<int> firstRepymentPlanIds, string errorMessage = null)
        {
            string ids = StringParsingHelper.GetCommaDelimitedList(firstRepymentPlanIds);
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = accountNumber,
                Arc = ReviewArc,
                Comment = $"Borrower pre-transfer file requires review. Please see First Repayment Plan Ids: {ids}",
                ScriptId = Program.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                errorMessage = "Unable to add manual review arc for Account Number: " + accountNumber + " First Repayment Plan Ids: ";
                foreach(int id in firstRepymentPlanIds)
                {
                    errorMessage += id.ToString() + ";";
                }
                logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            return result.ArcAddProcessingId;
        }

        private bool AddManualReviewDatabaseRecord(long? arcAddProcessingId, string errorLog, string ssn = null)
        {
            return DA.InsertToReviewQueue(arcAddProcessingId, errorLog, ssn);
        }

    }
}
