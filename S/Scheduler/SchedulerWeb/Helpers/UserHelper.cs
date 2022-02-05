using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;

namespace SchedulerWeb
{
    public class UserHelper
    {
        private string sam;
        public UserHelper(System.Security.Principal.IIdentity identity)
        {
            sam = identity.Name;
        }
        public UserType CurrentUserType
        {
            get
            {
                var result = FindCurrentUser;
                foreach (string memberOf in result.Properties["memberOf"])
                {
                    var isInGroup = new Func<string, bool>(find =>
                    {
                        string format = "CN={0},";
                        return memberOf.ToLower().Contains(string.Format(format, find).ToLower());
                    });
                    if (userTypeOverrides.ContainsKey(sam))
                        return userTypeOverrides[sam];
                    if (isInGroup("ROLE - Operations - Director"))
                        return UserType.Admin;
                    else if (isInGroup("ROLE - Projects - Business Systems Analyst"))
                        return UserType.BusinessAnalyst;
                    else if (isInGroup("Developers"))
                        return UserType.Developer;
                }
                return UserType.NonUser;
            }
        }

        public string CurrentFullName
        {
            get
            {
                var result = FindCurrentUser;
                var name = (string)result.Properties["displayName"][0];
                return name;
            }
        }

        private SearchResult FindCurrentUser
        {
            get
            {
                sam = sam.Replace("UHEAA\\", "");
                DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"));
                searcher.Filter = string.Format("(&(samaccountname={0}))", sam);

                var result = searcher.FindOne();
                return result;
            }
        }

        public void TypeToggle()
        {
            var type = CurrentUserType;
            type++;
            if ((int)type >= Enum.GetNames(typeof(UserType)).Length)
                type = default(UserType);
            if (type == UserType.NonUser)
                type++;
            SetOverride(sam, type);
        }

        public static Dictionary<string, UserType> userTypeOverrides = new Dictionary<string, UserType>();
        public static void SetOverride(string sam, UserType newType)
        {
            userTypeOverrides[sam] = newType;
            if (newType == UserType.NonUser)
                userTypeOverrides.Remove(sam);
        }

    }
}