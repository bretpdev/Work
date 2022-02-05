using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace PAYOFF
{
    public class Borrower
    {
        public SystemBorrowerDemographics Demos { get; set; }
        public string PayoffDate { get; set; }
        public List<LoanPayoffData> LoanPayoffRecords { get; set; }
        public bool DemosLoaded { get; set; }

        public Borrower()
        {
            LoanPayoffRecords = new List<LoanPayoffData>();
            Demos = new SystemBorrowerDemographics();
        }

    }
}
