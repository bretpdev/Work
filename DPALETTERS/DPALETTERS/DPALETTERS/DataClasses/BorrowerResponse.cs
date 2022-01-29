using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DPALETTERS.Utilities;

namespace DPALETTERS
{
    public class BorrowerResponse
    {
        public Borrower Borrower { get; set; }
        public ProcessLoop Response { get; set; }

    }
}
