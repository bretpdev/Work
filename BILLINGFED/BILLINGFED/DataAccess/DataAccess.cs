using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace BILLINGFED
{
    public class DataAccess
    {
        private ProcessLogRun LogRun { get; set; }
        public SqlConnection CdwConn { get; set; }
        public SqlConnection ClsConn { get; set; }
        public SqlConnection EcorrConn { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            CdwConn = new SqlConnection(DataAccessHelper.GetConnectionString(DB.Cdw));
            CdwConn.Open();
            ClsConn = new SqlConnection(DataAccessHelper.GetConnectionString(DB.Cls));
            ClsConn.Open();
            EcorrConn = new SqlConnection(DataAccessHelper.GetConnectionString(DB.ECorrFed));
            EcorrConn.Open();
        }

        /// <summary>
        /// Gets a list of all borrowers that have something that needs to be processed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].GetPendingAccountsForScript")]
        public List<Borrower> GetUnprocessedBorrowerData(string ScriptId)
        {
            return DataAccessHelper.ExecuteList<Borrower>("[billing].GetPendingAccountsForScript", ClsConn, SqlParams.Single("ScriptId", ScriptId));
        }

        /// <summary>
        /// Gets all the line data for the borrowr
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].GetLineDataForAccount")]
        public List<string> GetLineData(int printProcessingId)
        {
            return DataAccessHelper.ExecuteList<string>("[billing].GetLineDataForAccount", ClsConn, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Gets the special message text for the bill
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].GetSpecialMessages")]
        public BillText GetTextForBill(int reportNumber)
        {
            return DataAccessHelper.ExecuteSingle<BillText>("[billing].GetSpecialMessages", ClsConn, SqlParams.Single("ReportNumber", $"{reportNumber}"));
        }

        /// <summary>
        /// Sets the borrowers document as printed
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].SetPrintedAt")]
        public DateTime SetPrintedAt(int printProcessingId)
        {
            return DataAccessHelper.ExecuteSingle<DateTime>("[billing].SetPrintedAt", ClsConn, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Set the borrowers account as having arc added
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].SetArcAddedAt")]
        public DateTime SetArcProcessed(int printProcessingId)
        {
            return DataAccessHelper.ExecuteSingle<DateTime>("[billing].SetArcAddedAt", ClsConn, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Sets the borrowers ecorr document as created
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].SetEcorrDocumentCreatedAt")]
        public DateTime SetEcorrDocumentCreated(int PrintProcessingId)
        {
            return DataAccessHelper.ExecuteSingle<DateTime>("[billing].SetEcorrDocumentCreatedAt", ClsConn,  SqlParams.Single("PrintProcessingId", PrintProcessingId));
        }

        /// <summary>
        /// Set the borroers document as having been imaged
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].SetImagedAt")]
        public DateTime SetImagedAt(int printProcessingId)
        {
            return DataAccessHelper.ExecuteSingle<DateTime>("[billing].SetImagedAt", ClsConn, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Gets the endorser ssn from PD10
        /// </summary>
        /// <param name="endAcctNum"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
        public string GetSsnFromAcctNum(string endAcctNum)
        {
            var result = DataAccessHelper.ExecuteList<string>("spGetSSNFromAcctNumber", CdwConn, SqlParams.Single("AccountNumber", endAcctNum));
            return result.SingleOrDefault();
        }

        /// <summary>
        /// Gets the bill level data
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].GetBillData")]
        public List<BillData> GetBillData()
        {
            return DataAccessHelper.ExecuteList<BillData>("[billing].GetBillData", ClsConn);
        }


        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].GetHeader")]
        public Dictionary<string, int> GetHeaderData()
        {
            Dictionary<string, int> fileData = new Dictionary<string, int>();
            List<string> header = DataAccessHelper.ExecuteSingle<string>("[billing].GetHeader", ClsConn).SplitAndRemoveQuotes(",");
            for (int index = 0; index < header.Count; index++)
                fileData.Add(header[index], index);

            return fileData;
        }
    }
}