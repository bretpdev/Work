using System;
using System.Data.Linq;
using System.Linq;
using Q;

namespace ENRCNCTSKS
{
    class DataAccess : DataAccessBase
    {
        private static string _orsManagerUserId = null;
        public static string GetOrsManagerUserId(bool testMode)
        {
            if (string.IsNullOrEmpty(_orsManagerUserId))
            {
                string query = "SELECT Top 1 A.UserID";
                query += " FROM SYSA_LST_UserIDInfo A";
                query += " INNER JOIN GENR_REF_BU_Agent_Xref B";
                query += " ON A.WindowsUserName = B.WindowsUserId";
                query += " WHERE B.BusinessUnit = 'Account Services'";
                query += " AND B.Role = 'Manager'";
                query += " AND A.[Date Access Removed] IS NULL";
                query += " ORDER BY UserID";
                _orsManagerUserId = BsysDataContext(testMode).ExecuteQuery<string>(query).SingleOrDefault();
                if (string.IsNullOrEmpty(_orsManagerUserId)) { throw new Exception("ORS manager not found in BSYS."); }
            }
            return _orsManagerUserId;
        }//GetOrsManagerUserId()
    }//class
}//namespace
