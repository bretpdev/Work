using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace KEYIDENCHN
{
    public static class DataAccess
    {
        public static bool CurrentUserHasSupervisorAccess()
        {
#if DEBUG
            return true;
#endif
            return UserHasSupervisorAccess(Environment.UserName);
        }

        public static bool UserHasSupervisorAccess(string windowsUsername)
        {
            return DataAccessHelper.ExecuteSingle<bool>("spSYSA_UserHasUserKey", DataAccessHelper.Database.Csys,
                new SqlParameter("WindowsUserName", windowsUsername), new SqlParameter("UserKey", "CRKEY"));
        }

        public static List<Supervisor> GetAllSupervisors()
        {
            List<Supervisor> all = new List<Supervisor>();
            foreach (Supervisor s in GetSupervisors())
                if (UserHasSupervisorAccess(s.WindowsUserName))
                    all.AddRange(Supervisor.LoadIds(s));
            return all;
        }

        private static List<Supervisor> GetSupervisors()
        {
            return DataAccessHelper.ExecuteList<Supervisor>("spSYSA_GetUsersByBusinessUnitId", DataAccessHelper.Database.Csys, SqlParams.Single("BusinessUnitId", 1));
        }

        public static List<string> GetSupervisorUtIds(string supervisorWindowsUsername)
        {
            return DataAccessHelper.ExecuteList<string>("spSYSA_GetUtIdByWindowsUsername", DataAccessHelper.Database.Bsys, SqlParams.Single("WindowsUsername", supervisorWindowsUsername));
        }
    }
}
