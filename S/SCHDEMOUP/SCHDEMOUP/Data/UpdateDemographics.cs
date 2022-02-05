using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCHDEMOUP
{
    class UpdateDemographics
    {
        public bool NameUpdated { get; set; }
        public bool AddressUpdated { get; set; }
        public bool PhoneUpdated { get; set; }
        public bool FaxUpdated { get; set; }
        public bool ContactUpdated { get; set; }
        public string UpdateMode { get; set; }
    }
}
