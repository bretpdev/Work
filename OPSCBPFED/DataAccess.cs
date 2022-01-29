using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace OPSCBPFED
{
    class DataAccess
    {
        //add record to check by phone table on CLS
        [UsesSproc(DataAccessHelper.Database.Cls, "spAddCheckByPhoneEntry")]
        public static void AddEntryToDB(OPSEntry data)
        {
            DataAccessHelper.Execute("spAddCheckByPhoneEntry", DataAccessHelper.Database.Cls,
                new SqlParameter("AccountNumber", data.AccountNumber), new SqlParameter("RoutingNumber", data.RoutingNumber), new SqlParameter("BankAccountNumber", data.BankAccountNumber),
                new SqlParameter("AccountType", data.CalculatedAccountType), new SqlParameter("PaymentAmount", data.PaymentAmount), new SqlParameter("AccountHolderName", data.AccountHolderName.Replace("'", " ")),
                new SqlParameter("EffectiveDate", data.EffectiveDate), new SqlParameter("AcctStoreAuthorization", false), new SqlParameter("DataSource", "OPSCBPFED"));
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "GetPaymentAmountRPF")]
        public static string PullRPF(OPSEntry data)
        {
            return DataAccessHelper.ExecuteSingle<string>("GetPaymentAmountRPF", DataAccessHelper.Database.Cdw, new SqlParameter("SSN", data.TS24SSN), new SqlParameter("EffectiveDate", data.EffectiveDate));
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_CheckForInvalidValues")]
        public static int CheckInvalidRouting(string routingNumber)
        {
            return DataAccessHelper.ExecuteList<int>("spGENR_CheckForInvalidValues", DataAccessHelper.Database.Csys, new SqlParameter("ValueKey", "ABA_Routing"), new SqlParameter("Value", routingNumber)).SingleOrDefault();
        }
    }
}
