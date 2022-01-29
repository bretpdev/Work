using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCURINT
{
    public class DemographicInfo
    {
        public string Ssn { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }

        public DemographicInfo(string ssn, string address2, string address, string city, string state, string zip, string phone)
        {
            Ssn = ssn;
            Address = address;
            Address2 = address2;
            City = city;
            State = state;
            Zip = zip;
            Phone = phone;
        }

        public override string ToString()
        {
            return $"Address1:{Address}; Address2:{Address2}; City:{City}; State:{State}; Zip:{Zip}; Phone:{Phone}";
        }
    }
}
