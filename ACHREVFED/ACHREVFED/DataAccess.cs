using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACHREVFED
{
    public class DataAccess
    {
        LogDataAccess LDA { get; set; }
        public DataAccess(LogDataAccess lda)
        {
            this.LDA = lda;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "achrevfed.[AchReviewAddWork]")]
        public void AddNewWork()
        {
            LDA.Execute("achrevfed.[AchReviewAddWork]", DataAccessHelper.Database.Cls, int.MaxValue);
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "achrevfed.[AchReviewGetUnprocessedAccounts]")]
        public List<ACHData> GetAccounts()
        {
            return LDA.ExecuteList<ACHData>("achrevfed.[AchReviewGetUnprocessedAccounts]", DataAccessHelper.Database.Cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "achrevfed.ACHReviewMarkAccountProcessed")]
        public void MarkAccountProcessed(string accountNumber)
        {
            LDA.Execute("achrevfed.ACHReviewMarkAccountProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("AccountNumber", accountNumber));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "achrevfed.[CheckOnEcorr]")]
        public bool CheckOnEcorr(string accountNumber)
        {
            return LDA.ExecuteSingle<bool>("achrevfed.[CheckOnEcorr]", DataAccessHelper.Database.Cls, SqlParams.Single("AccountNumber", accountNumber)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "LT_Header")]
        public AddressInfo GetBorrowersAddress(string account, bool isCoBorrower)
        {
            //Don't pass isCoBorrower to sproc because it will not return results because of a join on LT20
            //We just want the demographic info for an account number, so just use the procedure like the account number is a borrower
            return LDA.ExecuteSingle<AddressInfo>("LT_Header", DataAccessHelper.Database.Cdw, SqlParams.Single("AccountNumber", account)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "spGetAccountNumberFromSsn")]
        public string GetAccountNumberFromSsn(string ssn)
        {
            return LDA.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Cdw, SqlParams.Single("SSN", ssn)).Result;
        }



    }
}
