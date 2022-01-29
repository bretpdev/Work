using System;
using System.Collections.Generic;

namespace Uheaa.Common.DataAccess
{
    public class QueueResults
    {
        public int QueueId { get; set; }
        public bool QueueAdded { get; set; }
        public List<string> Errors { get; set; }
        public Exception Ex { get; set; }

        public QueueResults()
        {
            Errors = new List<string>();
        }
    }
}