using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LSLETTERSU
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        /// <summary>
        /// Gets all the letter data for all letter types
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "lslettersu.GetLetters")]
        public List<LetterData> GetLetterData() => LogRun.LDA.ExecuteList<LetterData>("lslettersu.GetLetters", DataAccessHelper.Database.Uls).Result;

        public List<string> CollectRoles(string scriptId) => LogRun.LDA.ExecuteList<string>("GetScriptRoles", DataAccessHelper.Database.CentralData,
                SP("ScriptId", scriptId)).Result;

        /// <summary>
        /// Executes a stored procedure by name that is pulled from a table
        /// </summary>
        public DataTable ExecuteSproc(string accountNumber, string sprocName, bool isEndorser)
        {
            DataTable dt = LogRun.LDA.ExecuteDataTable(sprocName, DataAccessHelper.Database.Uls, true,
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
        [UsesSproc(DataAccessHelper.Database.Uls, "lslettersu.GetAccountNumber")]
        public string GetAccountNumber(string accountNumber) => LogRun.LDA.ExecuteSingle<string>("lslettersu.GetAccountNumber", DataAccessHelper.Database.Uls,
                SP("AccountNumber", accountNumber)).Result;

        /// <summary>
        /// Gets the cost center for the letter
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "spGetBUCostCenterByLetter")]
        public CostCenter GetCostCenter(string letterId) => LogRun.LDA.ExecuteSingle<CostCenter>("spGetBUCostCenterByLetter", DataAccessHelper.Database.Bsys,
                SP("LetterName", letterId)).Result;

        /// <summary>
        /// Gets the letter name from BSYS for each letter id
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "GetLetterNameFromId")]
        public string GetLetterName(string letterId) => LogRun.LDA.ExecuteSingle<string>("GetLetterNameFromId", DataAccessHelper.Database.Bsys,
                SP("ID", letterId)).Result;

        /// <summary>
        /// Pulls back a list of co-borrowers and the loan sequence that ties them to the borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "lslettersu.GetCoborrowers")]
        public List<Endorsers> GetCoborrowers(string ssn) => LogRun.LDA.ExecuteList<Endorsers>("lslettersu.GetCoborrowers", DataAccessHelper.Database.Uls,
                SP("BF_SSN", ssn)).Result;

        /// <summary>
        /// Gets the demographic information that matches the System Borrower Demographics object
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetDemos(string accountNumber) => LogRun.LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", DataAccessHelper.Database.Udw,
                SP("AccountIdentifier", accountNumber)).Result;

        /// <summary>
        /// Gets all the potential merge fields for the 
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "lslettersu.GetMergeFields")]
        public List<MergeFields> GetMergeFields(int loanServicingLettersId) => LogRun.LDA.ExecuteList<MergeFields>("lslettersu.GetMergeFields", DataAccessHelper.Database.Uls,
                SP("LoanServicingLettersId", loanServicingLettersId)).Result;

        public SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}