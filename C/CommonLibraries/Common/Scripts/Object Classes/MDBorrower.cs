using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.Scripts
{
    /// <summary>
    /// This class is for using with ScriptBase as it is called from MauiDUDE
    /// Please do not use this class for anything else or make changes to it as it could break MD
    /// The object is filled out by MD and sent into the scriptbase args when MD starts a script
    /// </summary>
    public class MDBorrower
    {
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string DOB { get; set; } //this is a date string
        public MDBorrowerDemographics CompassDemos { get; set; }
        public MDBorrowerDemographics UserProvidedDemos { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal DailyInterest { get; set; }
        public List<string> LoanProgramsDistinctList { get; set; }
        public decimal AmountPastDue { get; set; }
        public decimal CurrentAmountDue { get; set; }
        public decimal OutstandingLateFees { get; set; }
        public decimal MonthlyPaymentAmount { get; set; }
        public bool HasRepaymentSchedule { get; set; }
    }
}
