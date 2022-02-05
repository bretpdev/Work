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

namespace VERFORBUH
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
            DB = DB.Udw;
            var ident = GetAccountIdentifier(accountNumber);
            if (ident != null)
                Ssn = ident.Ssn;
        }

        /// <summary>
        /// Gets account identifier
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetBorrowerIdentifiers]")]
        public AccountIdentifier GetAccountIdentifier(string accountIdentifier)
        {
            var result = logRun.LDA.ExecuteSingle<AccountIdentifier>("[verforbuh].[GetBorrowerIdentifiers]", DB, SqlParams.Single("AccountIdentifier", accountIdentifier));
            return result.Result;
        }

        /// <summary>
        /// Gets the next payment due date for the current borrower
        /// </summary>
        /// <returns>The date of the next pay due.  If none exists returns null</returns>
        [UsesSproc(DB.Udw, "[verforbuh].[GetNextBillDueDate]")]
        public DateTime? GetNextDueDate()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbuh].[GetNextBillDueDate]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Gets all of the loan statuses for the current borrower
        /// </summary>
        /// <returns>A list of all loan statuses on the account.</returns>
        [UsesSproc(DB.Udw, "[verforbuh].[GetLoanStatusInformation]")]
        public List<LoanStatusInfo> GetLoanStatuses()
        {
            return logRun.LDA.ExecuteList<LoanStatusInfo>("[verforbuh].[GetLoanStatusInformation]", DB, Sp("Ssn", Ssn ?? "")).Result;
        }

        /// <summary>
        /// Gets borrower loan programs
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetBorrowerLoanPrograms]")]
        public List<string> GetLoanPrograms()
        {
            return logRun.LDA.ExecuteList<string>("[verforbuh].[GetBorrowerLoanPrograms]", DB, Sp("Ssn", Ssn ?? "")).Result;
        }

        /// <summary>
        /// Gets the forbearance end date for the given borrower
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetForbearanceEndDate]")]
        public DateTime? GetForbearanceEndDate()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbuh].[GetForbearanceEndDate]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the Deferment end date for the given borrower
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetDefermentEndDate]")]
        public DateTime? GetDefermentEndDate()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbuh].[GetDefermentEndDate]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower is paid ahead
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerIsPaidAhead]")]
        public bool CheckPaidAhead()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerIsPaidAhead]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks to see if borrower has Invalid repayment schedules
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerHasInvalidRepaymentSchedule]")]
        public bool CheckRepaymentSchedules()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerHasInvalidRepaymentSchedule]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower has 2 different repayment schedules
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerHasSplitSchedules]")]
        public bool HasSplitSchedules()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerHasSplitSchedules]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the borrowers current payment amount and checks to see if they are equal to 0.00
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerPaymentAmountEqualsZero]")]
        public bool PaymentAmountEqualsZero()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerPaymentAmountEqualsZero]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the first date that the borrower become delinquent
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetDateDelinquencyOccurred]")]
        public DateTime? GetDateDelinquencyOccurred()
        {
            return logRun.LDA.ExecuteSingle<DateTime?>("[verforbuh].[GetDateDelinquencyOccurred]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower has a future dated forbearance.
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerHasFutureDatedForbearance]")]
        public bool HasFutureDatedForbearance()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerHasFutureDatedForbearance]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Checks to see if a borrower has a future dated deferment.
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerHasFutureDatedDeferment]")]
        public bool HasFutureDatedDeferment()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerHasFutureDatedDeferment]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets all of the borrowers collection suspense forbearance they have had on their account
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetCollectionSuspenseInfo]")]
        public List<CollectionSuspenseForbearance> GetCollectionSuspenseForbInfo()
        {
            return logRun.LDA.ExecuteList<CollectionSuspenseForbearance>("[verforbuh].[GetCollectionSuspenseInfo]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Get the total number of month the borrower has been in forbearance
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetMaxForbMonthsUsed]")]
        public decimal GetNumberOfForbDaysUsed()
        {
            return logRun.LDA.ExecuteSingle<decimal>("[verforbuh].[GetTotalForbDaysUsed]", DB, Sp("Ssn", Ssn)).Result;
        }

        /// <summary>
        /// Gets the borrowers Days past due
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetDaysPastDue]")]
        public int GetDaysPastDue()
        {
            return logRun.LDA.ExecuteSingle<int>("[verforbuh].[GetDaysPastDue]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks if the borrower has used their special 120 forbearance
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[GetBorrowerSpecialStatuses]")]
        public List<int> GetSpecialStatuses()
        {
            return logRun.LDA.ExecuteList<int>("[verforbuh].[GetBorrowerSpecialStatuses]", DB, Sp("AccountNumber", AccountNumber)).Result;
        }

        /// <summary>
        /// Checks to see if borrower has spousal consolidation loans
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerHasSpousalLoans]")]
        public bool CheckSpousalLoans(string ssn)
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerHasSpousalLoans]", DB, Sp("SSN", ssn)).Result;
        }

        /// <summary>
        /// Checks if borrower had a suspension on past 30 days.
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerUsedCollectionSuspensionInLast30Days]")]
        public bool BorrowerUsedCollectionSuspensionInLast30Days()
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerUsedCollectionSuspensionInLast30Days]", DB, Sp("SSN", Ssn)).Result;
        }

        /// <summary>
        /// Checks if borrower made a full payment in the past year
        /// </summary>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerMadeFullPaymentInLastYear]")]
        public bool BorrowerMadeFullPaymentInLastYear(string ssn)
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerMadeFullPaymentInLastYear]", DB, Sp("SSN", ssn)).Result;
        }

        /// <summary>
        /// Checks if the borrower had a forbearance in the past year.
        /// </summary>
        /// <param name="ssn"></param>
        /// <returns></returns>
        [UsesSproc(DB.Udw, "[verforbuh].[BorrowerUsedForbearanceWithinLastYear]")]
        public bool BorrowerUsedForbearanceWithinLastYear(string ssn)
        {
            return logRun.LDA.ExecuteSingle<bool>("[verforbuh].[BorrowerUsedForbearanceWithinLastYear]", DB, Sp("Ssn", ssn)).Result;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }


    }
}
