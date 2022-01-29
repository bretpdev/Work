using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;

namespace SCHRPT
{
    public class ActiveDirectoryUsers
    {
        const int cacheExpirationMinutes = 30;
        private ThreadedCacheItem<List<ActiveDirectoryUser>> authorizedUsers;
        readonly string[] AuthGroups;
        ActiveDirectoryHelper helper = new ActiveDirectoryHelper();
        public ActiveDirectoryUsers()
        {
            if (DataAccessHelper.TestMode)
                AuthGroups = new string[] { "UHEAA Developers", "ROLE - Systems Support - Business Systems Specialist", "ROLE - Systems Support - Business Systems Analyst", "ROLE - Projects - Business Systems Analyst" };
            else
                AuthGroups = new string[] { "ROLE - Systems Support - Business Systems Specialist", "ROLE - Systems Support - Business Systems Analyst", "ROLE - Projects - Business Systems Analyst" };

            authorizedUsers = new ThreadedCacheItem<List<ActiveDirectoryUser>>("AuthorizedUsers", cacheExpirationMinutes, () => helper.GetGroupMembers(AuthGroups));
        }
        public void EnsureCache()
        {
            var authCached = AuthorizedUsers;
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

        public List<ActiveDirectoryUser> AuthorizedUsers => authorizedUsers.Item;

    }

}