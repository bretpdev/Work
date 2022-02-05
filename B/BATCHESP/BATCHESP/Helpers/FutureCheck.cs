using System.Collections.Generic;

namespace BATCHESP
{
   public class FutureCheck
    {
        public EspEnrollment Esp { get; set; }
        public List<Ts26LoanInformation> Ts26s { get; set; }
        public List<TsayDefermentForbearance> Dfs { get; set; }
        public List<ParentPlusLoanDetailsInformation> Pplus { get; set; }
        public FutureCheck(EspEnrollment esp, List<Ts26LoanInformation> ts26s, List<TsayDefermentForbearance> dfs, List<ParentPlusLoanDetailsInformation> pplus)
        {
            Esp = esp;
            Ts26s = ts26s;
            Dfs = dfs;
            Pplus = pplus;
        }
    }
}
