using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGHISTFED
{
    public class BorrowerIndexRecord
    {
        public string Ssn { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DocType { get; set; }
        public string LoanId { get; set; }
        public string DocDate { get; set; }
        public string LoanProgramType { get; set; }
        public string GuarantyDate { get; set; }
        public string SaleDate { get; set; }
        public string DealId { get; set; }
    }
}
