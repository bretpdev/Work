using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPERM
{
    public class PossibleReferenceMatch
    {
        public string ReferenceNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Select { get; set; }
        public bool IsAuthed { get; set; }
    }
}
