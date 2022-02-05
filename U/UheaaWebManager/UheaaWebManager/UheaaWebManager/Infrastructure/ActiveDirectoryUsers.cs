using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace UheaaWebManager
{
    public class ActiveDirectoryUsers
    {
        const int cacheExpirationMinutes = 30;
        private ThreadedCacheItem<List<ActiveDirectoryUser>> windowsUsers;
        private ThreadedCacheItem<List<ActiveDirectoryUser>> authorizedUsers;
        private ThreadedCacheItem<List<string>> availableGroups;
        readonly string AuthGroup;
        ActiveDirectoryHelper helper = new ActiveDirectoryHelper();
        public ActiveDirectoryUsers()
        {
            if (DataAccessHelper.TestMode)
                AuthGroup = "UHEAA Developers";
            else
                AuthGroup = "ROLE - Systems Support - Business Systems Specialist";

            windowsUsers = new ThreadedCacheItem<List<ActiveDirectoryUser>>("WindowsUsers", cacheExpirationMinutes, () => helper.GetGroupMembers("Uheaa Staff"));
            authorizedUsers = new ThreadedCacheItem<List<ActiveDirectoryUser>>("AuthorizedUsers", cacheExpirationMinutes, () => helper.GetGroupMembers(AuthGroup));
            availableGroups = new ThreadedCacheItem<List<string>>("AvailableGroups", cacheExpirationMinutes, () => helper.GetSubGroupsRecursive("Uheaa Staff").OrderBy(o => o).ToList());
        }
        public void EnsureCache()
        {
            var cached = WindowsUsers;
            var authCached = AuthorizedUsers;
            var availableGroups = AvailableGroups;
        }

        public bool CurrentUserIsAuthorized
        {
            get
            {
                string username = HttpContext.Current.User.Identity.Name.Replace(@"UHEAA\", "").ToLower();
                var authorizedMatch = AuthorizedUsers.SingleOrDefault(o => o.AccountName.ToLower() == username);
                return authorizedMatch != null;
            }
        }

        public List<ActiveDirectoryUser> WindowsUsers => windowsUsers.Item;
        public List<ActiveDirectoryUser> AuthorizedUsers => authorizedUsers.Item;
        public List<string> AvailableGroups => availableGroups.Item;
    }
}