using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    public class OtherLoanCoordinator
    {
        private List<OtherLoans> borrowerLoans = new List<OtherLoans>();
        private List<OtherLoans> spouseLoans = new List<OtherLoans>();
        public IReadOnlyList<OtherLoans> BorrowerLoans => borrowerLoans;
        public IReadOnlyList<OtherLoans> SpouseLoans => spouseLoans;
        public IReadOnlyList<OtherLoans> AllLoans => borrowerLoans.Union(spouseLoans).ToList();

        public void SetBorrowerLoans(List<OtherLoans> loans)
        {
            borrowerLoans = loans;
            foreach (var loan in loans)
                loan.Coordinator = this;
        }
        public void SetSpouseLoans(List<OtherLoans> loans)
        {
            spouseLoans = loans;
            foreach (var loan in loans)
                loan.Coordinator = this;
        }

        public void ClearBorrowers()
        {
            borrowerLoans = new List<OtherLoans>();
        }

        public void ClearSpouses()
        {
            spouseLoans = new List<OtherLoans>();
        }

        public void ClearLoans()
        {
            ClearBorrowers();
            ClearSpouses();
        }

    }
}
