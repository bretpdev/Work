using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    public static class UtIdHelper
    {
        /// <summary>
        /// The list of UT IDs that match the current windows username
        /// </summary>
        public static IEnumerable<string> CachedUtIds { get; private set; }
        static UtIdHelper()
        {
            ReCache();
        }

        public static void ReCache(string windowsUserName = null)
        {
            if (windowsUserName == null)
                windowsUserName = Environment.UserName;

            CachedUtIds = DataAccessHelper.ExecuteList<string>("GetUserId", DataAccessHelper.Database.Bsys, new SqlParameter("WindowsUserName", windowsUserName));

        }
    }
}
