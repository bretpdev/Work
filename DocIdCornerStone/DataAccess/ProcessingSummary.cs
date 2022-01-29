using System;

namespace DocIdCornerStone
{
	class ProcessingSummary
	{
		public DateTime Date { get; set; }
		public string DocId { get; set; }
		public int PostOffice { get; set; }
		public int InHouse { get; set; }
		public int Fax { get; set; }
		public int Other { get; set; }
		public int Total { get; set; }
	}
}
