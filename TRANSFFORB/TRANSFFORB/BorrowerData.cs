using System;
using System.Collections.Generic;

namespace TRANSFFORB
{
    class BorrowerData
    {
        public string AccountNumber { get; set; }
        public List<int> LoanSeq { get; set; }
        public string DeliqDate { get; set; }
        public string LoanDate { get; set; }
    }
}
