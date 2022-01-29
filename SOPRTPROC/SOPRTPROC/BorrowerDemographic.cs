using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOPRTPROC
{
    public class BorrowerDemographic
    {
        public string SSN { get; set; }
        public string AccountNumber { get; set; }
        public string BorrowerName { get; set; }
        public string GarnishmentAmount { get; set; }
        public DateTime ListDate { get; set; }
        public string WarrantNumber { get; set; }
        public bool IsSingle { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool UpdateAddress { get; set; }
    }
}
