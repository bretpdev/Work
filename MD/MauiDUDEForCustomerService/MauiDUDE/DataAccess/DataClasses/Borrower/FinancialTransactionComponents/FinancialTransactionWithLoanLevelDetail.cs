using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class FinancialTransactionWithLoanLevelDetail : FinancialTransaction
    {
        public List<FinancialTransaction> LoanLevelTransactions { get; set; } = new List<FinancialTransaction>();
    }
}
