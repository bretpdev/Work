using System;
using System.Data.Linq;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Collections.Generic;

namespace ACHSETUPFD
{
    public class DataAccess
    {

        ProcessLogRun PLR { get; set; }

        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
        }

        /// <summary>
        /// Adds the bank account information to the database for check by phone
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "spAddCheckByPhoneEntry")]
        public void AddEntryToDB(OPSEntry data)
        {
            PLR.LDA.Execute("spAddCheckByPhoneEntry", DataAccessHelper.Database.Cls,
                new SqlParameter("AccountNumber", data.AccountNumber),
                new SqlParameter("RoutingNumber", data.RoutingNumber),
                new SqlParameter("BankAccountNumber", data.BankAccountNumber),
                new SqlParameter("AccountType", data.CalculatedAccountType),
                new SqlParameter("PaymentAmount", data.PaymentAmount),
                new SqlParameter("AccountHolderName", data.AccountHolderName),
                new SqlParameter("EffectiveDate", DateTime.Parse(data.EffectiveDate).ToString("MM/dd/yyyy")),
                new SqlParameter("AcctStoreAuthorization", true),
                new SqlParameter("DataSource", "ACHSETUPFD"));
        }

        /// <summary>
        /// Checks to see if a value is valid or not
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_CheckForInvalidValues")]
        public bool IsInvalidValue(string valueKey, string theValue)
        {
            return PLR.LDA.ExecuteSingle<int>("spGENR_CheckForInvalidValues", DataAccessHelper.Database.Csys,
                new SqlParameter("ValueKey", valueKey),
                new SqlParameter("Value", theValue)).Result > 0;
        }

        /// <summary>
        /// Gets Endorsers for the specified borrower or returns an empty list
        /// </summary>
        /// <return>Non-null List<DataClasses.EndorserRecord></return>
        [UsesSproc(DataAccessHelper.Database.Cdw, "dbo.GetEndorsers")]
        public List<DataClasses.EndorserRecord> GetEndorsers(string SSN)
        {
            return PLR.LDA.ExecuteList<DataClasses.EndorserRecord>("dbo.GetEndorsers", DataAccessHelper.Database.Cdw,
                SqlParams.Single("SSN", SSN)).Result ?? new List<DataClasses.EndorserRecord>();
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "dbo.spGetAccountNumberFromSsn")]
        public string GetAccountNumberFromSsn(string Ssn)
        {
            return PLR.LDA.ExecuteSingle<string>("dbo.spGetAccountNumberFromSsn", DataAccessHelper.Database.Cdw,
                SqlParams.Single("SSN", Ssn)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "[achsetupfd].GetCoBorrowerForBorrowerByLoan")]
        public List<CoborrowerDemographics> GetCoBorrowersForBorrowerByLoan(string borrowerSSN)
        {
            return PLR.LDA.ExecuteList<CoborrowerDemographics>("[achsetupfd].GetCoBorrowersForBorrowerByLoan", DataAccessHelper.Database.Cdw, SqlParams.Single("BorrowerSSN", borrowerSSN)).Result;
        }


    }
}