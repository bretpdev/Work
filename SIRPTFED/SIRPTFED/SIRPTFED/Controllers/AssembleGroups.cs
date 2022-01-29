using System.Linq;
using System.Collections.Generic;
using Uheaa.Common;

namespace SIRPTFED.Controllers
{
    public class AssembleGroups
    {
        private List<ActiveDirectoryUser> assembly { get; set; }
        private List<ActiveDirectoryUser> others { get; set; }
        public AssembleGroups()
        {
            assembly = new List<ActiveDirectoryUser>();
            others = new List<ActiveDirectoryUser>();
        }

        public List<ActiveDirectoryUser> Assembly()
        {
            ActiveDirectoryHelper helper = new ActiveDirectoryHelper();
            assembly = helper.GetGroupMembers("ROLE - Systems Support - Supervisor").ToList();

            //others = helper.GetGroupMembers("ROLE - Systems Support - Manager").ToList();
            //assembly.AddRange(others);

            others = helper.GetGroupMembers("ROLE - Systems Support - Business Systems Analyst").ToList();
            assembly.AddRange(others);

            others = helper.GetGroupMembers("ROLE - Application Development - Programmer").ToList();
            assembly.AddRange(others);

            others = helper.GetGroupMembers("ROLE - Application Development - Manager").ToList();
            assembly.AddRange(others);

            return assembly;
        }
    }
}
