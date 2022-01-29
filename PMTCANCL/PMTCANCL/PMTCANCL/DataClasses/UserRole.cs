using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTCANCL
{
    public class UserRole
    {
        public int Role { get; set; }
        public string Description { get; set; }
        public bool UheaaAccess { get; set; }
        public bool FedAccess { get; set; }

        public UserRole()
        {

        }

        public UserRole(int role, string desc, bool uAccess, bool fAccess)
        {
            Role = role;
            Description = desc;
            UheaaAccess = uAccess;
            FedAccess = fAccess;
        }
    }
}
