using System;

namespace ACHSETUP
{
    class Loan
    {
        public double Balance { get; set; }
        public DateTime FirstDisbDate { get; set; }
        public string Program { get; set; }
        public int Sequence { get; set; }
        public bool WasOriginallyAch { get; set; }
    }
}