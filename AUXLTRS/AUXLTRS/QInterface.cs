using Q;
using System.Collections.Generic;

namespace AUXLTRS
{
    public class QInterface : Q.ScriptBase
    {
        public Q.MDBorrower Borrower { get; set; }
        public Uheaa.Common.Scripts.ReflectionInterface SRF {  get; set; }

        public QInterface(Q.ReflectionInterface ri, Q.MDBorrower bor, int runNumber)
            :base(ri, "AUXLTRS", bor, runNumber)
        {
            Uheaa.Common.Scripts.ReflectionInterface reflection = new Uheaa.Common.Scripts.ReflectionInterface(RI.ReflectionSession);
            Borrower = bor;
        }

        public override void Main()
        {
            Uheaa.Common.DataAccess.DataAccessHelper.CurrentMode = RI.TestMode ? Uheaa.Common.DataAccess.DataAccessHelper.Mode.Dev : Uheaa.Common.DataAccess.DataAccessHelper.Mode.Live;
            Uheaa.Common.Scripts.ReflectionInterface reflection = new Uheaa.Common.Scripts.ReflectionInterface(RI.ReflectionSession);

            AUXLTRS.Object_Classes.BorrowerData data = new AUXLTRS.Object_Classes.BorrowerData()
            {
                Ssn = Borrower.SSN,
                AccountNumber = Borrower.UserProvidedDemos.AccountNumber,
                Address1 = Borrower.UserProvidedDemos.Addr1,
                Address2 = Borrower.UserProvidedDemos.Addr2,
                City = Borrower.UserProvidedDemos.City,
                State = Borrower.UserProvidedDemos.State,
                Zip = Borrower.UserProvidedDemos.Zip
            };

            new AUXLTRS.CollectorLetters(reflection, Borrower, 0).Main();
        }
    }
}
