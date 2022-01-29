using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace TCPAFEDPS
{
    class DataAccess 
    {
        public static List<Arcs> GetArcsToCheck()
        {
            return DataAccessHelper.ExecuteList<Arcs>("spGetConsentArcs", DataAccessHelper.Database.Cls);
        }

        public static string GetAccountNumber(string ssn)
        {
            return DataAccessHelper.ExecuteSingle<string>("spGetAccountNumberFromSsn", DataAccessHelper.Database.Cdw, new SqlParameter("Ssn", ssn));
        }
    }
}
