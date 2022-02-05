using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORCPAYPOST
{
    public class FileData
    {
        public string AccountNumber { get; set; }
        public string BorrowersLastName { get; set; }
        public string PaymentEffectiveDate { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
