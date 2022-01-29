﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFACDFED
{
    public class AddressInfo
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityStateZip { get; set; }
        public string ForeignState { get; set; }
        public string Country { get; set; }
        public bool HasValidAddress { get; set; }
    }
}
