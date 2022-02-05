using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCURINT
{
    public class OneLinkDemosRecord
    {
		public int DemosId { get; set; }
		public string AccountNumber { get; set; }
		public string Ssn { get; set; }
		public string WorkGroup { get; set; }
		public string Department { get; set; }
		public DateTime? TaskCreatedAt { get; set; }
		public bool? SendToAccurint { get; set; }
		public DateTime? AddedToRequestFileAt { get; set; }
		public DateTime? TaskCompletedAt { get; set; }
		public bool? RequestCommentAdded { get; set; }
		public int? AddressTaskQueueId { get; set; }
		public int? PhoneTaskQueueId { get; set; }

		public override string ToString()
		{
			return $"OneLINK AccountNumber: {AccountNumber}, WorkGroup: {WorkGroup}, Dept: {Department}, DemosId: {DemosId}";
		}
	}
}
