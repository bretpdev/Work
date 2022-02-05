using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BATCHESP
{
    public class Ts01Enrollment
    {
        public int Ts01EnrollmentId { get; set; }
        public string BorrowerSsn { get; set; }
        public int LoanSequence { get; set; }
        public string StudentSsn { get; set; }
        public DateTime? SeparationDate { get; set; }
        public string SchoolCode { get; set; }
        public string SeparationReason { get; set; }
        public string SeparationSource { get; set; }
        public DateTime? DateNotified { get; set; }
        public DateTime? DateCertified { get; set; }
    }
}
