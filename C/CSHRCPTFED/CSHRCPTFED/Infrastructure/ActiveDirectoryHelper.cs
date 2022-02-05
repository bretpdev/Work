using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WebApi;

namespace CSHRCPTFED
{
    public class ActiveDirectoryUsers
    {
        const int cacheExpirationMinutes = 30;
        private ThreadedCacheItem<List<ActiveDirectoryUser>> authorizedUsers;
        ActiveDirectoryHelper helper = new ActiveDirectoryHelper();

        public ActiveDirectoryUsers(DataAccess da)
        {
            authorizedUsers = new ThreadedCacheItem<List<ActiveDirectoryUser>>("AuthorizedUsers", cacheExpirationMinutes, () =>
            {
                AuthorizedGroups = da.CollectRoles("CSHRCPTFED").ToArray();
                return helper.GetGroupMembers(AuthorizedGroups);
            });
        }
        public void EnsureCache()
        {
            var authCached = AuthorizedUsers;
        }

        public bool ValidateUser(string name)
        {
            return AuthorizedUsers.Any(o => o.AccountName.ToLower() == name.Replace(@"UHEAA\", "").ToLower());
        }

        public List<ActiveDirectoryUser> AuthorizedUsers => authorizedUsers.Item;
        public string[] AuthorizedGroups { get; private set; }

    }

}