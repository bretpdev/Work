using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace OPSCHKPHN
{
    class DataAccess
    {
		[UsesSproc(DataAccessHelper.Database.Norad, "spCKPH_AddCheckByPhoneEntry")]
        public static void AddEntryToDB(OPSEntry data)
        {
			DataAccessHelper.Execute("spCKPH_AddCheckByPhoneEntry", DataAccessHelper.Database.Norad, new SqlParameter("SSN", data.TS24SSN), new SqlParameter("NAME", data.TS24Name),
				new SqlParameter("DOB", data.TS24DOB), new SqlParameter("ABA", data.RoutingNumber), new SqlParameter("BANKACCOUNTNUMBER", data.BankAccountNumber), new SqlParameter("ACCOUNTTYPE", data.CalculatedAccountType),
				new SqlParameter("AMOUNT", data.PaymentAmount), new SqlParameter("EFFECTIVEDATE", data.EffectiveDate), new SqlParameter("ACCOUNTHOLDERNAME", data.AccountHolderName), new SqlParameter("DataSource", "OPSCHKPHN"));
        }

		[UsesSproc(DataAccessHelper.Database.Udw, "GetPaymentAmountRPF")]
		public static string PullRPF(OPSEntry data)
		{
			return DataAccessHelper.ExecuteSingle<string>("GetPaymentAmountRPF", DataAccessHelper.Database.Udw, new SqlParameter("SSN", data.TS24SSN), new SqlParameter("EffectiveDate", data.EffectiveDate));		}
    }
}
