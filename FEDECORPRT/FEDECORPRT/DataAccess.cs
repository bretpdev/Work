using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace FEDECORPRT
{
    public  class DataAccess
    {
        private static SqlConnection ClsConn;
        private LogDataAccess LDA { get; set; }
        public DataAccess(int processLogId)
        {
            ClsConn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.Cls, DataAccessHelper.CurrentMode));
            ClsConn.Open();
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, false, true);
        }


        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
        public string GetSsnFromAcctNumber(string accoutNumber)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, SqlParams.Single("AccountNumber", accoutNumber)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetAccountNumberFromSSN")]
        public string GetAccountNumberFromSsn(string ssn)
        {
            return LDA.ExecuteSingle<string>("spGetAccountNumberFromSSN", DataAccessHelper.Database.Cdw, SqlParams.Single("Ssn", ssn)).CheckResult();
        }

        /// <summary>
        /// Gets the Next highest priority Script.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetNextScriptForProcessing")]
        public ScriptData PopulateScriptData(List<ScriptData> previouslyProcessed)
        {
            ScriptData sd;
            if (previouslyProcessed.Any())
                sd = DataAccessHelper.ExecuteList<ScriptData>("[print].GetNextScriptForProcessing", ClsConn, SqlParams.Single("ScriptDataIds", previouslyProcessed.Select(p => new { ScriptDataIds = p.ScriptDataId }).ToDataTable())).SingleOrDefault();
            else
                sd = DataAccessHelper.ExecuteList<ScriptData>("[print].GetNextScriptForProcessing", ClsConn).SingleOrDefault();
            if (sd != null)
            {
                sd.FileHeaderConst = sd.FileHeader;
            }
            return sd;
        }

        /// <summary>
        /// Gets all of the PrintProcessing data for a given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetPendingAccountsForScript")]
        public List<PrintProcessingData> PopulatePrintProcessingData(int scriptDataId)
        {
            List<PrintProcessingData> ppd =  LDA.ExecuteList<PrintProcessingData>("[print].GetPendingAccountsForScript", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptDataId", scriptDataId)).CheckResult();
            foreach(PrintProcessingData p in ppd)
            {
                p.LetterDataConst = p.LetterData;
            }
            return ppd;
        }

        /// <summary>
        /// Gets the Arc Information for a given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetArcData")]
        public List<ArcInformation> GetDataForArc(int id)
        {
            return LDA.ExecuteList<ArcInformation>("[print].GetArcData", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptDataId", id)).CheckResult();
        }

        /// <summary>
        /// Gets the Header information for a geven ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetHeadersForArcs")]
        public List<string> GetHeadersForArcs(int id)
        {
            return LDA.ExecuteList<string>("[print].GetHeadersForArcs", DataAccessHelper.Database.Cls, SqlParams.Single("ArcScriptDataMappingId", id)).CheckResult();
        }

        /// <summary>
        /// Marks that the Arc is complete.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetArcAddComplete")]
        public void MarkArcAdded(int printProcessingId, int arcAddProcessingId)
        {
            LDA.Execute("[print].SetArcAddComplete", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId), SqlParams.Single("ArcAddProcessingId", arcAddProcessingId));
        }

        /// <summary>
        /// Marks that EcorrDocument is created.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetEcorrDocumentCreatedAt")]
        public void MarkEcorrDone(int printProcessingId)
        {
            LDA.Execute("[print].SetEcorrDocumentCreatedAt", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks imaging complete.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetImagedAt")]
        public void MarkImaged(int printProcessingId)
        {
            LDA.Execute("[print].SetImagedAt", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId)); 
        }

        /// <summary>
        /// Marks imaging complete for PrintProcessingCoBorrower.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetImagedAtCoBorrower")]
        public void MarkImagedCoBorrower(int printProcessingId)
        {
            LDA.Execute("[print].SetImagedAtCoBorrower", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks Printing Complete.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetPrintingComplete")]
        public void MarkPrintingComplete(int printProcessingId)
        {
            LDA.Execute("[print].SetPrintingComplete", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks Printing Complete for PrintProcessingCoBorrower.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetPrintingCompleteCoBorrower")]
        public void MarkPrintingCompleteCoBorrower(int printProcessingId)
        {
            LDA.Execute("[print].SetPrintingCompleteCoBorrower", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Gets the Last Processed Date for the given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetLastProcessed")]
        public void SetLastProcessed(int scriptDataId)
        {
            LDA.Execute("[print].SetLastProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptDataId", scriptDataId));
        }

        /// <summary>
        /// Gets all the scripts the need to have a file loaded.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetAllScriptsForProcessing")]
        public List<ScriptData> GetScriptsToLoad()
        {
            return LDA.ExecuteList<ScriptData>("[print].GetAllScriptsForProcessing", DataAccessHelper.Database.Cls).CheckResult();
        }

        /// <summary>
        /// Gets information of the CoBorrowers of a specified Borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetCoBorrowersForBorrower")]
        public List<CoBorrowerInformation> GetCoBorrowersForBorrower(string borrowerSSN)
        {
            return LDA.ExecuteList<CoBorrowerInformation>("[print].GetCoBorrowersForBorrower", DataAccessHelper.Database.Cls, SqlParams.Single("BorrowerSSN", borrowerSSN)).Result;
        }

        /// <summary>
        /// Inserts a row into the PrintProcessingCoBorrower table generated from the information on a Borrower's letter
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].InsertPrintProcessingRecordCoBwr")]
        public void InsertPrintProcessingRecordCoBwr(string accountNumber, string scriptId, string letterData, string costCenter, string letter, string borrowerSsn)
        {
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(scriptId, letter, letterData, accountNumber, costCenter, borrowerSsn);
        }

        /// <summary>
        /// Gets all of the PrintProcessingCoBorrower data for a given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetPendingCoBorrowerAccountsForScript")]
        public List<PrintProcessingData> PopulatePrintProcessingCoBorrowerData(int scriptDataId)
        {
            List<PrintProcessingData> ppd = LDA.ExecuteList<PrintProcessingData>("[print].GetPendingCoBorrowerAccountsForScript", DataAccessHelper.Database.Cls, SqlParams.Single("ScriptDataId", scriptDataId)).CheckResult();
            foreach (PrintProcessingData p in ppd)
            {
                p.LetterDataConst = p.LetterData;
            }
            return ppd;
        }

        /// <summary>
        /// Marks that EcorrDocument is created for CoBorrowers.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].SetCoBorrowerEcorrDocumentCreatedAt")]
        public void MarkEcorrDoneCoBorrower(int printProcessingId)
        {
            LDA.Execute("[print].SetCoBorrowerEcorrDocumentCreatedAt", DataAccessHelper.Database.Cls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Gets the internal name of file header columns.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetInternalNames")]
        public System.Data.DataTable GetInternalNames()
        {
            return LDA.ExecuteDataTable("[print].GetInternalNames", DataAccessHelper.Database.Cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[print].GetBulkLoadCount")]
        public int GetBulkLoadCount()
        {
            return LDA.ExecuteSingle<int>(string.Format("[print].GetBulkLoadCount"), DataAccessHelper.Database.Cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[print].Delete_BulkLoad")]
        public void DeleteBulkLoad()
        {
            LDA.Execute(string.Format("[print].Delete_BulkLoad"), DataAccessHelper.Database.Cls);
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[print].LoadPrintData")]
        public void LoadPrintData(int scriptDataId, string fileName)
        {
            LDA.Execute(string.Format("[print].LoadPrintData"), DataAccessHelper.Database.Cls, 300 /*5 minute timeout*/,
                            SqlParams.Single("ScriptDataId", scriptDataId),
                            SqlParams.Single("SourceFile", fileName),
                            SqlParams.Single("AddedBy", Environment.UserName));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[print].CheckFileProcessed")]
        public bool CheckFileProcessed(string fileName)
        {
            return LDA.ExecuteSingle<int>(string.Format("[print].CheckFileProcessed"), DataAccessHelper.Database.Cls, SqlParams.Single("SourceFile", fileName)).Result > 0;
        }

    }
}