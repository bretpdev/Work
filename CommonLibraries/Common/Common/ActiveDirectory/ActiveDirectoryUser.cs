using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
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

}
