using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Uheaa.Common.DataAccess;

namespace ProjectRequest
{
    public class ActiveDirectoryUser
    {
        public string CalculatedName
        {
            get
            {
                if (!string.IsNullOrEmpty(DisplayName))
                    return DisplayName;
                return FirstName + " " + LastName;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string AccountName { get; set; }
        public List<string> Groups { get; set; }
        public override bool Equals(object obj)
        {
            var other = obj as ActiveDirectoryUser;
            if (other == null)
                return false;
            return other.AccountName == AccountName;
        }

        public override int GetHashCode()
        {
            return AccountName.GetHashCode();
        }
    }

    public class ActiveDirectoryHelper
    {
        readonly string[] AuthGroups;
        readonly Dictionary<string, Models.RolePermissions> RolePermissions = new Dictionary<string, Models.RolePermissions>();
        public ActiveDirectoryHelper()
        {
            //Add all of the permissions to a lookup dicitonary
            List<Models.RolePermissions> permissions = DataAccess.GetRolePermissions();
            foreach(Models.RolePermissions p in permissions)
            {
                RolePermissions.Add(p.Role, p);
            }

            AuthGroups = RolePermissions.Keys.ToArray();
        }

        public List<ActiveDirectoryUser> GetUserByName(string user)
        {
            List<ActiveDirectoryUser> results = new List<ActiveDirectoryUser>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, "UHEAA");
            UserPrincipal userByName = UserPrincipal.FindByIdentity(context, user);

            if (userByName != null)
            {
                var result = new ActiveDirectoryUser()
                {
                    AccountName = userByName.SamAccountName,
                    FirstName = userByName.Name,
                    LastName = userByName.GivenName,
                    DisplayName = userByName.DisplayName,
                    Groups = userByName.GetGroups().Select(r => r.Name).ToList()
                };
                results.Add(result);
            }

            return results;
        }

        public enum UserPermissions
        {
            Read,
            Create,
            Score,
            ScoreFinance,
            ScoreRequestor,
            ScoreUrgency,
            ScoreResources,
            Archive,
            Admin
        }

        public void EnsureCache()
        {
            
        }

        public bool CurrentUserIsAuthorized
        {
            get
            {
                string username = HttpContext.Current.User.Identity.Name.Replace(@"UHEAA\", "").ToLower();
                string usernameWithGroup = HttpContext.Current.User.Identity.Name;
                var authorizedMatch = GetUserByName(usernameWithGroup).SingleOrDefault(o => o.AccountName.ToLower() == username);//AuthorizedUsers.SingleOrDefault(o => o.AccountName.ToLower() == username);
                return authorizedMatch != null;
            }
        }

        public bool UserHasReadPermissions()
        {
            var permissions = CurrentUserAuthorization;
            if(permissions.Admin || permissions.Read)
            {
                return true;
            }
            return false;
        }

        public bool UserHasEditPermissions()
        {
            var permissions = CurrentUserAuthorization;
            if (permissions.Admin)
            {
                return true;
            }
            return false;
        }

        public bool UserHasCreatePermissions()
        {
            var permissions = CurrentUserAuthorization;
            if (permissions.Create || permissions.Admin)
            {
                return true;
            }
            return false;
        }

        public bool UserHasArchivePermissions()
        {
            var permissions = CurrentUserAuthorization;
            if (permissions.Admin || permissions.Archive)
            {
                return true;
            }
            return false;
        }

        public Models.RolePermissions CurrentUserAuthorization
        {
            get
            {
                string username = HttpContext.Current.User.Identity.Name.Replace(@"UHEAA\", "").ToLower();
                string usernameWithGroup = HttpContext.Current.User.Identity.Name;
                var authorizedMatch = GetUserByName(usernameWithGroup).SingleOrDefault(o => o.AccountName.ToLower() == username);//AuthorizedUsers.SingleOrDefault(o => o.AccountName.ToLower() == username);
                var authorization = new Models.RolePermissions()
                {
                    Admin = false,
                    Create = false,
                    Archive = false,
                    Read = false,
                    Score = false,
                    ScoreFinance = false,
                    ScoreRequestor = false,
                    ScoreResources = false,
                    ScoreUrgency = false,
                    ScoreRisk = false,
                    Role = "ROLE_AGGREGATE"
                };
                foreach(string r in authorizedMatch.Groups)
                {
                    if(RolePermissions.ContainsKey(r))
                    {
                        authorization.Admin = authorization.Admin || RolePermissions[r].Admin;
                        authorization.Create = authorization.Create || RolePermissions[r].Create;
                        authorization.Archive = authorization.Archive || RolePermissions[r].Archive;
                        authorization.Read = authorization.Read || RolePermissions[r].Read;
                        authorization.Score = authorization.Score || RolePermissions[r].Score;
                        authorization.ScoreFinance = authorization.ScoreFinance || RolePermissions[r].ScoreFinance;
                        authorization.ScoreRequestor = authorization.ScoreRequestor || RolePermissions[r].ScoreRequestor;
                        authorization.ScoreResources = authorization.ScoreResources || RolePermissions[r].ScoreResources;
                        authorization.ScoreUrgency = authorization.ScoreUrgency || RolePermissions[r].ScoreUrgency;
                        authorization.ScoreRisk = authorization.ScoreRisk || RolePermissions[r].ScoreRisk;
                    }
                }
                return authorization;
            }
        }

    }

}