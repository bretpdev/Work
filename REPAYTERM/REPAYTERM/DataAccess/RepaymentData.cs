using System;
using Uheaa.Common;

namespace REPAYTERM
{
    public class RepaymentData
    {
        public string Ssn { get; set; }
        public string Name
        {
            get
            {
                return $"{FirstName} {(MiddleInitial.IsPopulated() ? $"{MiddleInitial} " : "")}{LastName}";
            }
        }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public double Payoff { get; set; }
        public double Balance { get; set; }
        public double Interest { get; set; }
        public double WeightedRate { get; set; }
        public bool HasSC { get; set; }
        public bool HasNewLoan { get; set; }
        public bool HasExtendedLoans { get; set; }
        public DateTime? PayoffDate { get; set; }
    }
}