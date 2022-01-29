using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MANMAIL
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "LTDB_GetLetterIds")]
        public List<string> GetLetterIds()
        {
            return LDA.ExecuteList<string>("LTDB_GetLetterIds", DataAccessHelper.Database.Bsys).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "GetReturnMailReasons")]
        public List<string> GetReasons()
        {
            return LDA.ExecuteList<string>("GetReturnMailReasons", DataAccessHelper.Database.Csys).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetStateCodes")]
        public List<string> GetStates()
        {
            return LDA.ExecuteList<string>("spGENR_GetStateCodes", DataAccessHelper.Database.Csys,
                Sp("IncludeTerritories", true)).Result;
        }

        /// <summary>
        /// Checks if the borrower is in California and returns the email information to add them to the Email Batch Script if they are
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Odw, "bcsretmail.GetCaliforniaBorrower")]
        [UsesSproc(DataAccessHelper.Database.Odw, "bcsretmail.GetCaliforniaBorForEnd")]
        [UsesSproc(DataAccessHelper.Database.Odw, "bcsretmail.GetCaliforniaEndorser")]
        [UsesSproc(DataAccessHelper.Database.Odw, "bcsretmail.GetCaliforniaEndForBor")]
        [UsesSproc(DataAccessHelper.Database.Udw, "bcsretmail.GetCaliforniaBorrower")]
        [UsesSproc(DataAccessHelper.Database.Udw, "bcsretmail.GetCaliforniaBorForEnd")]
        [UsesSproc(DataAccessHelper.Database.Udw, "bcsretmail.GetCaliforniaEndorser")]
        [UsesSproc(DataAccessHelper.Database.Udw, "bcsretmail.GetCaliforniaEndForBor")]
        public List<BorrowerEmailData> GetCAEmailData(string accountNumber, ManualReturnMail.AddressRegion invalidatedOn, bool isBorrower)
        {
            //Will not return results for when Address region is not compass or onelink, becasue of single region endorsers in a cross region situation
            //region.both can not be used to get emailing information
            List<BorrowerEmailData> emailData = new List<BorrowerEmailData>();
            if (invalidatedOn == ManualReturnMail.AddressRegion.Onelink)
            {
                if (!isBorrower)
                {
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaBorForEnd", DataAccessHelper.Database.Odw,
                        Sp("AccountNumber", accountNumber)).Result);
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaEndorser", DataAccessHelper.Database.Odw,
                        Sp("AccountNumber", accountNumber)).Result);
                }
                else
                {
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaBorrower", DataAccessHelper.Database.Odw,
                        Sp("AccountNumber", accountNumber)).Result);
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaEndForBor", DataAccessHelper.Database.Odw,
                        Sp("AccountNumber", accountNumber)).Result);
                }
            }
            if (invalidatedOn == ManualReturnMail.AddressRegion.Compass)
            {
                if (!isBorrower)
                {
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaBorForEnd", DataAccessHelper.Database.Udw,
                        Sp("AccountNumber", accountNumber)).Result);
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaEndorser", DataAccessHelper.Database.Udw,
                        Sp("AccountNumber", accountNumber)).Result);
                }
                else
                {
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaBorrower", DataAccessHelper.Database.Udw,
                        Sp("AccountNumber", accountNumber)).Result);
                    emailData.AddRange(LDA.ExecuteList<BorrowerEmailData>("bcsretmail.GetCaliforniaEndForBor", DataAccessHelper.Database.Udw,
                            Sp("AccountNumber", accountNumber)).Result);
                }
            }

            //if (emailData.Any(p => p.Priority == 0) && (emailData.Any(p => p.Priority == 1) || emailData.Any(p => p.Priority == 3)))
            //    return emailData.Where(p => p.Priority == 0 || p.Priority == 2).ToList();

            return new List<BorrowerEmailData>(emailData.Where(p => p.State == "CA").Distinct());
        }

        /// <summary>
        /// Inserts the Email Batch Script record for the California borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "manmail.GetCampaignIdFromHtml")]
        [UsesSproc(DataAccessHelper.Database.Uls, "emailbatch.LoadSingleRecord")]
        public void InsertEmailBatch(BorrowerEmailData eData, string htmlFile, bool isEndorser)
        {
            int id = LDA.ExecuteSingle<int>("manmail.GetCampaignIdFromHtml", DataAccessHelper.Database.Uls,
                Sp("HtmlFile", htmlFile),
                Sp("IsEndorser", isEndorser)).Result;

            LDA.Execute("emailbatch.LoadSingleRecord", DataAccessHelper.Database.Uls,
                Sp("EmailCampaignId", id),
                Sp("AccountNumber", eData.AccountNumber),
                Sp("EmailData", eData.EmailData),
                Sp("ArcNeeded", eData.ArcNeeded));
        }

        /// <summary>
        /// Gets the business unit associated with a letter to determine what correspondence to send to CA borrowers and coborrowers
        /// 0 -> onelink
        /// 1 -> compass
        /// 2 -> both
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Bsys, "bcsretmail.GetUnitForLetter")]
        public int GetUnitForLetter(string LetterId)
        {
            return LDA.ExecuteSingle<int>("bcsretmail.GetUnitForLetter", DataAccessHelper.Database.Bsys, SqlParams.Single("LetterId", LetterId)).Result;
        }

        /// <summary>
        /// Pulls back everyone associated with the account identifier
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "manmail.GetAssociatedAccounts")]
        public List<AssociatedAccounts> GetAssociatedAccounts(string accountIdentifer)
        {
            List<AssociatedAccounts> accounts = new List<AssociatedAccounts>();
            accounts.AddRange(LDA.ExecuteList<AssociatedAccounts>("manmail.GetAssociatedAccounts", DataAccessHelper.Database.Udw,
                Sp("AccountIdentifier", accountIdentifer)).Result);
            accounts.AddRange(LDA.ExecuteList<AssociatedAccounts>("manmail.GetAssociatedAccounts", DataAccessHelper.Database.Odw,
                Sp("AccountIdentifier", accountIdentifer)).Result);
            return accounts.Distinct().ToList();
        }


        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }


    }
}