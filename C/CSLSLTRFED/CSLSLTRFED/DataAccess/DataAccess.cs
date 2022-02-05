using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CSLSLTRFED
{
    public class DataAccess
    {
        LogDataAccess LDA { get; set; }
        ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "cslsltrfed.GetLetters")]
        public List<LetterData> GetLetterData()
        {
            return LDA.ExecuteList<LetterData>("cslsltrfed.GetLetters", DataAccessHelper.Database.Cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "cslsltrfed.GetFileHeader")]
        public string GetFileHeader(string letter, string scriptId)
        {
            return LDA.ExecuteSingle<string>("cslsltrfed.GetFileHeader", DataAccessHelper.Database.Cls, SP("Letter", letter), SP("ScriptId", scriptId)).Result;
        }
        
        public DataTable ExecuteSproc(string accountNumber, string sprocName, bool isEndorser)
        {
            DataTable dt = LDA.ExecuteDataTable(sprocName, DataAccessHelper.Database.Cls, true,
                SP("AccountNumber", accountNumber),
                SP("IsCoborrower", isEndorser)).Result;

            if (dt == null || dt.Rows.Count == 0)
            {
                string message = $"Unable to determine Loan Detail information for {accountNumber}; Sproc: {sprocName}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return dt;
        }

        /// <summary>
        /// Sends in the account number/ssn provided by the user and returns the account number if it is found. This verifies the
        /// borrower is in the warehouse and returns the account number if ssn is provided.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cls, "cslsltrfed.GetAccountNumber")]
        public string GetAccountNumber(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("cslsltrfed.GetAccountNumber", DataAccessHelper.Database.Cls,
                SP("AccountNumber", accountNumber)).Result;
        }

        /// <summary>
        /// Gets the cost center for the letter
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetBUCostCenterByLetter")]
        public CostCenter GetCostCenter(string letterId)
        {
            return LDA.ExecuteSingle<CostCenter>("spGetBUCostCenterByLetter", DataAccessHelper.Database.Bsys,
                SP("LetterName", letterId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "cslsltrfed.GetLetterNameFromId")]
        public string GetLetterName(string letterId)
        {
            return LDA.ExecuteSingle<string>("cslsltrfed.GetLetterNameFromId", DataAccessHelper.Database.Bsys,
                SP("ID", letterId)).Result;
        }

        /// <summary>
        /// Pulls back a list of co-borrowers an the loan sequence that ties them to the borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "cslsltrfed.GetCoborrowers")]
        public List<Endorsers> GetCoborrowers(string ssn)
        {
            return LDA.ExecuteList<Endorsers>("cslsltrfed.GetCoborrowers", DataAccessHelper.Database.Cls,
                SP("BF_SSN", ssn)).Result;
        }

        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}