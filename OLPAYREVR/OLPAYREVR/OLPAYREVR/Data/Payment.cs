using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLPAYREVR
{
    public class Payment
    {
        public int? ProcessingQueueId { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; } 
        public double? PaymentAmount { get; set; }
        public DateTime? PaymentEffectiveDate { get; set; } 
        public string PaymentType { get; set; }
        public DateTime? PaymentPostDate { get; set; }

        public override string ToString()
        {
            string paymentAmount = PaymentAmount.HasValue ? PaymentAmount.Value.ToString() : "";
            string paymentEffectiveDate = PaymentEffectiveDate.HasValue ? PaymentEffectiveDate.Value.ToString("MM/dd/yyyy") : "";
            string paymentPostDate = PaymentPostDate.HasValue ? PaymentPostDate.Value.ToString("MM/dd/yyyy") : "";
            string processingQueueId = ProcessingQueueId.HasValue ? ProcessingQueueId.Value.ToString() : "";
            return $"Account Number:{AccountNumber}, Payment Amount:{paymentAmount}, Payment Effective Date:{paymentEffectiveDate}, Payment Post Date:{paymentPostDate} Payment Type:{PaymentType}, ProcessingQueueId:{processingQueueId}";
        }
    }
}
