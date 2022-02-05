using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace OLPAYREVR
{
    public class PaymentProcessor
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        private ReflectionInterface RI { get; set; }
        public List<Payment> ReversalErrorsList { get; private set; } = new List<Payment>(); //TODO: Waiting on BA to see if we want to write this out to a file or just look at PL for tracking errors

        public PaymentProcessor(ProcessLogRun logRun, DataAccess da, ReflectionInterface ri)
        {
            LogRun = logRun;
            DA = da;
            RI = ri;
        }

        public bool ProcessPaymentReversals(List<Payment> payments)
        {
            bool processingResult = true;
            foreach (Payment payment in payments)
            {
                processingResult &= ReversePayment(payment);
            }
            return processingResult;
        }

        private bool ReversePayment(Payment payment)
        {
            if (!payment.PaymentPostDate.HasValue)
            {
                string error = $"Unable to find post date for payment {payment}. Please process the reversal manually.";
                LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                DA.SetProcessed(payment.ProcessingQueueId, true, error);
                return false;
            }
            return UpdateSession(payment);
        }

        private bool UpdateSession(Payment payment)
        {
            RI.FastPath($"LC65C{payment.Ssn}");
            RI.PutText(7, 34, "PE");
            RI.PutText(8, 34, payment.PaymentPostDate.Value.ToString("MMddyyyy"));
            RI.PutText(9, 34, payment.PaymentEffectiveDate.Value.ToString("MMddyyyy"));
            RI.PutText(20, 3, $"Reversal of {payment.PaymentType} payment due to COVID DCL", ReflectionInterface.Key.Enter);
            string messageCode = GetSessionMessageCode(); // Message code appears on a different location than normal
            string error;

            if (messageCode == "47004") //47004 = NO RECORDS FOUND
            {
                error = $"Matching record on LC65C not found for payment {payment}. Session message: {GetSessionMessage()}";
                LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.SetProcessed(payment.ProcessingQueueId, true, error);
                return false;
            }

            if (messageCode == "46011") //46011 = MAKE DESIRED DATA CHANGES
            {
                double amountInSession = RI.GetText(9, 64, 12).ToDouble();
                if (amountInSession == payment.PaymentAmount.Value)
                {
                    RI.PutText(4, 69, payment.PaymentAmount.Value.ToString("F2"), true);
                    RI.PutText(9, 2, "Y", ReflectionInterface.Key.F6);
                    if (RI.AltMessageCode == "49000" || RI.GetText(21, 3, 5) == "49000") // 49000 = DATA SUCCESSFULLY UPDATED
                    {
                        DA.SetProcessed(payment.ProcessingQueueId);
                        return true;
                    }
                    error = $"The following payment was not successfully reversed on LC65C. {payment}. Session message: {GetSessionMessage()}";
                    LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.SetProcessed(payment.ProcessingQueueId, true, error);
                    return false;
                }
                else
                {
                    error = $"Payment amount differs on screen {RI.ScreenCode} for payment {payment}. Amount in session: {RI.GetText(9, 64, 12)}, expected amount: {payment.PaymentAmount.Value.ToString("F2")}";
                    LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.SetProcessed(payment.ProcessingQueueId, true, error);
                    return false;
                }
            }

            error = $"Unknown error encountered. Error hit while trying to reverse the payment {payment}. Session message: {GetSessionMessage()}";
            LogRun.AddNotification(error, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            DA.SetProcessed(payment.ProcessingQueueId, true, error);
            return false;
        }

        private string GetSessionMessage()
        {
            return RI.GetText(21, 3, 78) ?? "";
        }

        private string GetSessionMessageCode()
        {
            if (IsValidCode(RI.GetText(21, 3, 5)))
                return RI.GetText(21, 3, 5);

            if (IsValidCode(RI.AltMessageCode))
                return RI.AltMessageCode;

            if (IsValidCode(RI.MessageCode))
                return RI.MessageCode;

            return ""; // No code found
        }

        private bool IsValidCode(string messageCode)
        {
            if (messageCode.Length == 5 && messageCode.IsNumeric())
                return true;
            return false;
        }
    }
}
