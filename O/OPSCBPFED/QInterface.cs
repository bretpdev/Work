using Q;
using System;

namespace OPSCBPFED
{
    public class QInterface : Q.FedScriptBase
    {
        public MDBorrower Borrower { get; set; }

        public QInterface(Q.ReflectionInterface ri, Q.MDBorrower bor, int runNumber)
            : base(ri, "OPSCBPFED", Region.CornerStone, bor, runNumber)
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
                AccountNumber = Borrower.CLAccNum,
                FirstName = Borrower.FirstName,
                LastName = Borrower.LastName,
                Address1 = Borrower.UserProvidedDemos.Addr1,
                Address2 = Borrower.UserProvidedDemos.Addr2,
                City = Borrower.UserProvidedDemos.City,
                State = Borrower.UserProvidedDemos.State,
                Zip = Borrower.UserProvidedDemos.Zip,
                Demos = Borrower.UserProvidedDemos,
                ScriptInfoToGenericBusinessUnit = Borrower.ScriptInfoToGenericBusinessUnit,
                AmountPastDue = Borrower.AmountPastDue,
                DemosLoaded = true,
                LoanPrograms = string.Join(",", Borrower.LoanProgramsDistinctList.ToArray()),
                DaysDelq = Borrower.NumDaysDelinquent,
                TotalBalance = double.Parse(Borrower.Principal + Borrower.Interest + decimal.Parse(Borrower.TotalLateFeesDue.ToString()).ToString())
            };

            OPSCBPFED run = new OPSCBPFED(reflection, data);
            run.Main();
        }
    }
}