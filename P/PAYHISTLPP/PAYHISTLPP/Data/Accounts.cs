using System.Collections.Generic;
using Uheaa.Common;

namespace PAYHISTLPP
{
    public class Accounts
    {
        public int AccountsId { get; set; }
        public string Ssn { get; set; }
        public string Lender { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return $"{FirstName}{(MiddleInitial.IsPopulated() ? $" {MiddleInitial}" : "")} {LastName}";
            }
        }
        //public List<EA80Data> Ea80Data { get; set; }
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public string FileName { get; set; }
    }
}