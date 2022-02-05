using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class ACHInformation
    {
        private string _directDebit;
        public string DirectDebit
        {
            get
            {
                if(_directDebit == "A")
                {
                    return "YES";
                }
                else
                {
                    return "NO";
                }
            }

            set
            {
                _directDebit = value;
            }

        }

        public string StatusDate { get; set; }
        public int SequenceNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public decimal AdditionalWithdrawalAmount { get; set; }
        public int NSFCounter { get; set; }
        public string DenialReason { get; set; }
        public List<ACHLoanData> LoansOnACH { get; set; }
        public List<ACHLoanData> LoansNotOnACH { get; set; }
    }
}
