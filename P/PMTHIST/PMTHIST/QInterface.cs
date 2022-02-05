using Q;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMTHIST
{
    public class QInterface : Q.ScriptBase
    {
        public Q.MDBorrower Borrower { get; set; }

        public QInterface(Q.ReflectionInterface ri, Q.MDBorrower bor, int runNumber)      
            :base(ri, "PMTHIST", bor, runNumber)
        {
            Borrower = bor;
        }

        public override void Main()
        {
            Uheaa.Common.DataAccess.DataAccessHelper.CurrentMode = RI.TestMode ? Uheaa.Common.DataAccess.DataAccessHelper.Mode.Dev : Uheaa.Common.DataAccess.DataAccessHelper.Mode.Live;
            Uheaa.Common.Scripts.ReflectionInterface reflection = new Uheaa.Common.Scripts.ReflectionInterface(RI.ReflectionSession);

            string ssn = Borrower.SSN;

            new PMTHIST.PaymentHistory(reflection, ssn).Main();
        }
    }
}
