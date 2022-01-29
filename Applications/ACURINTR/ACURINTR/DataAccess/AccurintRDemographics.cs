using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace ACURINTR
{
	class AccurintRDemographics
	{
		public string MblIndicator { get; set; }
		public string ConsentIndicator { get; set; }
        public string AccountNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string EmailAddress { get; set; }
        public string PrimaryPhone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string AlternatePhone { get; set; }
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
