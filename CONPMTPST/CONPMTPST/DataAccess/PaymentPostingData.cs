using System.Collections.Generic;

namespace CONPMTPST
{
    public class PaymentPostingData
    {
        public double PaymentAmount { get; set; }
        public bool IsCash { get; set; }
        public PaymentSources PaymentSource { get; set; }
        public List<BorrowerData> BorData { get; set; }

        public PaymentPostingData()
        {
            PaymentSource = new PaymentSources();
            BorData = new List<BorrowerData>();
        }
    }
}