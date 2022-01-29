using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BARCODEFED
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetStateCodes")]
        public List<string> GetStateCodes()
        {
            return LDA.ExecuteList<string>("GetStateCodes", DataAccessHelper.Database.Bsys).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "LTDB_GetResendMail")]
        public bool LetterIsRequired(string letterId)
        {
            return LDA.ExecuteSingle<bool>("LTDB_GetResendMail", DataAccessHelper.Database.Bsys,
                Sp("LetterId", letterId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "LTDB_GetLetterIds")]
        public List<string> GetLetterIds()
        {
            return LDA.ExecuteList<string>("LTDB_GetLetterIds", DataAccessHelper.Database.Bsys,
                Sp("Region", "Fed")).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetSSNFromAcctNumber")]
        public string GetSsnFromAcct(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw,
                Sp("AccountNumber", accountNumber)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spMD_GetDemographicData")]
        public WarehouseDemographics GetDemographicsFromWarehouse(string accountNumber)
        {
            return LDA.ExecuteSingle<WarehouseDemographics>("spMD_GetDemographicData", DataAccessHelper.Database.Cdw,
                Sp("AccountNumber", accountNumber)).Result;
        }

        /// <summary>
        /// Check if the borrower is in the Cornerstone region
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "[barcodefed].CheckBorrowerExists")]
        public bool CheckBorrowerRegion(string accountNumber)
        {
            return LDA.ExecuteSingle<string>("[barcodefed].CheckBorrowerExists", DataAccessHelper.Database.Cdw,
                Sp("AccountNumber", accountNumber)).Result.IsPopulated();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[barcodefed].GetRecords")]
        public List<ForwardingInfo> GetRecordsToProcess()
        {
            return LDA.ExecuteList<ForwardingInfo>("[barcodefed].GetRecords", DataAccessHelper.Database.Cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[barcodefed].AddRecord")]
        public bool SaveScannedInfo(ForwardingInfo forwardingInfo)
        {
            try
            {
                LDA.Execute("[barcodefed].AddRecord", DataAccessHelper.Database.Cls,
                    Sp("RecipientId", forwardingInfo.RecipientId),
                    Sp("LetterId", forwardingInfo.LetterId),
                    Sp("CreateDate", forwardingInfo.CreateDate),
                    Sp("Address1", forwardingInfo.Address1.IsPopulated() ? forwardingInfo.Address1 : null),
                    Sp("Address2", forwardingInfo.Address2.IsPopulated() ? forwardingInfo.Address2 : null),
                    Sp("City", forwardingInfo.City.IsPopulated() ? forwardingInfo.City : null),
                    Sp("State", forwardingInfo.State.IsPopulated() ? forwardingInfo.State : null),
                    Sp("Zip", forwardingInfo.Zip.IsPopulated() ? forwardingInfo.Zip : null),
                    Sp("Country", forwardingInfo.Country.IsPopulated() ? forwardingInfo.Country : null),
                    Sp("AddedBy", Environment.UserName),
                    Sp("BorrowerSsn", forwardingInfo.BorrowerSsn.IsPopulated() ? forwardingInfo.BorrowerSsn : null),
                    Sp("PersonType", forwardingInfo.PersonType.IsPopulated() ? forwardingInfo.PersonType : null),
                    Sp("ReceivedDate", forwardingInfo.ReceivedDate));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PRIMARY KEY constraint"))
                {
                    MessageBox.Show("The record you are trying to add is already in the Database.", "Record exists", MessageBoxButtons.OK);
                    return false;
                }
                else if (ex.Message.Contains("UNIQUE"))
                {
                    Dialog.Error.Ok("The record you are trying to add is already in the database", "Record exists");
                    return false;
                }
                else
                {
                    throw ex;
                }
            }
            return true;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[barcodefed].[SetRecordProcessed]")]
        public void SetProcessed(int returnMailId)
        {
            LDA.Execute("[barcodefed].[SetRecordProcessed]", DataAccessHelper.Database.Cls,
                Sp("ReturnMailId", returnMailId));
        }

        /// <summary>
        /// If the borrower is in CA, add a record to the Email Campaign batch script to send them an email
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "barcodefed.GetCampaignIdFromLetter")]
        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.UploadCampaignDataSingle")]
        public void InsertEmailBatch(List<EmailData> eData, bool isBorrower)
        {
            int id = LDA.ExecuteSingle<int>("barcodefed.GetCampaignIdFromLetter", DataAccessHelper.Database.Cls,
                Sp("LetterId", "RETMAILFED.html"),
                Sp("IsEndorser", !isBorrower)).Result;

            foreach (EmailData e in eData)
            {
                LDA.Execute("emailbtcf.UploadCampaignDataSingle", DataAccessHelper.Database.Cls,
                    Sp("EmailCampaignId", id),
                    Sp("Recipient", e.Recipient),
                    Sp("AccountNumber", e.AccountNumber),
                    Sp("FirstName", e.FirstName),
                    Sp("LastName", e.LastName));
            }
        }

        /// <summary>
        /// Pulls back everyone associated with the account identifier
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "barcodefed.GetAssociatedAccounts")]
        public List<AssociatedAccounts> GetAssociatedAccounts(string accountIdentifier)
        {
            return LDA.ExecuteList<AssociatedAccounts>("barcodefed.GetAssociatedAccounts", DataAccessHelper.Database.Cdw, 
                Sp("AccountIdentifier", accountIdentifier)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "GetReturnMailReasons")]
        public List<string> GetReasons()
        {
            return LDA.ExecuteList<string>("GetReturnMailReasons", DataAccessHelper.Database.Csys).Result;
        }

        public SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }

        /// <summary>
        /// Checks if the borrower is in California and returns the email information to add them to the Email Batch Script if they are
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "barcodefed.GetCaliforniaBorrower")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "barcodefed.GetCaliforniaBorForEnd")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "barcodefed.GetCaliforniaEndorser")]
        [UsesSproc(DataAccessHelper.Database.Cdw, "barcodefed.GetCaliforniaEndForBor")]
        public List<EmailData> GetCAEmailData(string accountIdentifier, bool isBorrower)
        {
            List<EmailData> emailData = new List<EmailData>();
            if (!isBorrower)
            {
                emailData.AddRange(LDA.ExecuteList<EmailData>("barcodefed.GetCaliforniaBorForEnd", DataAccessHelper.Database.Cdw,
                    Sp("AccountNumber", accountIdentifier)).Result);
                emailData.AddRange(LDA.ExecuteList<EmailData>("barcodefed.GetCaliforniaEndorser", DataAccessHelper.Database.Cdw,
                    Sp("AccountNumber", accountIdentifier)).Result);
            }
            else
            {
                emailData.AddRange(LDA.ExecuteList<EmailData>("barcodefed.GetCaliforniaBorrower", DataAccessHelper.Database.Cdw,
                    Sp("AccountNumber", accountIdentifier)).Result);
                emailData.AddRange(LDA.ExecuteList<EmailData>("barcodefed.GetCaliforniaEndForBor", DataAccessHelper.Database.Cdw,
                        Sp("AccountNumber", accountIdentifier)).Result);
            }
            return new List<EmailData>(emailData.Where(p => p.State == "CA").Distinct());
        }

    }
}