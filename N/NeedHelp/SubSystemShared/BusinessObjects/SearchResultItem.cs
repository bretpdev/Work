using System;

namespace SubSystemShared
{
    public class SearchResultItem
    {
		public string TicketCode { get; set; }
        public long TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
		public DateTime LastUpdateDate { get; set; }
    }
}