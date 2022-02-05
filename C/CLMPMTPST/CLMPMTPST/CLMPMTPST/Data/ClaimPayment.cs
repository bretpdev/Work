using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLMPMTPST
{
    public class ClaimPayment
    {
		public int ClaimPaymentId { get; set; }
		public string AccountNumber { get; set; }
		public string Ssn { get; set; }
		public double PaymentAmount { get; set; }
		[DisplayName("Effective Date")]
		public DateTime EffectiveDate { get; set; }
		[DisplayName("Guarantor Code")]
		public string GuarantorCode { get; set; }
		[DisplayName("Loan Sequences")]
		public int LoanSequence { get; set; }
		[DisplayName("Last Name")]
		public string LastName { get; set; }
		public const string DATE_FORMAT = "MMddyy";
	}
}
