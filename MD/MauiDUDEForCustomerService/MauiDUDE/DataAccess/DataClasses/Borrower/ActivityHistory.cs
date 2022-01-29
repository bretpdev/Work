using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class ActivityHistoryRecord
    {
        public int SequenceNumber { get; set; }
        public string RequestCode { get; set; }
        public string ResponseCode { get; set; }
        public string RequestDescription { get; set; }
        public string RequestDate { get; set; }
        public string ResponseDate { get; set; }
        public string Requestor { get; set; }
        public string PerformedDate { get; set; }
        public string CommentText { get; set; }
    }
}
