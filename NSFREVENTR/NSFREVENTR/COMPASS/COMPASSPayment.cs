using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;

namespace NSFREVENTR
{
    public class COMPASSPayment : ListViewItem
    {
        public string BatchCode { get; set; }
        public string EffectiveDate { get; set; }
        public string Batch { get; set; }
        public string RemittanceAmount { get; set; }
        public string TransactionType { get; set; }
        public bool UserSelected { get; set; }
        public string PostedDate { get; set; }

        public COMPASSPayment(string batchCode, string effectiveDate, string batch, string remittanceAmount, string transactionType, string postedDate)
            : base(new string[] { effectiveDate, remittanceAmount, transactionType, postedDate })
        {
            BatchCode = batchCode;
            EffectiveDate = effectiveDate;
            Batch = batch;
            RemittanceAmount = remittanceAmount;
            TransactionType = transactionType;
            UserSelected = false;
            PostedDate = postedDate;
        }
    }
}
