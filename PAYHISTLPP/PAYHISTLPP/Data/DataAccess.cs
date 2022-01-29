using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace PAYHISTLPP
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) =>
            LDA = lda;

        /// <summary>
        /// Gets the list of users that have access to run the script
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.GetUserAccess")]
        public List<UserAccess> GetUsers() =>
            LDA.ExecuteList<UserAccess>("payhistlpp.GetUserAccess", Uls).Result;

        /// <summary>
        /// Gets a list of the active lender codes
        /// </summary>
        [UsesSproc(Udw, "payhistlpp.GetLenders")]
        public List<string> GetLenders() =>
            LDA.ExecuteList<string>("payhistlpp.GetLenders", Udw).Result;

        /// <summary>
        /// Returns all runs ids that are open and not deleted
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.GetAllRecoveryRunIds")]
        public List<Process> GetAllRecoveryIds(int userId, string lender) =>
            LDA.ExecuteList<Process>("payhistlpp.GetAllRecoveryRunIds", Uls,
                Sp("UserAccessId", userId),
                Sp("Lender", lender)).Result;

        /// <summary>
        /// Deletes all accounts that are in a run being deleted
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.DeleteUnprocessedAccounts")]
        public void DeleteUnproceesedAccounts(int runId) =>
            LDA.Execute("payhistlpp.DeleteUnprocessedAccounts", Uls,
                Sp("RunId", runId));

        /// <summary>
        /// Deletes a run that is in recovery
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.DeleteRun")]
        public void DeleteRun(int runId) =>
            LDA.Execute("payhistlpp.DeleteRun", Uls,
                Sp("RunId", runId));

        /// <summary>
        /// Starts a run and sets up recovery
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.StartRun")]
        public int StartRun(UserAccess user, bool tilp) =>
            LDA.ExecuteId<int>("payhistlpp.StartRun", Uls,
                Sp("UserAccessId", user.UserAccessId),
                Sp("Tilp", tilp));

        /// <summary>
        /// Updates the directory used so the job can recover
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.UpdateRunDir")]
        public void UpdateDirectory(string dir, int runId) =>
            LDA.Execute("payhistlpp.UpdateRunDir", Uls,
                Sp("Dir", dir),
                Sp("RunId", runId));

        /// <summary>
        /// Loads borrower data into the Accounts table
        /// </summary>
        [UsesSproc(Udw, "payhistlpp.LoadAccounts")]
        public void LoadAccounts(int runId, int userId, string lender, int count, bool isTilp) =>
            LDA.Execute("payhistlpp.LoadAccounts", Udw,
                Sp("UserAccessId", userId),
                Sp("RunId", runId),
                Sp("Lender", lender),
                Sp("Count", count),
                Sp("IsTilp", isTilp));

        /// <summary>
        /// Gets all the borrower data for the user and run id
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.GetAccounts")]
        public List<Accounts> GetAccounts(int runId, int userId) =>
            LDA.ExecuteList<Accounts>("payhistlpp.GetAccounts", Uls,
                Sp("UserAccessId", userId),
                Sp("RunId", runId)).Result;

        /// <summary>
        /// Inserts a single borrower record for manual processing
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.InsertSingleRecord")]
        public void InsertSingleRecord(int runId, int userId, string ssn, string lender) =>
            LDA.Execute("payhistlpp.InsertSingleRecord", Uls,
                Sp("UserAccessId", userId),
                Sp("RunId", runId),
                Sp("Ssn", ssn),
                Sp("Lender", lender));

        /// <summary>
        /// Checks if the ssn has been processed
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.CheckProcessedAccount")]
        public bool CheckProcessedAccount(string account) =>
            LDA.ExecuteSingle<bool>("payhistlpp.CheckProcessedAccount", Uls,
                Sp("Account", account)).Result;

        /// <summary>
        /// Gets a count of the unprocessed accounts for the current run
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.GetUnprocessedCount")]
        public int GetUnprocessedCount(int runId) =>
            LDA.ExecuteSingle<int>("payhistlpp.GetUnprocessedCount", Uls,
                Sp("RunId", runId)).Result;

        /// <summary>
        /// Gets the SSN for the passed in account number
        /// </summary>
        [UsesSproc(Udw, "payhistlpp.GetSsn")]
        public string GetSsn(string account) =>
            LDA.ExecuteSingle<string>("payhistlpp.GetSsn", Udw,
                Sp("Account", account)).Result;

        /// <summary>
        /// Gets the loan programs and disbursement dates
        /// </summary>
        [UsesSproc(Udw, "payhistlpp.GetLoans")]
        public List<Loan> GetLoans(string ssn, string lender, bool isTilp) =>
            LDA.ExecuteList<Loan>("payhistlpp.GetLoans", Udw,
                Sp("BF_SSN", ssn),
                Sp("Lender", lender),
                Sp("IsTilp", isTilp)).Result;

        /// <summary>
        /// Gets the payments for a loan by unique id
        /// </summary>
        [UsesSproc(Udw, "payhistlpp.GetPayments")]
        [UsesSproc(Udw, "payhistlpp.GetAlternatePayments")]
        public List<Payment> GetPayments(string uniqueId, string uniqueIdSeq, int lnseq, string ssn, string lender = "", bool isTilp = false)
        {
            List<Payment> payments = new List<Payment>();
            if (uniqueId.IsPopulated())
                payments = LDA.ExecuteList<Payment>("payhistlpp.GetPayments", Udw,
                    Sp("UniqueId", uniqueId),
                    Sp("UniqueIdSeq", uniqueIdSeq)).Result;

            if (payments == null || payments.Count == 0)
                payments = LDA.ExecuteList<Payment>("payhistlpp.GetAlternatePayments", Udw,
                    Sp("UniqueId", uniqueId),
                    Sp("LN_SEQ", lnseq),
                    Sp("BF_SSN", ssn),
                    Sp("Lender", lender),
                    Sp("IsTilp", isTilp)).Result;

            return payments;
        }

        /// <summary>
        /// Sets the borrower account as processed so they are not picked up again
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.SetProcessedAt")]
        public void SetProcessedAt(int accountsId) =>
            LDA.Execute("payhistlpp.SetProcessedAt", Uls,
                Sp("AccountsId", accountsId));

        /// <summary>
        /// Sets the run job as completed
        /// </summary>
        [UsesSproc(Uls, "payhistlpp.SetCompletedAt")]
        public void SetCompletedAt(int runId) =>
            LDA.Execute("payhistlpp.SetCompletedAt", Uls,
                Sp("RunId", runId));

        public SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}