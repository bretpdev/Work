using System;
using System.Collections.Generic;

namespace COQTSKBLDR
{
    public class QueueBuilderRecord
    {
        public string Ssn { get; set; }
        public string Arc { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? NeededByDate { get; set; }
        public string RecipientId { get; set; }
        public RegardsTo RegardsTo { get; set; }
        public string RegardsToText { get; set; }
        public string RegardsToId { get; set; }
        public List<int> LoanSequences { get; set; }
        public string Comment { get; set; }
    }
}
