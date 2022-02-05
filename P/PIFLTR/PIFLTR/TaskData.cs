using Uheaa.Common.DataAccess;

namespace PIFLTR
{
    public class TaskData
    {
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        private string taskControlNumber;
        public string TaskControlNumber
        {
            get
            {
                return taskControlNumber;
            }
            set
            {
                taskControlNumber = value.Trim();
            }
        }

        [DbName("RequestArc")]
        public string ActionResponse { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public int LoanSeq { get; set; }
        public string LoanProgram { get; set; }
        public string EffectiveDate { get; set; }
        public bool IsConsolPif { get; set; } //Indicates if a consolidated loan
        public bool IsCanceled { get; set; }
        public string CoBorrowerSsn { get; set; }
        public string FirstDisbursementDate { get; set; }
        public string OriginalBalance { get; set; }
        public string CostCode { get; set; }

        public int? PrintProcessingId { get; set; }

        public int? CoBwrPrintProcessingId { get; set; } 
        public int ProcessQueueId { get; set; }
        
        public override string ToString()
        {
            return string.Format("Queue:{0}; SubQueue:{1}; TaskControlNumber:{2}; AccountNumber:{3}; Ssn:{4}; ActionResponse:{5}",
                 Queue, SubQueue, TaskControlNumber, AccountNumber, Ssn, ActionResponse);
        }

    }
}
