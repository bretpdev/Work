using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMPMTHIST
{
    public class BorrowerPaymentInformation
    {

        public string SSN { get; set; }
        public string BorrowerName { get; set; }
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public BalanceInformation BalanceInfo { get; set; } = new BalanceInformation();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
