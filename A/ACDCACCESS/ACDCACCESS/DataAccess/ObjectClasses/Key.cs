using System;

namespace ACDCAccess
{
    public class Key
    {
		public int ID { get; set; }
        public string Name { get; set; }
        public string Application { get; set; }
		public string Type { get; set; }
        public string Description { get; set; }
		public string AddedBy { get; set; }
        public string StartDate { get; set; }
        public string RemovedBy { get; set; }
        public string EndDate { get; set; }
    }
}
