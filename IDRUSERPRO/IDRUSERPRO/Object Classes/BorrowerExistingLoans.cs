using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    public class BorrowerExistingLoans
    {
        public List<Ts26Loans> AllLoans { get; private set; }
        public List<Ts26Loans> FilteredLoans
        {
            get
            {
                return AllLoans.Where(o => o.HasBalance).ToList();
            }
        }
        public List<Ts26Loans> EligibleLoans
        {
            get
            {
                return FilteredLoans.Where(o => o.IsEligible).ToList();
            }
        }
        public BorrowerExistingLoans(List<Ts26Loans> loans)
        {
            AllLoans = loans;
        }
    }
}
