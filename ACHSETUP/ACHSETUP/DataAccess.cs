using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace ACHSETUP
{
    class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun PLR)
        {
            LDA = PLR.LDA;
        }

        [UsesSproc(Norad, "dbo.spCKPH_AddCheckByPhoneEntry")]
        public void AddEntryToDB(OPSEntry data)
        {
            LDA.Execute("dbo.spCKPH_AddCheckByPhoneEntry", Norad,
                SP("SSN", data.SSN.ToString()),
                SP("NAME", data.FullName),
                SP("DOB", data.DOB),
                SP("ABA", data.RoutingNumber),
                SP("BANKACCOUNTNUMBER", data.BankAccountNumber),
                SP("ACCOUNTTYPE", data.CalculatedAccountType),
                SP("AMOUNT", data.PaymentAmount),
                SP("EFFECTIVEDATE", DateTime.Parse(data.EffectiveDate).ToString("MM/dd/yyyy")),
                SP("ACCOUNTHOLDERNAME", data.AccountHolderName),
                SP("DataSource", "ACHSETUP"));
        }

        [UsesSproc(Bsys, "dbo.GetAffiliatedLenderIds")]
        public bool IsUheaaAffiliatedLenderId(string lenderId)
        {
            List<string> lenders = LDA.ExecuteList<string>("dbo.GetAffiliatedLenderIds", Bsys,
                SP("Affiliation", "UHEAA")).Result;
            return lenders.Contains(lenderId);
        }

        [UsesSproc(Udw, "[achsetup].GetCoBorrowerForBorrowerByLoan")]
        public List<CoborrowerDemographics> GetCoBorrowersForBorrowerByLoan(string borrowerSSN)
        {
            return LDA.ExecuteList<CoborrowerDemographics>("[achsetup].GetCoBorrowersForBorrowerByLoan", Udw,
                SP("BorrowerSSN", borrowerSSN)).Result;
        }

        private SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}