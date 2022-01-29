using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.WebApi;

namespace LSLETTERSU
{
    public class ActiveDirectoryUsers
    {
        const int cacheExpirationMinutes = 30;
        private ThreadedCacheItem<List<ActiveDirectoryUser>> authorizedUsers;
        ActiveDirectoryHelper helper = new ActiveDirectoryHelper();
        public static string UserName { get; set; }

        public ActiveDirectoryUsers(DataAccess da)
        {
            authorizedUsers = new ThreadedCacheItem<List<ActiveDirectoryUser>>("AuthorizedUsers", cacheExpirationMinutes, () =>
            {
                AuthorizedGroups = da.CollectRoles("LSLETTERSU").ToArray();
                return helper.GetGroupMembers(AuthorizedGroups);
            });
        }
        public void EnsureCache()
        {
            var authCached = AuthorizedUsers;
        }

        public bool ValidateUser(string name)
        {
            UserName = AuthorizedUsers.Where(o => o.AccountName.ToLower() == name.Replace(@"UHEAA\", "")).FirstOrDefault()?.AccountName;
            return AuthorizedUsers.Any(o => o.AccountName.ToLower() == name.Replace(@"UHEAA\", "").ToLower());
        }

        public List<ActiveDirectoryUser> AuthorizedUsers => authorizedUsers.Item;
        public string[] AuthorizedGroups { get; private set; }
    }
}