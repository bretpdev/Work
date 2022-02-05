using System;
using Uheaa.Common.ProcessLogger;

namespace SubSystemShared
{
    public class SearchCriteria
    {
		public string TicketNumber { get; set; }
		public KeyWordScope KeyWordSearchScope { get; set; }
		public string KeyWord { get; set; }
		public string Subject { get; set; }
        public SqlUser Court { get; set; }
        public SqlUser Requester { get; set; }
		public string Status { get; set; }
		public BusinessUnit BusinessUnit { get; set; }
		public TicketType TicketType { get; set; }
		public string FunctionalArea { get; set; }
        public SqlUser AssignedTo { get; set; }
		public SortField SortingOption { get; set; }
		public DateTime CreateDateRangeStart { get; set; }
		public DateTime CreateDateRangeEnd { get; set; }

		public SearchCriteria()
		{
			//CreateDateRangeStart and CreateDateRangeEnd will always have a value,
			//but their default value (1/1/0001 12:00 AM) isn't valid for SQL Server.
			//It looks like the data binding on the search form doesn't take effect
			//before the initial search when the form starts up, so we need to
			//assign values here that are valid in SQL Server.
			CreateDateRangeStart = new DateTime(1900, 1, 1);
			CreateDateRangeEnd = DateTime.Now;
		}
    }
}