using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace TCPAMCGTNM
{
	class DataAccess
	{
		public static string GetSsnFromAccountNumber(string accountNumber)
		{
            return DataAccessHelper.GetContext(DataAccessHelper.Database.Cdw).ExecuteQuery<string>(@"EXEC spGetSSNFromAcctNumber {0}", accountNumber).SingleOrDefault();
		}
	}
}
