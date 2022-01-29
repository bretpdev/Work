using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace TILPCROST
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Inserts a single record into the Accounts table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "tilpcrost.InsertRecord")]
        public void InsertRecord(TilpRecord record)
        {
            LogRun.LDA.Execute("tilpcrost.InsertRecord", DataAccessHelper.Database.Uls,
                SP("AccountNumber", record.AccountNumber),
                SP("TransactionType", record.TransactionType),
                SP("LoanSequence", record.LoanSequence),
                SP("PrincipalAmount", record.PrincipalAmount),
                SP("TransactionDate", record.TransactionDate),
                SP("LastName", record.LastName));
        }

        /// <summary>
        /// Gets all accounts ready to process
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "tilpcrost.GetAvailableAccounts")]
        public List<TilpRecord> GetAccounts()
        {
            return LogRun.LDA.ExecuteList<TilpRecord>("tilpcrost.GetAvailableAccounts", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Sets an individual account as processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "tilpcrost.SetProcessedAt")]
        public void SetProcessed(int accountsId)
        {
            LogRun.LDA.Execute("tilpcrost.SetProcessedAt", DataAccessHelper.Database.Uls,
                SP("AccountsId", accountsId));
        }

        private SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}