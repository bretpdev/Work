using System;

namespace ADDMOREFED
{
	public class Phone
	{
		public string PhoneType { get; set; }
		public string MBLIndicator { get; set; }
		public string ConsentIndicator { get; set; }
		public DateTime VerifiedDate { get; set; }
		public string ValidityIndicator { get; set; }
		public string DomesticAreaCode { get; set; }
		public string DomesticPrefix { get; set; }
		public string DomesticLineNumber { get; set; }
		public string Extension { get; set; }
		public string ForeignCountryCode { get; set; }
		public string ForeignCityCode { get; set; }
		public string ForeignLocalNumber { get; set; }
	}
}