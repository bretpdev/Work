using System;
using Uheaa.Common.DataAccess;

namespace CLSCHLLNFD
{
    public class LoanDetail
    {
        [DbName("Loan Program")]
        public string LoanProgram { get; set; }
        [DbName("Disbursement Date")]
        public DateTime FirstDisbursementDate { get; set; }
        [DbName("Current Principal Balance")]
        public double CurrentBalance { get; set; }
    }
}