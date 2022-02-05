using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class ActiveDirectoryHelper
    {
        public string GetRoleName()
        {
            foreach (var group in UserPrincipal.Current.GetGroups())
            {
                if (group.Name.StartsWith("ROLE -"))
                    return group.Name;
            }
            throw new Exception("No ROLE found.");
        }

        public List<ActiveDirectoryUser> GetGroupMembers(string[] groupNames)
        {
            List<ActiveDirectoryUser> results = new List<ActiveDirectoryUser>();
            foreach (var groupName in groupNames)
                results.AddRange(GetGroupMembers(groupName));
            return results;
        }
        public List<ActiveDirectoryUser> GetGroupMembers(string groupName)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            using (var searcher = new PrincipalSearcher())
            using (var group = new GroupPrincipal(context, groupName))
            {
                searcher.QueryFilter = group;
                var foundGroup = searcher.FindOne() as GroupPrincipal;
                if (foundGroup == null)
                    return new List<ActiveDirectoryUser>();
                var results = GetUsersFromGroupRecursive(foundGroup);
                return results.Distinct().OrderBy(o => o.CalculatedName).ToList();
            }
        }

        private List<ActiveDirectoryUser> GetUsersFromGroupRecursive(GroupPrincipal group)
        {
            List<ActiveDirectoryUser> results = new List<ActiveDirectoryUser>();
            foreach (var member in group.Members)
            {
                if (member.Name == group.Name)
                    continue;  //recursive group reference, ignore
                var childGroup = member as GroupPrincipal;
                if (childGroup != null)
                    results.AddRange(GetUsersFromGroupRecursive(childGroup));
                var user = member as UserPrincipal;
                if (user != null)
                {
                    var result = new ActiveDirectoryUser()
                    {
                        AccountName = user.SamAccountName,
                        FirstName = user.Name,
                        LastName = user.GivenName,
                        DisplayName = user.DisplayName
                    };
                    results.Add(result);
                }
            }
            return results;
        }

        public List<string> GetSubGroupsRecursive(string groupName, int depth = -1)
        {
            List<string> results = new List<string>();
            using (var context = new PrincipalContext(ContextType.Domain))
            using (var searcher = new PrincipalSearcher())
            using (var group = new GroupPrincipal(context, groupName))
            {
                searcher.QueryFilter = group;
                var foundGroup = searcher.FindOne() as GroupPrincipal;
                if (depth != 0)
                {
                    depth--;
                    foreach (var member in foundGroup.Members)
                    {
                        if (member.Name == groupName)
                            continue; //ignore recursive reference.
                        var childGroup = member as GroupPrincipal;
                        if (childGroup != null)
                        {
                            results.Add(childGroup.Name);
                            var childResults = GetSubGroupsRecursive(childGroup.Name, depth);
                            results.AddRange(childResults);
                        }
                    }
                }
            }
            return results;
        }

    }
}
