using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingRemoveCRCode
{
    class LoanDetail
    {
        public string DateFirstDisb { get; set; }
        public string InterestRate { get; set; }
        public string OrigPrincipal { get; set; }
        public string TotalAmtPaidToPri { get; set; }
        public string TotalAmtPaidToInt { get; set; }
        public string TotalAmtPaid { get; set; }
        public string CurrentBalance { get; set; }
    }
}
