using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace BILLING
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess(int processLogId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, true, true);
        }

        /// <summary>
        /// Gets a list of all unprocessed borrowers
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetPendingAccountsForScript")]
        public ManagedDataResult<List<Borrower>> GetUnprocessedBorrowerData(string ScriptId)
        {
            return LDA.ExecuteList<Borrower>("[print].GetPendingAccountsForScript", DB.Uls, SqlParams.Single("ScriptId", ScriptId));
        }

        /// <summary>
        /// Gets the line data for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetLineDataForAccount")]
        public ManagedDataResult<string> GetLineData(int printProcessingId)
        {
            return LDA.ExecuteSingle<string>("[print].GetLineDataForAccount", DB.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Sets the printing complete for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetPrintingCompleteWithTable")]
        public bool SetPrintComplete(DataTable printProcIds)
        {
            return LDA.Execute("[print].SetPrintingCompleteWithTable", DB.Uls, SqlParams.Single("PrintProcessingId", printProcIds));
        }

        /// <summary>
        /// Sets the Comment complete for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetArcAddComplete")]
        public bool SetCommentComplete(int printProcId)
        {
            return LDA.Execute("[print].SetArcAddComplete", DB.Uls, SqlParams.Single("PrintProcessingId", printProcId));
        }

        /// <summary>
        /// Sets the Imaging complete for the current borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetImagedAtWithTable")]
        public bool SetImageComplete(DataTable printProcIds)
        {
            return LDA.Execute("[print].SetImagedAtWithTable", DB.Uls , SqlParams.Single("PrintProcessingId", printProcIds));
        }

        /// <summary>
        /// Sets the data record date when the document is created.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetDocumentCreatedAtWithTable")]
        public bool SetDocumentCreated(DataTable printProcIds)
        {
            return LDA.Execute("[print].SetDocumentCreatedAtWithTable", DB.Uls, SqlParams.Single("PrintProcessingId", printProcIds));
        }

        /// <summary>
        /// Gets the DocId's being used by Billing
        /// </summary>
        /// <returns>list of strings of all doc Id's used</returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetDocIds")]
        public ManagedDataResult<List<string>> GetAllLetterIds()
        {
            return LDA.ExecuteList<string>("[print].GetBillingLetterIds", DataAccessHelper.Database.Uls);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "GetBillingDelqComment")]
        public ManagedDataResult<string> GetDelqComment(int daysDelq)
        {
            var result = LDA.ExecuteList<string>("GetBillingDelqComment", DataAccessHelper.Database.Uls,
                SqlParams.Single("DaysDelinquent", daysDelq));
            var newResult = new ManagedDataResult<string>();
            newResult.DatabaseCallSuccessful = result.DatabaseCallSuccessful;
            newResult.Result = result.Result.SingleOrDefault();
            return newResult;
        }
    }
}