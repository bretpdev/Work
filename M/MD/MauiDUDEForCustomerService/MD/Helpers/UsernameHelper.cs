using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

namespace MD
{
    public static class UsernameHelper
    {
        public static string GetDisplayName(string username)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, username);
                if (up != null)
                    return up.DisplayName;
            }
            return null;
        }
    }
}
