using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCURINT
{
    public class UheaaDemosRecord
    {
		public int DemosId { get; set; }
		public string AccountNumber { get; set; }
		public string Ssn { get; set; }
		public string EndorserSsn { get; set; }
		public string Queue { get; set; }
		public string SubQueue { get; set; }
		public string TaskControlNumber { get; set; }
		public string TaskRequestArc { get; set; }
		public DateTime? TaskCreatedAt { get; set; }
		public DateTime? AddedToRequestFileAt { get; set; }
		public DateTime? TaskCompletedAt { get; set; }
		public int? RequestArcId { get; set; }
		public int? ResponseAddressArcId { get; set; }
		public int? ResponsePhoneArcId { get; set; }

		public override string ToString()
		{
			return $"UHEAA AccountNumber: {AccountNumber}, Queue: {Queue}, SubQueue: {SubQueue}, TaskControlNumber: {TaskControlNumber}, DemosId: {DemosId}";
		}
	}
}
