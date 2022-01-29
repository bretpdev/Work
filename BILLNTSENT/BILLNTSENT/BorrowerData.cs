using System;
using System.Collections.Generic;

namespace BILLNTSENT
{
	class BorrowerData
	{
		public string AccountNumber { get; set; }
		public int LoanSequence { get; set; }
		public string DateBilled { get; set; }
		public string DateDue { get; set; }
		public string BillAmount { get; set; }
		public string SentIndicator { get; set; }
		public string SentDescription { get; set; }
		public string BillMethodType { get; set; }
	}
}
