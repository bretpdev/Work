using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common;

namespace LNDERLETTR
{
 public class US06BLCNTMBwrAddr
    {
        public string Ssn { get; set; }
        public string SsnLastFour
        {
            get
            {
                return Ssn.IsPopulated() ? Ssn.Substring(5, 4) : "";
            }
        }

        public string Name { get; set; }
        public string PrevName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string AltPhone1 { get; set; }
        public string AltPhone2 { get; set; }
        public string EmailAddress { get; set; }
        public string Province {  get; set; }
        public string Country { get; set; }

    }
}
