using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace DPAPOST
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        /// <summary>
        /// Gets all the unprocessed records
        /// </summary>
        [UsesSproc(Ols, "dpapost.GetUnprocessedData")]
        public List<FileData> GetRecords() =>
            LDA.ExecuteList<FileData>("dpapost.GetUnprocessedData", Ols).Result;

        /// <summary>
        /// Sets the record to Deleted
        /// </summary>
        [UsesSproc(Ols, "dpapost.DeleteRecord")]
        public void DeleteRecord(int postingDataId) =>
            LDA.Execute("dpapost.DeleteRecord", Ols,
                Sp("PostingDataId", postingDataId));

        /// <summary>
        /// Inserts new record to be processed
        /// </summary>
        [UsesSproc(Ols, "dpapost.InsertData")]
        public void InsertRecord(FileData fd) =>
            LDA.Execute("dpapost.InsertData", Ols,
                Sp("AccountNumber", fd.AccountNumber),
                Sp("Amount", fd.Amount));

        /// <summary>
        /// Sets the record as processed
        /// </summary>
        [UsesSproc(Ols, "dpapost.SetProcessed")]
        public void SetProcessed(int postingDataId) =>
            LDA.Execute("dpapost.SetProcessed", Ols,
                Sp("PostingDataId", postingDataId));

        /// <summary>
        /// Sets the ErrorPosting value if there was an error is posting the payment
        /// </summary>
        [UsesSproc(Ols, "dpapost.SetErrorPosting")]
        public void SetErrorPosting(int postingDataId) =>
            LDA.Execute("dpapost.SetErrorPosting", Ols,
                Sp("PostingDataId", postingDataId));

        private SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}