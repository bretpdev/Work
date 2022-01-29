using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTNEMLINVF
{
    public class UserByRegion
    {
        public string Ssn { get; set; }
        public bool OneLink { get; set; }

        public UserByRegion(string ssn, bool oneLink)
        {
            Ssn = ssn;
            OneLink = oneLink;
        }
    }

    //Used with the dictionary so key look ups can compare values
    public class UserByRegionComparer : IEqualityComparer<UserByRegion>
    {
        public bool Equals(UserByRegion x, UserByRegion y)
        {
            if(x == null && y == null)
            {
                return true;
            }
            else if(x == null || y == null)
            {
                return false;
            }
            else if(x.Ssn == y.Ssn && x.OneLink == y.OneLink)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(UserByRegion obj)
        {
            return obj.Ssn.GetHashCode() ^ obj.OneLink.GetHashCode();
        }
    }
}
