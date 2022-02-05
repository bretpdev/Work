using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace BLKADDFED
{
    public class BorrowerAddress
    {
        public Boolean Selected { get; set; }
        public DateTime VerifiedDate { get; set; }
        public string ValidityIndicator { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }


        public BorrowerAddress(SystemBorrowerDemographics sysBrwDemo)
        {
            Selected = false;
            VerifiedDate = sysBrwDemo.AddrValidityDate;
            ValidityIndicator = sysBrwDemo.AddrValidityIndicator;
            Address1 = sysBrwDemo.Addr1;
            Address2 = sysBrwDemo.Addr2;
            City = sysBrwDemo.City;
            State = sysBrwDemo.State;
            Zip = sysBrwDemo.Zip;
            Country = sysBrwDemo.Country;
        }
    }
}
