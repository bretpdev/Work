using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace UheaaWebActiveDirectory
{
    public class DataAccess
    {
        LogDataAccess LDA { get; }
        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        [UsesSproc(DB.UheaaWebManagement, "activedirectorycache.[SyncGroups]")]
        public bool SyncGroups()
        {
            return LDA.Execute("activedirectorycache.[SyncGroups]", DB.UheaaWebManagement);
        }

        [UsesSproc(DB.UheaaWebManagement, "activedirectorycache.[GetServerDate]")]

        public DateTime GetServerDate()
        {
            return LDA.ExecuteSingle<DateTime>("activedirectorycache.[GetServerDate]", DB.UheaaWebManagement).Result;
}

        [UsesSproc(DB.UheaaWebManagement, "activedirectorycache.[GetActiveDirectoryRoles]")]
        public List<Role> GetActiveDirectoryRoles()
        {
            return LDA.ExecuteList<Role>("activedirectorycache.[GetActiveDirectoryRoles]", DB.UheaaWebManagement).Result;
        }

        [UsesSproc(DB.UheaaWebManagement, "activedirectorycache.[SetUserAndGroups]")]
        public bool SetUserAndGroups(string associatedWindowsUsername, IEnumerable<int> groupIds)
        {
            var dt = new DataTable();
            dt.Columns.Add("GroupId");
            foreach (var groupId in groupIds)
                dt.Rows.Add(groupId);
            return LDA.Execute("activedirectorycache.[SetUserAndGroups]", DB.UheaaWebManagement, Sp("AssociatedWindowsUsername", associatedWindowsUsername), Sp("GroupIds", dt));
        }

        [UsesSproc(DB.UheaaWebManagement, "activedirectorycache.[RemoveOldUsers]")]
        public bool RemoveOldUsers(DateTime thresholdDate)
        {
            return LDA.Execute("activedirectorycache.[RemoveOldUsers]", DB.UheaaWebManagement, Sp("ThresholdDate", thresholdDate));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
