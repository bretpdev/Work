using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace BCSRETMAIL
{
    public class DataAccess
    {
        public LogDataAccess OLDA { get; set; }
        public LogDataAccess ULDA { get; set; }
        //In order to catch exception, LDA needs to turn off repeater and managed connection
        public LogDataAccess OlsNoRepeat { get; set; }
        public LogDataAccess UlsNoRepeat { get; set; }
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            OLDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false, DataAccessHelper.Region.Uheaa);
            OlsNoRepeat = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false, DataAccessHelper.Region.Uheaa);
            ULDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, false, DataAccessHelper.Region.Uheaa);
            UlsNoRepeat = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, false, false, DataAccessHelper.Region.Uheaa);
            LDA = logRun.LDA;
        }

        /// <summary>
        /// Inserts data that is scanned using the barcode scanning wand. Returns true if it was successful, false if it was not successfull and null if it was unique.
        /// </summary>
        [UsesSproc(Ols, "rtrnmailol.SaveScannedInfo")]
        [UsesSproc(Uls, "rtrnmailuh.SaveScannedInfo")]
        public List<ManagedDataResult<object>> SaveScannedInfo(BarcodeInfo barcodeInfo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                SP("RecipientId", barcodeInfo.RecipientId.Trim()),
                SP("LetterId", barcodeInfo.LetterId),
                SP("CreateDate", barcodeInfo.CreateDate),
                SP("ReceivedDate", barcodeInfo.ReceivedDate),
                SP("Address1", barcodeInfo.Address1),
                SP("Address2", barcodeInfo.Address2),
                SP("City", barcodeInfo.City),
                SP("State", barcodeInfo.State),
                SP("ZipCode", barcodeInfo.ZipCode),
                SP("Comment", barcodeInfo.Comment)
            };
            List<ManagedDataResult<object>> results = new List<ManagedDataResult<object>>();
            //Return a ManagedDataResult to check for unique constraint error
            if (barcodeInfo.System == BarcodeInfo.SystemType.Compass || barcodeInfo.System == BarcodeInfo.SystemType.Both)
                results.Add(UlsNoRepeat.ExecuteSingle<object>("rtrnmailuh.SaveScannedInfo", Uls, parameters.ToArray()));
            if (barcodeInfo.System == BarcodeInfo.SystemType.Onelink || barcodeInfo.System == BarcodeInfo.SystemType.Both)
                results.Add(OlsNoRepeat.ExecuteSingle<object>("rtrnmailol.SaveScannedInfo", Ols, parameters.ToArray()));
            if (results.Count == 0)
                results.Add(new ManagedDataResult<object>() { DatabaseCallSuccessful = false });
            return results;
        }

        /// <summary>
        /// Gets a list of all processed records from all databases
        /// </summary>
        [UsesSproc(Ols, "rtrnmailol.GetProcessedRecords")]
        [UsesSproc(Uls, "rtrnmailuh.GetProcessedRecords")]
        public List<ProcessedDocuments> GetProcessedDocuments(DateTime selectedDate)
        {
            List<ProcessedDocuments> documents = new List<ProcessedDocuments>();
            documents.AddRange(OLDA.ExecuteList<ProcessedDocuments>("rtrnmailol.GetProcessedRecords", Ols, SP("SelectedDate", selectedDate)).Result);
            documents.AddRange(ULDA.ExecuteList<ProcessedDocuments>("rtrnmailuh.GetProcessedRecords", Uls, SP("SelectedDate", selectedDate)).Result);
            return documents;
        }

        /// <summary>
        /// Gets the return reasons from CSYS
        /// </summary>
        [UsesSproc(Csys, "GetReturnMailReasons")]
        public List<string> GetReturnReasons() => LDA.ExecuteList<string>("GetReturnMailReasons", Csys).Result;

        /// <summary>
        /// Gets all the available letter ids for both regions
        /// </summary>
        /// <returns></returns>
        [UsesSproc(Bsys, "bcsretmail.GetLetterIds")]
        public List<string> GetLetterIds() =>
            ULDA.ExecuteList<string>("bcsretmail.GetLetterIds", Bsys).Result;

        /// <summary>
        /// Gets the earliest processed record for all regions
        /// </summary>
        [UsesSproc(Ols, "rtrnmailol.GetEarliestDate")]
        [UsesSproc(Uls, "rtrnmailuh.GetEarliestDate")]
        public DateTime GetEarliestDate()
        {
            DateTime? olsDate = OLDA.ExecuteSingle<DateTime?>("rtrnmailol.GetEarliestDate", Ols).Result;
            DateTime? ulsDate = ULDA.ExecuteSingle<DateTime?>("rtrnmailuh.GetEarliestDate", Uls).Result;

            DateTime?[] dates = new DateTime?[] { olsDate, ulsDate, DateTime.Now.Date };
            return dates.Min() ?? DateTime.Now.Date;
        }

        /// <summary>
        /// Checks to see if the borrower/Endorser has a balance and returns true if there are open loans
        /// </summary>
        [UsesSproc(Udw, "GetOpenLoanBalance")]
        public bool CheckForOpenLoans(string accountNumber)
        {
            decimal? balance = ULDA.ExecuteSingle<decimal?>("GetOpenLoanBalance", Udw,
                SP("AccountIdentifier", accountNumber)).Result;

            if (balance.HasValue && balance.Value > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Checks each region to see if the borrower exists. Checks the system (onelink,compass) is the unit is supplied otherwise it checks all
        /// </summary>
        /// <returns></returns>
        [UsesSproc(Udw, "GetSystemBorrowerDemographics")]
        [UsesSproc(Odw, "GetSystemBorrowerDemographics")]
        [UsesSproc(Odw, "GetReferenceDemos")]
        public List<Account> GetAccountIdentifier(string accountIdentifier)
        {
            List<Account> accounts = new List<Account>();
            Account acct;
            acct = ULDA.ExecuteSingle<Account>("GetSystemBorrowerDemographics", Udw, SP("AccountIdentifier", accountIdentifier)).Result;
            if (acct != null && acct.AccountNumber.IsPopulated())
            {
                acct.System = BarcodeInfo.SystemType.Compass;
                acct.Region = DataAccessHelper.Region.Uheaa;
                accounts.Add(acct);
            }
            if (accountIdentifier.ToUpper().StartsWith("RF@"))
                acct = OLDA.ExecuteSingle<Account>("GetReferenceDemos", Odw, SP("AccountIdentifier", accountIdentifier)).Result;
            else
                acct = OLDA.ExecuteSingle<Account>("GetSystemBorrowerDemographics", Odw, SP("AccountIdentifier", accountIdentifier)).Result;

            if (acct != null && acct.AccountNumber.IsPopulated())
            {
                acct.System = BarcodeInfo.SystemType.Onelink;
                acct.Region = DataAccessHelper.Region.Uheaa;
                accounts.Add(acct);
            }
            return accounts;
        }

        /// <summary>
        /// Gets a list of state codes
        /// </summary>
        [UsesSproc(Bsys, "GetStateCodes")]
        public List<string> GetStateCodes() => ULDA.ExecuteList<string>("GetStateCodes", Bsys).Result;

        public SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}