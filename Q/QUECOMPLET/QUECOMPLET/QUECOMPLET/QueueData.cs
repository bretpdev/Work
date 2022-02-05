namespace QUECOMPLET
{
    public class QueueData
    {
        public int QueueId { get; set; }
        public string Queue { get; set; }
        public string SubQueue { get; set; }
        public string AccountIdentifier { get; set; }
        public string TaskControlNumber { get; set; }
        public string ARC { get; set; }
        public string TaskStatus { get; set; }
        public string ActionResponse { get; set; }
        public string WC_TYP_NUM_CTL_TSK { get; set; }

        public override string ToString()
        {
            return $"QueueId:{QueueId}; Queue: {Queue}; SubQueue: {SubQueue}; AccountIdentifier: {AccountIdentifier}; TaskControlNumber: {TaskControlNumber}; ARC: {ARC}; TaskStatus: {TaskStatus}; ActionResponse: {ActionResponse}";
        }
    }
}