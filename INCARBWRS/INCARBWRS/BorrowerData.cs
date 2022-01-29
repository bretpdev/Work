using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INCARBWRS
{
    class BorrowerData
    {
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ForenState { get; set; }
        public string Country { get; set; }
        public string PayoffDate { get; set; }
        public bool DemosLoaded { get; set; }
    }
}
