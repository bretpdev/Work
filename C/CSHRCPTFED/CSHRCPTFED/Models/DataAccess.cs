using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Linq;
using CSHRCPTFED.Infrastructure;
using System.Data.SqlClient;

namespace CSHRCPTFED
{
    public class DataAccess
    {
        private LogDataAccess LDA;
        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }



        [UsesSproc(DataAccessHelper.Database.Cls, "[cshrcptfed].[AddCashRcptRecord]")]
        public int? LoadRecord(string account, string borrower, decimal amount, string check, Payee payee, DateTime date, int? arcid, string username)
        {
            int? eax = LDA.ExecuteIdNullable<int>("[cshrcptfed].[AddCashRcptRecord]", DataAccessHelper.Database.Cls, 
                SqlParams.Single("Account", account),
                SqlParams.Single("Borrower", borrower),
                SqlParams.Single("Amount", amount),
                SqlParams.Single("Check", check),
                SqlParams.Single("Payee", payee),
                SqlParams.Single("Date", date),
                SqlParams.Single("ArcId", arcid),
                SqlParams.Single("WindowsUser", username));

            return eax;
        }

        [UsesSproc(DataAccessHelper.Database.CentralData, "[dbo].[GetScriptRoles]")]
        public List<string> CollectRoles(string script)
        {
            return LDA.ExecuteList<string>("[dbo].[GetScriptRoles]", DataAccessHelper.Database.CentralData,
                SqlParams.Single("ScriptId", script)).Result;

        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[cshrcptfed].[GetAccountIdentifiers]")]
        public AccountIdentifiers AccountNumber(string accountIdentifier)
        {
            return LDA.ExecuteSingle<AccountIdentifiers>("[cshrcptfed].[GetAccountIdentifiers]", DataAccessHelper.Database.Cls,
                SqlParams.Single("AccountIdentifier", accountIdentifier)).Result;

        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[cshrcptfed].[CheckAlreadyExists]")]
        public ExistingCheckData FindExistingCheck(string accountNumber, string checkNumber, DateTime date, Payee payee)
        {
            return LDA.ExecuteList<ExistingCheckData>("[cshrcptfed].[FindExistingCheck]", DataAccessHelper.Database.Cls,
                Sp("AccountNumber", accountNumber), Sp("CheckNumber", checkNumber), Sp("Date", date), Sp("Payee", payee)).Result.SingleOrDefault();

        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[cshrcptfed].[GetBorrowerName]")]
        public string GetBorrowerName(string accountIdentifier)
        {
            return LDA.ExecuteList<string>("[cshrcptfed].[GetBorrowerName]", DataAccessHelper.Database.Cls,
                SqlParams.Single("AccountIdentifier", accountIdentifier)).Result.FirstOrDefault();

        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}