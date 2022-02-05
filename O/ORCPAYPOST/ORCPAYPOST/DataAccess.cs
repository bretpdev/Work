using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace ORCPAYPOST
{
    class DataAccess
    {
        public static string GetBorrowersLastName(string accountNumber)
        {
            List<string> results = DataAccessHelper.ExecuteList<string>("spGetBorrowersLastNameByAccountNumber"
                , DataAccessHelper.Database.Udw, new SqlParameter("AccountNumber", accountNumber));
            return results.Count < 1 ? "Smith" : results.First();
        }
    }
}
