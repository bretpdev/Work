using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace RTRNMAILOL
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
            LDA = logRun.LDA;
        }

        [UsesSproc(Ols, "rtrnmailol.GetRecordsToProcess")]
        public List<BarcodeData> GetRecordsToProcess() => 
            LDA.ExecuteList<BarcodeData>("rtrnmailol.GetRecordsToProcess", Ols).Result;

        /// <summary>
        /// Gets the demographic information that matches the System Borrower Demographics object
        /// </summary>
        [UsesSproc(Odw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetDemos(string accountIdentifier) => LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", Odw,
                SP("AccountIdentifier", accountIdentifier)).Result;

        /// <summary>
        /// Checks if the borrower is in California and returns the email information to add them to the Email Batch Script if they are
        /// </summary>
        [UsesSproc(Odw, "rtrnmailol.GetCaliforniaBorForEnd")]
        [UsesSproc(Odw, "rtrnmailol.GetCaliforniaBorrower")]
        [UsesSproc(Odw, "rtrnmailol.GetCaliforniaEndForBor")]
        [UsesSproc(Odw, "rtrnmailol.GetCaliforniaEndorser")]
        public List<BorrowerEmailData> GetCAEmailData(string accountIdentifier)
        {
            List<BorrowerEmailData> emailData = new List<BorrowerEmailData>();

            //Assume the accountNumber belongs to a borrower
            emailData = LDA.ExecuteList<BorrowerEmailData>("rtrnmailol.GetCaliforniaBorrower", Odw,
                SP("AccountIdentifier", accountIdentifier)).Result;

            //If the accountNumber belongs to an endorser
            if (emailData == null || emailData.Count == 0)
            {
                emailData = LDA.ExecuteList<BorrowerEmailData>("rtrnmailol.GetCaliforniaBorForEnd", Odw,
                    SP("AccountIdentifier", accountIdentifier)).Result;

                emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("rtrnmailol.GetCaliforniaEndorser", Odw,
                    SP("AccountIdentifier", accountIdentifier)).Result);
            }

            //If the account number was indeed the borrower's
            else
            {
                emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("rtrnmailol.GetCaliforniaEndForBor", Odw,
                    SP("AccountIdentifier", accountIdentifier)).Result);
            }

            return new List<BorrowerEmailData>(emailData.Where(b => b.State == "CA"));
        }

        /// <summary>
        /// Sets the record to processed in the BD
        /// </summary>
        [UsesSproc(Ols, "rtrnmailol.MarkBarcodeRecordCompleted")]
        public void SetProcessed(int barcodeDataId) => LDA.Execute("rtrnmailol.MarkBarcodeRecordCompleted", Ols,
            SP("BarcodeDataId", barcodeDataId));

        /// <summary>
        /// Inserts the ArcAddProcessingId for the BarcodeData record
        /// </summary>
        [UsesSproc(Ols, "rtrnmailol.InsertArcAddId")]
        public void InsertArcAddId(int barcodeDataId, int arcAddProcessingId)
        {
            LDA.Execute("rtrnmailol.InsertArcAddId", Ols,
                SP("BarcodeDataId", barcodeDataId),
                SP("ArcAddProcessingId", arcAddProcessingId));
        }

        /// <summary>
        /// Inserts the Email Batch Script record for the California borrower
        /// </summary>
        [UsesSproc(Ols, "rtrnmailol.GetCampaignIdFromHtml")]
        [UsesSproc(Uls, "emailbatch.LoadSingleRecord")]
        public bool InsertEmailBatch(BorrowerEmailData eData, bool isEndorser, string accountNumber)
        {
            try
            {
                int id = LDA.ExecuteSingle<int>("rtrnmailol.GetCampaignIdFromHtml", Ols,
                    SP("HtmlFile", "RTRNMLEML.html"),
                    SP("IsEndorser", isEndorser)).Result;

                LDA.Execute("emailbatch.LoadSingleRecord", Uls,
                    SP("EmailCampaignId", id),
                    SP("AccountNumber", accountNumber),
                    SP("EmailData", eData.EmailData),
                    SP("ArcNeeded", !isEndorser));
                return true;
            }
            catch (Exception ex)
            {
                string message = $"There was an error adding the California borrower record to the Email Batch table for {(isEndorser ? "Endorser:" : "Borrower:")} {eData.AccountNumber}. A comment will not be added to the ArcAddProcessing table.; Error: {ex.Message}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the Business Unit number by searching for Document Services then
        /// gets the BU Manager email address
        /// </summary>
        [UsesSproc(Csys, "spGENR_GetBusinessUnitId")]
        [UsesSproc(Csys, "spNDHP_GetManagerEmail")]
        public string GetDocServicesManagerEmail()
        {
            int BuId = LDA.ExecuteSingle<int>("spGENR_GetBusinessUnitId", Csys,
                SP("BU", "Document Services")).Result;
            string userName = LDA.ExecuteSingle<string>("spNDHP_GetManagerEmail", Csys,
                SP("BusinessUnit", BuId)).Result;
            return userName + "@utahsbr.edu";
        }

        private SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}