using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPLTRS.Rehabilitation.DataObjects
{
    public class LoanDataRow
    {
        public string Clm { get; set; }
        public double? Pri { get; set; }
        public double? Int { get; set; }
        public double? Col { get; set; }
        public double? Fee { get; set; }
        public double? Tot { get; set; }

        public double? RateByLoan { get; set; }
        public double? ColCostProjByLoan { get; set; }
        public string UniqueId { get; set; }
    }
}
