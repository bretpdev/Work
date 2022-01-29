using System;

namespace R301QUEUE
{
	class QueueData
	{
		public string SSN { get; set; }
		public string LoanSequence { get; set; }
		public string Status { get; set; }
		public DateTime DateRequested { get; set; }
		public bool WasWorked { get; set; }
	}//class
}//namespace
