﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Q;

namespace OPSCBPFED
{
    public class BorrowerData
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
        public string Country { get; set; }
        public MDBorrowerDemographics Demos { get; set; }
        public MDScriptInfoSpecificToBusinessUnitBase ScriptInfoToGenericBusinessUnit { get; set; }
        public double AmountPastDue { get; set; }
        public bool DemosLoaded { get; set; }
        public string LoanPrograms { get; set; }
        public string DaysDelq { get; set; }
        public double TotalBalance { get; set; }
    }
}
