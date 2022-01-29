using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public DataAccessHelper.Database DB { get; set; } = DataAccessHelper.Database.Cls;
        private ProcessLogRun logRun;
        
        public ManualReviewHelper ReviewHelper { get; set; }
        public int? BorrowerInformationId { get; set; }
        public List<int> BorrowerInformationIds { get; set; } = new List<int>();
        public string Ssn { get; set; }

        //Use only when erroring for logging the ssn in the filename
        public string FileNameSsn { get; set; }

        public string ZipFileName { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            this.logRun = logRun;
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, true);
        }

        public void resetLocalVariablesForNewFile()
        {
            BorrowerInformationId = null;
            BorrowerInformationIds = new List<int>();
            Ssn = null;
            FileNameSsn = null;
            ZipFileName = null;
        }

        /// <summary>
        /// NOTE: The insert to BorrowerInformation will need to be done first so that a Foreign key is available for the other tables
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToBorrowerInformation]")]
        public int? InsertToBorrowerInformation(string ssn, decimal interestRate, DateTime firstPayDue, decimal paymentAmount, string repaymentPlan, string page)
        {
            ManagedDataResult<int> result = LDA.ExecuteSingle<int>("[pridrcrp].[InsertToBorrowerInformation]", DB, SP("Ssn", ssn), SP("InterestRate", interestRate), SP("FirstPayDue", firstPayDue), SP("PaymentAmount", paymentAmount), SP("RepayPlan", repaymentPlan), SP("Page", page), SP("ZipFile", ZipFileName == null ? "" : ZipFileName));
            if(result.DatabaseCallSuccessful)
            {
                //Set up borrower information Ids
                BorrowerInformationId = result.Result;
                BorrowerInformationIds.Add(result.Result);
                Ssn = ssn;
                return BorrowerInformationId;
            }
            else
            {
                BorrowerInformationId = null;
                logRun.AddNotification("Unable to add Borrower Information. SSN: " + ssn + " Interest Rate: " + interestRate.ToString() + " First Pay Due: " + firstPayDue.ToShortDateString() + " Payment Amount: " + paymentAmount + " Repay Plan: " + repaymentPlan, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ReviewHelper.FlagForManualReview(new List<Error>() { new Error(null, "Unable to add Borrower Information. SSN: " + ssn + " Interest Rate: " + interestRate.ToString() + " First Pay Due: " + firstPayDue.ToShortDateString() + " Payment Amount: " + paymentAmount + " Repay Plan: " + repaymentPlan) }, FileNameSsn);
                throw new FormatException("Bad borrower information record. Ssn: " + ssn);
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToDisbursements]")]
        public bool InsertToDisbursements(List<DisbursementRecord> records)
        {
            bool success = LDA.Execute("[pridrcrp].[InsertToDisbursements]", DB, SP("DisbursementRecord", records.ToDataTable()), SP("BorrowerInformationId", BorrowerInformationId));
            if(!success)
            {
                logRun.AddNotification("Unable to add Disbursement Records. BorrowerInformationId: " + BorrowerInformationId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ReviewHelper.FlagForManualReview(new List<Error>() { new Error(BorrowerInformationId, "Unable to add Disbrsement Records. BorrowerInformationId: " + BorrowerInformationId) }, FileNameSsn);
                throw new FormatException("Bad Disbursement Records. BorrowerInformationId: " + BorrowerInformationId);
            }
            return success;
        }
        
        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToPaymentHistory]")]
        public bool InsertToPaymentHistory(List<PaymentHistoryRecord> records)
        {
            bool success = LDA.Execute("[pridrcrp].[InsertToPaymentHistory]", DB, SP("PaymentHistoryRecord", records.ToDataTable()), SP("BorrowerInformationId", BorrowerInformationId));
            if (!success)
            {
                logRun.AddNotification("Unable to add Payment History Records. BorrowerInformationId: " + BorrowerInformationId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                //ReviewHelper.FlagForManualReview("Unable to add Payment History Records. BorrowerInformationId: " + BorrowInformationId, FileNameSsn);
                //throw new FormatException("Bad Payment History Records. BorrowerInformationId: " + BorrowInformationId);
            }
            return success;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[CheckLoansHaveBalance]")]
        public bool CheckLoansHaveBalance(string accountNumber)
        {
            bool? result = LDA.ExecuteSingle<bool?>("[pridrcrp].[CheckLoansHaveBalance]", DB, SP("AccountNumber", accountNumber)).Result;
            return result.HasValue ? result.Value : false;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[CheckBorrowerInformationIdHasLoan]")]
        public bool CheckBorrowerInformationIdHasLoan(int borrowerInformationId)
        {
            bool? result = LDA.ExecuteSingle<bool?>("[pridrcrp].[CheckBorrowerInformationIdHasLoan]", DB, SP("BorrowerInformationId", borrowerInformationId)).Result;
            return result.HasValue ? result.Value : false;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToMonetaryHistory]")]
        public bool InsertToMonetaryHistory(List<MonetaryHistoryRecord> records)
        {
            var result = LDA.ExecuteList<MonetaryHistoryIds>("[pridrcrp].[InsertToMonetaryHistory]", DB, SP("MonetaryHistoryRecord", records.ToDataTable()));
            if (!result.DatabaseCallSuccessful)
            {
                logRun.AddNotification("Unable to add Monetary History Records. Ssn: " + records.First().Ssn, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                //ReviewHelper.FlagForManualReview("Unable to add Monetary History Records. Ssn: " + records.First().Ssn, FileNameSsn);
                //throw new FormatException("Bad Monetary History Records. Ssn: " + records.First().Ssn);
            }
            //Insert Plans to a mapping table with the monetary history id
            List<MonetaryHistoryIds> ids = result.Result ?? new List<MonetaryHistoryIds>();
            foreach (var monetaryId in ids)
            {
                foreach (int planId in BorrowerInformationIds)
                {
                    InsertToMonetaryHistroyToBorrowerInformation(monetaryId.MonetaryHistoryId, planId);
                }
            }

            return result.DatabaseCallSuccessful;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToMonetaryHistoryToBorrowerInformation]")]
        public void InsertToMonetaryHistroyToBorrowerInformation(int monetaryHistoryId, int borrowerInformationId)
        {
            LDA.Execute("[pridrcrp].[InsertToMonetaryHistoryToBorrowerInformation]", DB, SP("BorrowerInformationId", borrowerInformationId), SP("MonetaryHistoryId", monetaryHistoryId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[CalculateLoanSequence]")]
        public List<LoanSequenceMappingRecord> CalculateLoanSequence(List<LoanSequenceMappingRecord> records)
        {
            return LDA.ExecuteList<LoanSequenceMappingRecord>("[pridrcrp].[CalculateLoanSequence]", DB, SP("LoanSequenceMappingRecord", records.ToDataTable())).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToBorrowerActivityHistory]")]
        public List<BorrowerActivityResult> InsertToBwrAtyHst(List<BorrowerActivityRecord> records)
        {
            List<BorrowerActivityResult> results = new List<BorrowerActivityResult>();
            //Insert the records one at a time to preserve their identity order so that the records match the file order.
            foreach(var rec in records)
            {
                var result = LDA.ExecuteList<BorrowerActivityResult>("[pridrcrp].[InsertToBorrowerActivityHistory]", DB, SP("BorrowerActivityRecord", new List<BorrowerActivityRecord>() { rec }.ToDataTable()), SP("BorrowerInformationId", BorrowerInformationId));
                if (!result.DatabaseCallSuccessful)
                {
                    logRun.AddNotification("Unable to add Borrower Activity Records. BorrowerInformationId: " + BorrowerInformationId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    ReviewHelper.FlagForManualReview(new List<Error>() { new Error(BorrowerInformationId, "Unable to add Borrower Activity Records. BorrowerInformationId: " + BorrowerInformationId) }, FileNameSsn);
                    throw new FormatException("Bad Borrower Activity Records. BorrowerInformationId: " + BorrowerInformationId);
                }
                foreach(var res in result.Result)
                {
                    results.Add(res);
                }
            }
            //var result = LDA.ExecuteList<BorrowerActivityResult>("[pridrcrp].[InsertToBorrowerActivityHistory]", DB, SP("BorrowerActivityRecord", records.ToDataTable()), SP("FirstRepaymentPlanId", FirstRepaymentPlanId));
            //if (!result.DatabaseCallSuccessful)
            //{
            //    logRun.AddNotification("Unable to add Borrower Activity Records. FirstRepaymentId: " + FirstRepaymentPlanId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            //    ReviewHelper.FlagForManualReview("Unable to add Borrower Activity Records. FirstRepaymentId: " + FirstRepaymentPlanId, FileNameSsn);
            //    throw new FormatException("Bad Borrower Activity Records. FirstRepaymentId: " + FirstRepaymentPlanId);
            //}
            return results;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToRepaymentPlanChanges]")]
        public bool InsertToRepaymentPlanChanges(BorrowerActivityResult result, RepaymentPlanChangesRecord record)
        {
            bool success = LDA.Execute("[pridrcrp].[InsertToRepaymentPlanChanges]", DB, SP("BorrowerInformationId", BorrowerInformationId), SP("BorrowerActivityId", result == null ? (object)DBNull.Value : (object)result.BorrowerActivityId), SP("PlanType", record.PlanType), SP("EffectiveDate", record.EffectiveDate));
            if (!success)
            {
                if(result == null)
                {
                    logRun.AddNotification("Unable to add Repayment Plan Change. BorrowerActivtyRecordId: NULL(automatically defaulting repayment plan)", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    ReviewHelper.FlagForManualReview(new List<Error>() { new Error(BorrowerInformationId, "Unable to add Repayment Plan Change. BorrowerActivtyRecordId: NULL(automatically defaulting repayment plan)") }, FileNameSsn);
                    throw new FormatException("Unable to add Repayment Plan Change. BorrowerActivtyRecordId: NULL(automatically defaulting repayment plan)");
                }
                else
                {
                    logRun.AddNotification("Unable to add Repayment Plan Change. BorrowerActivtyRecordId: " + result.BorrowerActivityId, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    ReviewHelper.FlagForManualReview(new List<Error>() { new Error(BorrowerInformationId, "Unable to add Repayment Plan Change. BorrowerActivtyRecordId: " + result.BorrowerActivityId) }, FileNameSsn);
                    throw new FormatException("Unable to add Repayment Plan Change. BorrowerActivtyRecordId: " + result.BorrowerActivityId);
                }
            }
            return success;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToReviewQueue]")]
        public bool InsertToReviewQueue(long? arcAddProcessingId, string exceptionLog, string ssn = null)
        {
            bool success = LDA.Execute("[pridrcrp].[InsertToReviewQueue]", DB, SP("SSN", Ssn ?? ssn), SP("ArcAddProcessingId", arcAddProcessingId), SP("ExceptionLog", exceptionLog));
            if (!success)
            {
                logRun.AddNotification("Unable to add Review Queue Record. Borrower Information Ids: " + (Ssn ?? ssn) + " Error Log: " + exceptionLog + " Arc Add Processing Id: " + arcAddProcessingId, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return success;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[GetAccountNumberFromSsn]")]
        public string GetAccountNumberFromSsn(string ssn)
        {
            return LDA.ExecuteSingle<string>("[pridrcrp].[GetAccountNumberFromSsn]", DB, SP("SSN", Ssn ?? ssn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[GetRepaymentPlanTypes]")]
        public List<RepaymentPlanTypeRecord> GetRepaymentPlanTypes()
        {
            return LDA.ExecuteList<RepaymentPlanTypeRecord>("[pridrcrp].[GetRepaymentPlanTypes]", DB).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToPaymentAmountChanges]")]
        public bool InsertToPaymentAmountChanges(int borrowerActivityId, decimal paymentAmount, DateTime? effectiveDate)
        {
            var result = LDA.Execute("[pridrcrp].[InsertToPaymentAmountChanges]", DB, SP("BorrowerInformationId", BorrowerInformationId), SP("BorrowerActivityId", borrowerActivityId), SP("PaymentAmount", paymentAmount), SP("EffectiveDate", effectiveDate));
            if (!result)
            {
                string formattedEffectiveDate = effectiveDate.HasValue ? effectiveDate.Value.ToShortDateString() : "";
                string errorMessage = $"Unable to add Payment Amount Change. Borrower Information Id: {BorrowerInformationId} Borrower Activity Id: {borrowerActivityId} Payment Amount: {paymentAmount} Effective Date: {formattedEffectiveDate}";
                ThrowDataAccessError(errorMessage);
            }
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToOutstandingPrincipalChanges]")]
        public bool InsertToOutstandingPrincipalChanges(int borrowerActivityId, decimal outstandingPrincipal, DateTime? effectiveDate)
        {
            bool result = LDA.Execute("[pridrcrp].[InsertToOutstandingPrincipalChanges]", DB, SP("BorrowerInformationId", BorrowerInformationId), SP("BorrowerActivityId", borrowerActivityId), SP("OutstandingPrincipal", outstandingPrincipal), SP("EffectiveDate", effectiveDate));
            if (!result)
            {
                string formattedEffectiveDate = effectiveDate.HasValue ? effectiveDate.Value.ToShortDateString() : "";
                string errorMessage = $"Unable to add Outstanding Principal Change. Borrower Information Id: {BorrowerInformationId} Borrower Activity Id: {borrowerActivityId} Outstanding Principal: {outstandingPrincipal.ToString()} Effective Date: {formattedEffectiveDate}";
                ThrowDataAccessError(errorMessage);
            }
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InsertToInterestRateChanges]")]
        public bool InsertToInterestRateChanges(int borrowerActivityId, decimal interestRate, DateTime? effectiveDate)
        {
            bool result = LDA.Execute("[pridrcrp].[InsertToInterestRateChanges]", DB, SP("BorrowerInformationId", BorrowerInformationId), SP("BorrowerActivityId", borrowerActivityId), SP("InterestRate", interestRate), SP("EffectiveDate", effectiveDate));
            if (!result)
            {
                string formattedEffectiveDate = effectiveDate.HasValue ? effectiveDate.Value.ToShortDateString() : "";
                string errorMessage = $"Unable to add Interest Rate Change. Borrower Information Id: {BorrowerInformationId} Borrower Activity Id: {borrowerActivityId} Interest Rate: {interestRate.ToString()} Effective Date: {formattedEffectiveDate}";
                ThrowDataAccessError(errorMessage);
            }
            return result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[GetBorrowerInformation]")]
        public List<BorrowerRecord> GetBorrowerInformation(int? borrowerInformationId = null)
        {
            return LDA.ExecuteList<BorrowerRecord>("[pridrcrp].[GetBorrowerInformation]", DB, SP("BorrowerInformationId", borrowerInformationId ?? BorrowerInformationId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[CheckExistingArc]")]
        public long? CheckExistingArc(string accountNumber, string arc)
        {
            List<long> results = LDA.ExecuteList<long>("[pridrcrp].[CheckExistingArc]", DB, SP("AccountNumber", accountNumber), SP("Arc", arc)).Result;
            return results != null && results.Count > 0 ? (long?)results.First() : null;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[GetLoanAddDate]")]
        public DateTime? GetLoanAddDate(int borrowerInformationId)
        {
            return LDA.ExecuteList<DateTime?>("[pridrcrp].[GetLoanAddDate]", DB, SP("BorrowerInformationId", borrowerInformationId)).Result.FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[AddMonthsInRepayment]")]
        public void AddMonthsInRepayment(int borrowerInformationId, DateTime loanAddDate, List<DueDatePeriod> dueDatePeriods)
        {
            bool success = LDA.Execute("[pridrcrp].[AddMonthsInRepayment]", DB, SP("BorrowerInformationId", borrowerInformationId), SP("LoanAddDate", loanAddDate), SP("DueDatePeriods", dueDatePeriods.ToDataTable()));
            if(!success)
            {
                string errorMessage = $"Errors adding MonthInRepayment. Borrower Information Id: { borrowerInformationId }";
                logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[GetDateInDefFor]")]
        public bool GetDateInDefFor(int borrowerInformationId, DateTime effectiveDate)
        {
            return LDA.ExecuteList<bool>("[pridrcrp].[GetDateInDefFor]", DB, SP("BorrowerInformationId", borrowerInformationId) , SP("EffectiveDate", effectiveDate.Date)).Result.FirstOrDefault();
        }

        
        [UsesSproc(DataAccessHelper.Database.Cls, "[pridrcrp].[InactivateDuplicateLoanBorrowerInformation]")]
        public bool InactivateDuplicateLoanBorrowerInformation()
        {
            return LDA.Execute("[pridrcrp].[InactivateDuplicateLoanBorrowerInformation]", DB);
        }

        public void ThrowDataAccessError(string errorMessage)
        {
            logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            ReviewHelper.FlagForManualReview(new List<Error>() { new Error(BorrowerInformationId, errorMessage) }, FileNameSsn);
            throw new FormatException(errorMessage);
        }

        public void ThrowDataValidationError(string errorMessage)
        {
            errorMessage += $" SSN: {FileNameSsn}";
            logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            ReviewHelper.FlagForManualReview(new List<Error>() { new Error(BorrowerInformationId, errorMessage) }, FileNameSsn);
            throw new FormatException(errorMessage);
        }

        private SqlParameter SP(string parameterName, object parameterValue)
        {
           return SqlParams.Single(parameterName, parameterValue);
        }

        
    }
}
