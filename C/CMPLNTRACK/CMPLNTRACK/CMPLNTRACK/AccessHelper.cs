using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace CMPLNTRACK
{
    public static class AccessHelper
    {
        public static bool IsAdmin
        {
            get
            {
                Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal wp = (WindowsPrincipal)Thread.CurrentPrincipal;
                bool result;
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, Environment.UserName);
                    PrincipalSearchResult<Principal> groups = up.GetGroups();
                    result = groups.Any((Principal o) => o.Name == "ROLE - Systems Support - Business Systems Analyst" || o.Name == "ROLE - Systems Support - Manager");
                }
                return result;
            }
        }
    }
}