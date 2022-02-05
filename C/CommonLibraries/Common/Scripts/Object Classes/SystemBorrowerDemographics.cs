using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.Scripts
{
	public class SystemBorrowerDemographics
	{
		public string AccountNumber { get; set; }
		public string Ssn { get; set; }
		public string FirstName { get; set; }
        public string AKA { get; set; }
		public string MiddleIntial { get; set; }
		public string LastName { get; set; }
		public string Suffix { get; set; }
		public string DateOfBirth { get; set; }
        public string Address1 { get; set; } 
		public string Address2 { get; set; }//UNDONE there are a lot of places in the code where we do ?? "" now that we are in VS 2015 you can do public string Address2 { get; set; } = "";
        public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public string ForeignState { get; set; }
		public bool IsValidAddress { get; set; }
		public string AddressValidityDate { get; set; }
		public string PrimaryPhone { get; set; }
		public string PrimaryPhoneType { get; set; }
		public string PrimaryMblIndicator { get; set; }
		public string PrimaryConsentIndicator { get; set; }
		public bool IsPrimaryPhoneValid { get; set; }
		public string PhoneValidityDate { get; set; }
		public string AlternatePhone { get; set; }
		public string AlternatePhoneType { get; set; }
		public string AlternateMblIndicator { get; set; }
		public string AlternateConsentIndicator { get; set; }
		public bool IsAlternatePhoneValid { get; set; }
		public string AlternamtePhoneValidityDate { get; set; }
		public string ForeignPhone { get; set; }
		public string EmailAddress { get; set; }
		public bool IsValidEmail { get; set; }
		public string EmailValidityDate { get; set; }

		public SystemBorrowerDemographics()
		{
		}

		public static string ApplyStandardAbbreviations(string address)
		{
			address = address.Replace("STREET", "ST");
			address = address.Replace("AVENUE", "AVE");
			address = address.Replace("ROAD", "RD");
			address = address.Replace("LANE", "LN");
			address = address.Replace("DRIVE", "DR");
			address = address.Replace("HIGHWAY", "HWY");
			address = address.Replace("FLOOR", "FL");
			address = address.Replace("P O BOX", "PO BOX");
			address = address.Replace("P O BX", "PO BOX");
			address = address.Replace("-", " ");
			return address;
		}
	}
}
