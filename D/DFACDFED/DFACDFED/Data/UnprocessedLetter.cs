using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace DFACDFED
{
    public class UnprocessedLetter
    {
        public int LetterRequestId { get; set; }
        public string AccountNumber { get; set; }
        public string LetterId { get; set; }
        public bool Ecorr { get; set; }
        public int LetterSequence { get; set; }
        public string Ssn { get; set; }
    }
}
