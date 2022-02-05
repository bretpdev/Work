using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class ServicingLoanDetail
    {
        public int LoanSeqNum { get; set; }
        public string LoanType { get; set; }
        public string FirstDisbursementDate { get; set; }
        public decimal OriginalBalance { get; set; }
        public decimal CurrentPrincipal { get; set; }
        public decimal InterestRate { get; set; }
        public string RepaymentType { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string SeparationDate { get; set; }
        public string GraceEndDate { get; set; }
        public string RepaymentStartDate { get; set; }
        public string Disclosure { get; set; }
        public string SchoolName { get; set; }
        public string BillMethod { get; set; }
        public int RepaymentTerm { get; set; }
        public string DueDate { get; set; }
        public decimal StatutoryInterestRate { get; set; }
        public int OnTimeMonthlyPayments { get; set; }
        public int RequiredOnTimePayments { get; set; }
        public string RPSDate { get; set; }
        public string PaidAhead { get; set; }
        public decimal RegInt { get; set; }

        private string _onACH;
        public string OnACH
        {
            get 
            {
                if(_onACH == "A")
                {
                    return "Y";
                }
                else
                {
                    return "N";
                }
            }
            set
            {
                _onACH = value;
            }
        }

        public string ACHEligible { get; set; }
        public decimal ACHRate { get; set; }
        public string RIRCount { get; set; }
        public string RIRInt { get; set; }
        public string RIRType { get; set; }
        public string HEP { get; set; }
        public string RIREligibility { get; set; }
        public string RIREligibilityDate { get; set; }
    }
}
