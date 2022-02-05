using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CLMPMTPST
{
	public class Payment
	{
		public const string DATE_FORMAT = "MMddyy";
		public int ClaimPaymentId { get; set; }
		public string AccountNumber { get; set; }
		public string Ssn { get; set; }
		public double PaymentAmount { get; set; }
		[DisplayName("Effective Date")]
		public DateTime EffectiveDate { get; set; }
		[DisplayName("Guarantor Code")]
		public string GuarantorCode { get; set; }
		[DisplayName("Loan Sequences")]
		public List<int> LoanSequences { get; set; }
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		public Payment(string ssn, double amount, DateTime effectiveDate, string guarantorCode, List<int> loanSequences, string lastName)
		{
			Ssn = ssn;
			PaymentAmount = amount;
			EffectiveDate = effectiveDate;
			GuarantorCode = guarantorCode;
			LoanSequences = loanSequences;
			LastName = lastName;
		}
    }
}
