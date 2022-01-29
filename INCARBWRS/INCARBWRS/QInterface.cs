using Q;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;



namespace INCARBWRS

{

    public class QInterface : Q.ScriptBase

    {

        public MDBorrower Borrower { get; set; }



        public QInterface(Q.ReflectionInterface ri, Q.MDBorrower bor, int runNumber)
            : base(ri, "INCARBWRS", bor, runNumber)
        {
            Borrower = bor;
        }

        public override void Main()
        {
            Uheaa.Common.DataAccess.DataAccessHelper.CurrentMode = RI.TestMode ? Uheaa.Common.DataAccess.DataAccessHelper.Mode.Dev : Uheaa.Common.DataAccess.DataAccessHelper.Mode.Live;
            Uheaa.Common.Scripts.ReflectionInterface reflection = new Uheaa.Common.Scripts.ReflectionInterface(RI.ReflectionSession);
            BorrowerData data = new BorrowerData()
            {
                Ssn = Borrower.SSN,
                AccountNumber = Borrower.UserProvidedDemos.AccountNumber,
                FirstName = Borrower.FirstName,
                LastName = Borrower.LastName,
                Address1 = Borrower.UserProvidedDemos.Addr1,
                Address2 = Borrower.UserProvidedDemos.Addr2,
                City = Borrower.UserProvidedDemos.City,
                State = Borrower.UserProvidedDemos.State,
                Zip = Borrower.UserProvidedDemos.Zip,
                DemosLoaded = true
            };



            IncarceratedBorrowers obj = new IncarceratedBorrowers(reflection, Borrower.SSN);

            obj.Main();

        }

    }

}