using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class TaskEoj
    {
        public R0Task Task { get; set; }
        public EojReport EojType { get; set; }
        public DateTime? R0CreateDate { get; set; }
        public DateTime? CreateDate10 { get; set; }
        public string CancelReason { get; set; }
        public decimal? OldMonthlyPayment { get; set; }
        public decimal? NewMonthlyPayment { get; set; }
        public bool ForceDisclosure { get; set; }

        public TaskResult GetTaskResult()
        {
            switch (EojType)
            {
                case EojReport.Cancelled:
                    return TaskResult.CancelTask;
                case EojReport.ExemptConditionSkipped:
                case EojReport.ForwardedSkipped:
                case EojReport.MaxLimitSkipped:
                case EojReport.PaymentsTooHighPrenotifications:
                    return TaskResult.SkipTask;
                case EojReport.Redisclosed:
                    return TaskResult.CompleteTask;
            }
            throw new Exception();
        }
    }

    public enum EojReport
    {
        Redisclosed = 1,
        PaymentsTooHighPrenotifications = 2,
        MaxLimitSkipped = 3,
        ExemptConditionSkipped = 4,
        ForwardedSkipped = 5,
        Cancelled = 6
    }
}
