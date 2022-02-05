using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NTISFCFED
{
    public class DataAccess
    {
        private ProcessLogRun PLR { get; set; }

        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
        }

        /// <summary>
        /// Gets all of the alt format documents to send
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetAlternateFormatRecords")]
        public List<DbData> Populate()
        {
            return PLR.LDA.ExecuteList<DbData>("GetAlternateFormatRecords", DataAccessHelper.Database.ECorrFed).Result;
        }

        /// <summary>
        /// Marks the letter as sent
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "MarkAltFormatAsProcessed")]
        public void UpdateRecordsAsProcessed(List<DbData> borrowers)
        {
            foreach (DbData item in borrowers)
                PLR.LDA.Execute("MarkAltFormatAsProcessed", DataAccessHelper.Database.ECorrFed, SqlParams.Single("DocumentDetailsId", item.DocumentDetailsId));
        }

        /// <summary>
        /// Inserts the run into the NTISFCFEDRunHistory table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "InsertNTISFCFEDRunRecord")]
        public void InsertRunRecord()
        {
            PLR.LDA.Execute("InsertNTISFCFEDRunRecord", DataAccessHelper.Database.Cls);
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetLetterIdFromLetter")]
        public string GetLetterIdFromLetterPath(string letter, string accountNumber)
        {
            return PLR.LDA.ExecuteSingle<string>("GetLetterIdFromLetter", DataAccessHelper.Database.ECorrFed, SqlParams.Single("Letter", letter), SqlParams.Single("AccountNumber", accountNumber)).Result;
        }

        /// <summary>
        /// Gets the return information From CLS
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cls, "GetNTISReturnInfo")]
        public string GetResponseInfo()
        {
            ReturnInfo ri = PLR.LDA.ExecuteSingle<ReturnInfo>("GetNTISReturnInfo", DataAccessHelper.Database.Cls).Result;
            return $"{ri.ReturnName},{ri.AddressLine1},{ri.AddressLine2},{ri.AddressLine3},{ri.City},{ri.State},{ri.ReturnZip},{ri.ReturnZipAddOn}";
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "GetRunHistoryForNTISFCFED")]
        public int GetRunNumber()
        {
            return PLR.LDA.ExecuteSingle<int>("GetRunHistoryForNTISFCFED", DataAccessHelper.Database.Cls).Result;
        }

    }
}
