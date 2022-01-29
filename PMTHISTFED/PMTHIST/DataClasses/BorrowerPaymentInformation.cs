using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMTHISTFED
{
    public class BorrowerPaymentInformation
    {

        public string AccountNumber { get; set; }
        public string BorrowerName { get; set; }
        public List<Loan> Loans { get; set; }
        public BalanceInformation BalanceInfo { get; set; }
        public List<Transaction> Transactions { get; set; }

        public BorrowerPaymentInformation()
        {
            AccountNumber = string.Empty;
            BorrowerName = string.Empty;
            Loans = new List<Loan>();
            BalanceInfo = new BalanceInformation();
            Transactions = new List<Transaction>();
        }

    }
}
