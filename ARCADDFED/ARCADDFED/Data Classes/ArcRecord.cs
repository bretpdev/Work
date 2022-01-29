using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace ARCADDFED
{
	class ArcRecord
	{

		public long RecordId { get; set; }
		public string AccountNumber { get; set; }
		public string RecipientId { get; set; }
		public TD22.RegardsTo InRegardsTo { get; set; }
		public string Arc { get; set; }
		public string Comment { get; set; }
		public DateTime RequestedDate { get; set; }
		public string UserId { get; set; }	

	}
}
