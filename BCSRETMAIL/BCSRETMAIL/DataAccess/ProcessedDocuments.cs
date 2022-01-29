using System;

namespace BCSRETMAIL
{
    public class ProcessedDocuments
    {
        public string AccountIdentifier { get; set; }
        public string LetterId { get; set; }
        public string System { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool HasForwarding { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public DateTime? ArcProcessedAt { get; set; }
    }
}