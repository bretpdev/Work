using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace VERFORBFED
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public ProcessLogRun logRun { get; set; }
        public string AccountNumber { get; private set; }
        public string Ssn { get; private set; }
        private DataAccessHelper.Database DB { get; set; }

        //Note to programmer unable to use USesSproc because the database is not known at compile time
        public DataAccess(ProcessLogRun PLR, string accountNumber)
        {
            this.logRun = PLR;
            AccountNumber = accountNumber;
            DB = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone ? DB.Cdw : DB.Udw;
            var ident = GetAccountIdentifier(accountNumber);
            if (ident != null)
                Ssn = ident.Ssn;
        }

        /// <summary>
        /// Gets account identifier
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetBorrowerIdentifiers]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetBorrowerIdentifiers]")]
        public AccountIdentifier GetAccountIdentifier(string accountIdentifier)
        {
            var result = logRun.LDA.ExecuteSingle<AccountIdentifier>("[verforbfed].[GetBorrowerIdentifiers]", DB, SqlParams.Single("AccountIdentifier", accountIdentifier));
            return result.Result;
        }

        /// <summary>
        /// Gets the next payment due date for the current borrower
        /// </summary>
        /// <returns>The date of the next pay due.  If none exists returns null</returns>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetNextBillDueDate]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetNextBillDueDate]")]
        public DateTime? GetNextDueDate()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbfed].[GetNextBillDueDate]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Gets all of the loan statuses for the current borrower
        /// </summary>
        /// <returns>A list of all loan statuses on the account.</returns>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetLoanStatusInformation]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetLoanStatusInformation]")]
        public List<LoanStatusInfo> GetLoanStatuses()
        {
            return logRun.LDA.ExecuteList<LoanStatusInfo>("[verforbfed].[GetLoanStatusInformation]", DB, Sp("Ssn", Ssn ?? "")).Result;
        }

        /// <summary>
        /// Gets borrower loan programs
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetBorrowerLoanPrograms]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetBorrowerLoanPrograms]")]
        public List<string> GetLoanPrograms()
        {
            return logRun.LDA.ExecuteList<string>("[verforbfed].[GetBorrowerLoanPrograms]", DB, Sp("Ssn", Ssn ?? "")).Result;
        }

        /// <summary>
        /// Gets the forbearance end date for the given borrower
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetForbearanceEndDate]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetForbearanceEndDate]")]
        public DateTime? GetForbearanceEndDate()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbfed].[GetForbearanceEndDate]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the Deferment end date for the given borrower
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetDefermentEndDate]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetDefermentEndDate]")]
        public DateTime? GetDefermentEndDate()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbfed].[GetDefermentEndDate]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower is paid ahead
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerIsPaidAhead]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerIsPaidAhead]")]
        public bool CheckPaidAhead()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerIsPaidAhead]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks to see if borrower has Invalid repayment schedules
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerHasInvalidRepaymentSchedule]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerHasInvalidRepaymentSchedule]")]
        public bool CheckRepaymentSchedules()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerHasInvalidRepaymentSchedule]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower has 2 different repayment schedules
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerHasSplitSchedules]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerHasSplitSchedules]")]
        public bool HasSplitSchedules()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerHasSplitSchedules]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the borrowers current payment amount and checks to see if they are equal to 0.00
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerPaymentAmountEqualsZero]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerPaymentAmountEqualsZero]")]
        public bool PaymentAmountEqualsZero()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerPaymentAmountEqualsZero]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the first date that the borrower become delinquent
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetDateDelinquencyOccurred]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetDateDelinquencyOccurred]")]
        public DateTime? GetDateDelinquencyOccurred()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbfed].[GetDateDelinquencyOccurred]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower has a future dated forbearance.
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerHasFutureDatedForbearance]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerHasFutureDatedForbearance]")]
        public bool HasFutureDatedForbearance()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerHasFutureDatedForbearance]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower has a future dated deferment.
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerHasFutureDatedDeferment]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerHasFutureDatedDeferment]")]
        public bool HasFutureDatedDeferment()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerHasFutureDatedDeferment]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets all of the borrowers collection suspense forbearance they have had on their account
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetCollectionSuspenseInfo]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetCollectionSuspenseInfo]")]
        public List<CollectionSuspenseForbearance> GetCollectionSuspenseForbInfo()
        {
            return logRun.LDA.ExecuteList<CollectionSuspenseForbearance>("[verforbfed].[GetCollectionSuspenseInfo]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Get the total number of month the borrower has been in forbearance
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetMaxForbMonthsUsed]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetMaxForbMonthsUsed]")]
        public decimal GetNumberOfForbDaysUsed()
        {
            return logRun.LDA.ExecuteSingle<decimal>("[verforbfed].[GetTotalForbDaysUsed]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the borrowers Days past due
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetDaysPastDue]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetDaysPastDue]")]
        public int GetDaysPastDue()
        {
            return logRun.LDA.ExecuteSingle<int>("[verforbfed].[GetDaysPastDue]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks if the borrower has used their special 120 forbearance
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[GetBorrowerSpecialStatuses]")]
        [UsesSproc(DB.Udw, "[verforbfed].[GetBorrowerSpecialStatuses]")]
        public List<int> GetSpecialStatuses()
        {
            return logRun.LDA.ExecuteList<int>("[verforbfed].[GetBorrowerSpecialStatuses]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks to see if borrower has spousal consolidation loans
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerHasSpousalLoans]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerHasSpousalLoans]")]
        public bool CheckSpousalLoans(string ssn)
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerHasSpousalLoans]", DB, Sp("SSN", ssn)).Result;
        }

        /// <summary>
        /// Checks if borrower had a suspension on past 30 days.
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerUsedCollectionSuspensionInLast30Days]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerUsedCollectionSuspensionInLast30Days]")]
        public bool BorrowerUsedCollectionSuspensionInLast30Days()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerUsedCollectionSuspensionInLast30Days]", DB, Sp("SSN", Ssn)).Result;
        }

        /// <summary>
        /// Checks if borrower made a full payment in the past year
        /// </summary>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerMadeFullPaymentInLastYear]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerMadeFullPaymentInLastYear]")]
        public bool BorrowerMadeFullPaymentInLastYear(string ssn)
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerMadeFullPaymentInLastYear]", DB, Sp("SSN", ssn)).Result;
        }

        /// <summary>
        /// Checks if the borrower had a forbearance in the past year.
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        [UsesSproc(DB.Cdw, "[verforbfed].[BorrowerUsedForbearanceWithinLastYear]")]
        [UsesSproc(DB.Udw, "[verforbfed].[BorrowerUsedForbearanceWithinLastYear]")]
        public bool BorrowerUsedForbearanceWithinLastYear(string ssn)
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbfed].[BorrowerUsedForbearanceWithinLastYear]", DB, Sp("Ssn", ssn)).Result;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }


    }
}
