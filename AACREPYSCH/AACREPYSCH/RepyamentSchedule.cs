using System;
using System.Collections.Generic;

namespace AACREPYSCH
{
	class RepaymentSchedule
	{
		public string Type { get; set; }
		public DateTime FirstDueDate { get; set; }
		public bool HasMoreThan1Loan { get; set; }
		public bool HasMoreThan1Tier { get; set; }
		public bool IsALevelSch { get; set; }
		public bool IsNotFirstTeir { get; set; }
		public double PaymentAmount { get; set; }
		public string LoanPgm { get; set; }
		public Loan Loan { get; set; }
		public List<Loan> Loans { get; set; }
	}//class
}//namespace
