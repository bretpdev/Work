using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace COMPLPPPPS
{
    class DataAccess
    {
        public static string GetBorrowersLastName(string ssn)
        {
            List<string> results = DataAccessHelper.ExecuteList<string>("spGetBorrowersLastName", DataAccessHelper.Database.Udw, new SqlParameter("Ssn", ssn));
            return results.Count < 1 ? "Smith" : results.First();
        }

    }
}
