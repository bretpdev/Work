using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCURINT
{
    public class RunInfo
    {
        public int RunId { get; set; }
        public string RequestFileName { get; set; }
        public DateTime? RequestFileCreatedAt { get; set; }
        public DateTime? RequestFileUploadedAt { get; set; }
        public string ResponseFileName { get; set; }
        public DateTime? ResponseFileDownloadedAt { get; set; }
        public DateTime? ResponseFileProcessedAt { get; set; }
        public int? RecordsSent { get; set; }
        public int? RecordsReceived { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
