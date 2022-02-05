using System;

namespace CMPLNTRACK
{
    public class ComplaintHistory
    {
        public int ComplaintHistoryId { get; set; }
        public string HistoryDetail { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
    }
}