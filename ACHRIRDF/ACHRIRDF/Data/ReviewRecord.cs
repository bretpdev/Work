using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACHRIRDF
{
    public class ReviewRecord
    {
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AccountNumber { get; set; }
        public List<int> LoanSequences { get; set; } = new List<int>();

        public ReviewRecord(DateTime? beginDate, DateTime? endDate, string accountNumber, int loanSequence)
        {
            BeginDate = beginDate;
            EndDate = endDate;
            AccountNumber = accountNumber;
            LoanSequences.Add(loanSequence);
        }

        public string GetCommaDelimitedLoanSequences()
        {
            string val = "";
            LoanSequences.Sort();
            foreach(int seq in LoanSequences)
            {
                val += seq.ToString() + ",";
            }
            val = val.Remove(val.Length - 1);
            return val;
        }
    }
}
