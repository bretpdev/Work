using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.DataAccess
{
    public static class SystemAccessHelper
    {
        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetRolesForKey")]
        public static bool IsAuthorizedInActiveDirectory(string key)
        {
            var searchEntry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local");
            var searcher = new DirectorySearcher();
            searcher.SearchRoot = searchEntry;
            searcher.Filter = String.Format("SAMAccountName={0}", Environment.UserName);
            var result = searcher.FindOne();

            if (result != null)
            {
                var attributes = result.Properties;
                //get a list of roles (Active Directory groups) to which the key is assigned
                var roles = DataAccessHelper.ExecuteList<string>("spSYSA_GetRolesForKey", DataAccessHelper.Database.Csys, SqlParams.Single("UserKey", key));

                //return True if the user is found in one of the roles to which the key is assigned
                foreach (string role in roles)
                    if (attributes["memberOf"].Cast<string>().Any(o => o.ToLowerInvariant().Contains(role.ToLowerInvariant())))
                        return true;
            }

            return false;
        }
    }
}
