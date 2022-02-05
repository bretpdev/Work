using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLTRS.DataObjects
{
    public class CoBorrowerRecord
    {
        public string CoBorrowerSSN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string ValidAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ForeignCountry { get; set; }
        public string AF_APL_ID { get; set; }
        public string AF_APL_ID_SFX { get; set; }
        public string UniqueId { get; set; }
    }
}
