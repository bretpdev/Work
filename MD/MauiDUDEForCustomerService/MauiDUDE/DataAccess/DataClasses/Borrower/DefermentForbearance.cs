using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class DefermentForbearance
    {
        public string Type { get; set; }
        public string TypeCode { get; set; }
        public string CertDate { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string CapInd { get; set; }
        public string MonthsUsed { get; set; }
        public int OneofTheLoanSequenceNumbersAssociatedWithDeferOrForb { get; set; }
    }
}
