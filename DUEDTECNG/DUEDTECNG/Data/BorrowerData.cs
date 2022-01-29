using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUEDTECNG
{
    public class BorrowerData
    {
        public int DueDateChangeId { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string DueDate {get;set;}
        public decimal TotalBalance { get; set; }
        public decimal MonthlyInterest { get; set; }
        public DateTime FirstDisbursement { get; set; }
    }
}
