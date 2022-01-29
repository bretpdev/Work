using Q;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYOFF
{
    public class QInterface : Q.ScriptBase
    {
        public MDBorrower Borrower { get; set; }

        public QInterface(Q.ReflectionInterface ri, Q.MDBorrower bor, int runNumber)
            : base(ri, "PAYOFF", bor, runNumber)
        {
            Borrower = bor;
        }

        public override void Main()
        {
            Uheaa.Common.DataAccess.DataAccessHelper.CurrentMode = RI.TestMode ? Uheaa.Common.DataAccess.DataAccessHelper.Mode.Dev : Uheaa.Common.DataAccess.DataAccessHelper.Mode.Live;
            Uheaa.Common.Scripts.ReflectionInterface reflection = new Uheaa.Common.Scripts.ReflectionInterface(RI.ReflectionSession);

            Borrower data = new Borrower();
            data.Demos.Ssn = Borrower.SSN;
            data.Demos.FirstName = Borrower.FirstName;
            data.Demos.LastName = Borrower.LastName;
            data.Demos.Address1 = Borrower.UserProvidedDemos.Addr1;
            data.Demos.Address2 = Borrower.UserProvidedDemos.Addr2;
            data.Demos.City = Borrower.UserProvidedDemos.City;
            data.Demos.State = Borrower.UserProvidedDemos.State;
            data.Demos.ZipCode = Borrower.UserProvidedDemos.Zip;
            data.LoanPayoffRecords = new List<LoanPayoffData>();
            data.DemosLoaded = true;

            new PAYOFF.PayoffProcessor(reflection, data).Main();
            //PayOffLetterFed payoff = new PayOffLetterFed(reflection, data);
            //payoff.Main();
        }
    }
}
