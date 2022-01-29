using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MD
{
    public static class AccessHelper
    {
        private static Dictionary<DataAccessHelper.Mode, bool> UserAdminChecks = new Dictionary<DataAccessHelper.Mode, bool>();
        public static bool UserIsFaqAdmin
        {
            get
            {
                if (!DataAccessHelper.ModeSet) return false;
                if (!UserAdminChecks.ContainsKey(DataAccessHelper.CurrentMode))
                {
                    try
                    {
                        UserAdminChecks[DataAccessHelper.CurrentMode] = 
                            DataAccessHelper.ExecuteSingle<bool>("spMD_UserIsFaqAdmin", DataAccessHelper.Database.Csys, SqlParams.Single("WindowsUserName", Environment.UserName));
                    }
                    catch (SqlException)
                    {
                        //stored procedure doesn't exist yet
                        UserAdminChecks[DataAccessHelper.CurrentMode] = false;
                    }
                }
                return UserAdminChecks[DataAccessHelper.CurrentMode];
            }
        }
    }
}
