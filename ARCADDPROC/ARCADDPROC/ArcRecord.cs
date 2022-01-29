using System;
using System.Collections.Generic;

namespace ARCADDPROCESSING
{
    public class ArcRecord
    {
        public long ArcAddProcessingId { get; set; }
        public int ArcTypeId { get; set; }
        public string AccountNumber { get; set; }
        public string RecipientId { get; set; }
        public string Arc { get; set; }
        public string ScriptId { get; set; }
        public string Comment { get; set; }
        public bool IsReference { get; set; }
        public bool IsEndorser { get; set; }
        public DateTime? ProcessFrom { get; set; }
        public DateTime? ProcessTo { get; set; }
        public DateTime? NeededBy { get; set; }
        public string RegardsTo { get; set; }
        public string RegardsCode { get; set; }
        public List<string> LoanPrograms { get; set; }
        public List<int> LoanSequences { get; set; }
        public string ResponseCode { get; set; }
        public int ProcessingAttempts { get; set; }

        public void Trim()
        {
            AccountNumber = AccountNumber.Trim();
            if (RecipientId != null)
                RecipientId = RecipientId.Trim();
            Arc = Arc.Trim();
            if (Comment != null)
                Comment = Comment.Trim();
            if (ResponseCode != null)
                ResponseCode = ResponseCode.Trim();
        }
    }
}
