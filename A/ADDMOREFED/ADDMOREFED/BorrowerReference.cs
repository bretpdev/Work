using Uheaa.Common.Scripts;

namespace ADDMOREFED
{
    public class BorrowerReference
    {
        public SystemBorrowerDemographics Demographics { get; set; }
        public System.Windows.Forms.CheckState AddressInvalidateFirst { get; set; }
        public Phone HomePhone { get; set; }
        public System.Windows.Forms.CheckState HomePhoneInvalidateFirst { get; set; }
		public Phone AlternatePhone { get; set; }
        public System.Windows.Forms.CheckState AlternatePhoneInvalidateFirst { get; set; }
		public Phone WorkPhone { get; set; }
        public System.Windows.Forms.CheckState WorkPhoneInvalidateFirst { get; set; }
        public System.Windows.Forms.CheckState EmailInvalidateFirst { get; set; }
        public string Suffix { get; set; }
        public string Relationship { get; set; }
        public string Source { get; set; }

        public BorrowerReference()
        { 
            Demographics = new SystemBorrowerDemographics();
            HomePhone = new Phone();
			AlternatePhone = new Phone();
            WorkPhone = new Phone();
        }
     }

}
