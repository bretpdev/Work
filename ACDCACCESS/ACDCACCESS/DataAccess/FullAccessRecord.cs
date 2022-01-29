using Q;

namespace ACDCAccess
{
    class FullAccessRecord
    {
        public string UserKey { get; set; }
        public string Application { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public string WindowsUserName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AddedBy { get; set; }
		public int RemovedBy { get; set; }
		public string UserName { get; set; }
		public string AddedByUserName { get; set; }
		public string RemovedByUserName { get; set; }
    }
}
