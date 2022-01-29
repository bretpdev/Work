using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public class DeleteAndReapplyRowData
    {
   
        public string LoanSequenceNumber { get; set; }
        public string Amount { get; set; }
        public string DisbursementDate { get; set; }
        public string TransactionType { get; set; }

        public DeleteAndReapplyRowData()
        {
            LoanSequenceNumber = string.Empty;
        }

    }
}
