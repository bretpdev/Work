using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace IMGHISTFED
{
    public class RepaymentInfo : RFile
    {
        public string BorrowerSsn { get; set; }
        public string BorrowerName { get; set; }
        public string NextPaymentDue { get; set; }
        public string LoanProgram { get; set; }
        public string DisbursementDate { get; set; }
        public string ScheduleStatus { get; set; }
        public string ScheduleType { get; set; }
        public string ScheduleFirstDueDate { get; set; }
        public string GradationSequence { get; set; }
        public string Term { get; set; }
        public string Payment { get; set; }

        protected override void AfterParse(List<string> fields)
        {
            int? days = NextPaymentDue.ToIntNullable();
            if (days == null)
                NextPaymentDue = null;
            else
                NextPaymentDue = DateTime.Now.AddDays(days.Value).ToShortDateString();

            DateTime? date = DisbursementDate.ToDateNullable();
            if (date.HasValue)
                DisbursementDate = date.Value.ToShortDateString();
            else
                DisbursementDate = null;

            date = ScheduleFirstDueDate.ToDateNullable();
            if (date.HasValue)
                ScheduleFirstDueDate = date.Value.ToShortDateString();
            else
                ScheduleFirstDueDate = null;

            Payment = "$" + Payment;
        }

    }
}
