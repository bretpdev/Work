using System;

namespace COMPCB
{
    /// <summary>
    /// Encapsulates the task data: the borrower/endorser
    /// for which the task is being done and the task's
    /// processing status.
    /// </summary>
    public class TaskInfo
    {
        public int ProcessingQueueId { get; set; }
        public string BorrowerSsn { get; set; }
        public string BorrowerAccountNumber { get; set; }
        public string EndorserSsn { get; set; }
        public string EndorserAccountNumber { get; set; }
        public string TaskControlNumber { get; set; }
        public string RequestArc { get; set; }
        public DateTime TaskRequestedDate { get; set; }
        public bool? IsEndorserTask { get; set; } // Determined off the ARC type
        public bool? IsForeignAddress { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? ArcAddProcessingId { get; set; } // Relates to the ULS.dbo.ArcAddProcessing table
        public int ProcessingAttempts { get; set; }
    }
}
