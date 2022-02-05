using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWORKERLGP
{
    public class QueueData
    {
        public int QueueId { get; set; }
        public string Ssn { get; set; }
        public string Department { get; set; }
        public string WorkGroupId { get; set; }
        public string ActionCode { get; set; }
        public string ActivityType { get; set; }
        public string ActivityContactType { get; set; }
        public string TaskComment { get; set; }
        public bool HadError { get; set; }
        //public string TaskStatus { get; set; } //Probably going to want this

        public override string ToString()
        {
            return $"QueueId:{QueueId}; Ssn: {Ssn}; Department: {Department}; WorkGroupId: {WorkGroupId}; ActionCode: {ActionCode}; ActivityType: {ActivityType}; ActivityContactType: {ActivityContactType}; TaskComment: {TaskComment}";
        }
    }
}
