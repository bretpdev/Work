using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace BatchLoginDatabase
{
    class UserIdScriptMapping
    {
        public string UserName { get; set; }
        public bool IsRelated { get; set; }

        public static List<UserIdScriptMapping> GetAllIds(string scriptId)
        {
            return DataAccessHelper.ExecuteList<UserIdScriptMapping>("GetAllLogins", DataAccessHelper.Database.BatchProcessing, scriptId.ToSqlParameter("ScriptId"));
        }
    }
}
