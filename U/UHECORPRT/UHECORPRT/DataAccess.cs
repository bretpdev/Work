using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using System.Data;
using Uheaa.Common.ProcessLogger;

namespace UHECORPRT
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess(ProcessLogRun logRun)
        {
            this.LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetSSNFromAcctNumber")]
        public string GetSsnFromAcctNumber(string accoutNumber)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", accoutNumber)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetAccountNumberFromSSN")]
        public string GetAccountNumberFromSsn(string ssn)
        {
            return LDA.ExecuteSingle<string>("spGetAccountNumberFromSSN", DataAccessHelper.Database.Udw, SqlParams.Single("Ssn", ssn)).Result;
        }

        /// <summary>
        /// Gets the Next highest priority Script.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetNextScriptForProcessing")]
        public ScriptData PopulateScriptData(List<ScriptData> previouslyProcessed)
        {
            ScriptData sd;
            if (previouslyProcessed.Any())
                sd = LDA.ExecuteList<ScriptData>("[print].GetNextScriptForProcessing", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptDataIds", previouslyProcessed.Select(p => new { ScriptDataIds = p.ScriptDataId }).ToDataTable())).Result.SingleOrDefault();
            else
                sd = LDA.ExecuteList<ScriptData>("[print].GetNextScriptForProcessing", DataAccessHelper.Database.Uls).Result.SingleOrDefault();
            if (sd != null)
            {
                sd.FileHeaderConst = sd.FileHeader;
            }
            return sd;
        }

        /// <summary>
        /// Gets all of the PrintProcessing data for a given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetPendingAccountsForScript")]
        public List<PrintProcessingData> PopulatePrintProcessingData(int scriptDataId)
        {
            List<PrintProcessingData> ppd = LDA.ExecuteList<PrintProcessingData>("[print].GetPendingAccountsForScript", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptDataId", scriptDataId)).Result;
            foreach (PrintProcessingData p in ppd)
            {
                p.LetterDataConst = p.LetterData;
            }
            return ppd;
        }

        /// <summary>
        /// Gets the Arc Information for a given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetArcData")]
        public List<ArcInformation> GetDataForArc(int id)
        {
            return LDA.ExecuteList<ArcInformation>("[print].GetArcData", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptDataId", id)).Result;
        }

        /// <summary>
        /// Gets the Header information for a geven ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetHeadersForArcs")]
        public List<string> GetHeadersForArcs(int id)
        {
            return LDA.ExecuteList<string>("[print].GetHeadersForArcs", DataAccessHelper.Database.Uls, SqlParams.Single("ArcScriptDataMappingId", id)).Result;
        }

        /// <summary>
        /// Marks that the Arc is complete.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetArcAddComplete")]
        public void MarkArcAdded(int printProcessingId, int arcAddProcessingId)
        {
            LDA.Execute("[print].SetArcAddComplete", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId), SqlParams.Single("ArcAddProcessingId", arcAddProcessingId));
        }

        /// <summary>
        /// Marks that EcorrDocument is created.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetEcorrDocumentCreatedAt")]
        public void MarkEcorrDone(int printProcessingId)
        {
            LDA.Execute("[print].SetEcorrDocumentCreatedAt", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks imaging complete.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetImagedAt")]
        public void MarkImaged(int printProcessingId)
        {
            LDA.Execute("[print].SetImagedAt", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks Printing Complete
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetPrintingComplete")]
        public void MarkPrintingComplete(int printProcessingId)
        {
            LDA.Execute("[print].SetPrintingComplete", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Gets the Last Processed Date for the given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetLastProcessed")]
        public void SetLastProcessed(int scriptDataId)
        {
            LDA.Execute("[print].SetLastProcessed", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptDataId", scriptDataId));
        }

        /// <summary>
        /// Gets all the scripts the need to have a file loaded.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetAllScriptsForProcessing")]
        public List<ScriptData> GetScriptsToLoad()
        {
            return LDA.ExecuteList<ScriptData>("[print].GetAllScriptsForProcessing", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Marks Printing Complete For The CoBorrower Table
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetPrintingCompleteCoBorrower")]
        public void MarkPrintingCompleteCoBorrower(int printProcessingId)
        {
            LDA.Execute("[print].SetPrintingCompleteCoBorrower", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks imaging complete for the CoBorrower Table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetImagedAtCoBorrower")]
        public void MarkImagedCoBorrower(int printProcessingId)
        {
            LDA.Execute("[print].SetImagedAtCoBorrower", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        /// <summary>
        /// Marks that EcorrDocument is created for the CoBorrower Table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].SetEcorrDocumentCreatedAtCoBorrower")]
        public void MarkEcorrDoneCoBorrower(int printProcessingId)
        {
            LDA.Execute("[print].SetEcorrDocumentCreatedAtCoBorrower", DataAccessHelper.Database.Uls, SqlParams.Single("PrintProcessingId", printProcessingId));
        }

        // <summary>
        /// Inserts a row into the PrintProcessingCoBorrower table generated from the information on a Borrower's letter
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].InsertPrintProcessingRecordCoBwr")]
        public void InsertPrintProcessingRecordCoBwr(string scriptId, string letterId, string letterData, string accountNumber, string costCenter, string borrowerSsn)
        {
            EcorrProcessing.AddCoBwrRecordToPrintProcessing(scriptId, letterId, letterData, accountNumber, costCenter, borrowerSsn);
        }

        /// <summary>
        /// Gets all of the PrintProcessingCoBorrower data for a given ScriptDataId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetPendingCoBorrowerAccountsForScript")]
        public List<PrintProcessingData> PopulatePrintProcessingCoBorrowerData(int scriptDataId)
        {
            List<PrintProcessingData> ppd = LDA.ExecuteList<PrintProcessingData>("[print].GetPendingCoBorrowerAccountsForScript", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptDataId", scriptDataId)).CheckResult();
            foreach (PrintProcessingData p in ppd)
            {
                p.LetterDataConst = p.LetterData;
            }
            return ppd;
        }

        /// <summary>
        /// Gets the internal name of file header columns.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetInternalNames")]
        public System.Data.DataTable GetInternalNames()
        {
            return LDA.ExecuteDataTable("[print].GetInternalNames", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Gets information of the CoBorrowers of a specified Borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[print].GetCoBorrowersForBorrower")]
        public List<CoBorrowerInformation> GetCoBorrowersForBorrower(string borrowerSSN)
        {
            return LDA.ExecuteList<CoBorrowerInformation>("[print].GetCoBorrowersForBorrower", DataAccessHelper.Database.Uls, SqlParams.Single("BorrowerSSN", borrowerSSN)).Result;
        }

    }
}
