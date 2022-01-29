using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUPLREFS
{
    public class Borrower
    {
        public int? BorrowerQueueId { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string UserId { get; set; } //Agent who ran the script
        public List<Reference> References { get; set; } = new List<Reference>();
        public List<Reference> DuplicateReferences { get; set; } = new List<Reference>();
        public List<Reference> PossibleDuplicateReferences { get; set; } = new List<Reference>();
        public List<Reference> UserUpdatedReferences { get; set; } = new List<Reference>();

    }
}
