using System;
using System.Windows.Forms;

namespace DOCIDUHEAA
{
    public class ProcessedDocuments
    {
        public string AccountIdentifier { get; set; }
        public string Document { get; set; }
        public string Source { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Comment { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; }
        public long ArcAddProcessingId { get; set; }
    }
}