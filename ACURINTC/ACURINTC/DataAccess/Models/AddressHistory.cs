using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    public class AddressHistory
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string ForeignState { get; set; }
        public bool AddressIsValid { get; set; }
        public DateTime VerificationDate { get; set; }
    }
}
