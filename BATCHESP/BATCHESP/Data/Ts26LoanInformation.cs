using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATCHESP
{
    /// <summary>
    /// Loan level information pulled back from the DB.
    /// This data is central to organizing the other loan-level data around, 
    /// such as deferments, enrollments, disbursements, etc.
    /// </summary>
    public class Ts26LoanInformation
    {
        public int Ts26LoanInformationId { get; set; }
        public string BorrowerSsn { get; set; }
        public int? LoanSequence { get; set; }
        public string LoanProgramType { get; set; }
        public decimal? CurrentPrincipal { get; set; }
        public DateTime? DisbursementDate { get; set; }
        public DateTime? GraceEndDate { get; set; }
        public string LoanStatus { get; set; }
        public DateTime? RepaymentStartDate { get; set; }
        public DateTime? TermBeg { get; set; }
        public DateTime? TermEnd { get; set; }
        public string RehabRepurch { get; set; }
        public DateTime? EffectAddDate { get; set; }
    }
}
