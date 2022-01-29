using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    class RedisclosureExemptionData
    {
        public DateTime? CreateDate10 { get; set; }

        public bool HasAllLoansInExemptStatus { get; set; }
        public bool IsInExemptForbType { get; set; }
        public bool IsInExemptScheduleType { get; set; }
        public bool IsInNonExemptScheduleType { get; set; }
        public bool HasUndisclosedSetupArc { get; set; }
        public bool HasUndisclosedRepayOptionsArc { get; set; }
        public bool RedisclosedAfterR0Date { get; set; }
        public bool HasAllLoansDeconverted { get; set; }
        public bool HasAllLoansPaidInFull { get; set; }
        public bool IsInFixedAlternativeScheduleType { get; set; }
        public bool AllExemptScheduleTypes { get; set; }
        public bool HasAllLoansInLitigation { get; set; }
    }
}
