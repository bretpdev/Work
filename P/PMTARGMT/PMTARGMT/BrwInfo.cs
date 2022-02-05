using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;

namespace Payments
{
    class BrwInfo
    {

        public enum PPAIOptions
        {
            ValNotSet = 0,
            PermPmtArrg = 1,
            FollowUp = 2
        }

        /// <summary>
        /// Through the magic of polymorphism BrwDemos handles both the SystemBorrowerDemographic object and/or MauiDUDEBorrowerDemographic object.  Pretty Cool!
        /// </summary>
        public BorrowerDemographics BrwDemos { get; set; }
        public string SSN { get; set; }
        public double APmt { get; set; }
        public string BillCd { get; set; }
        public DateTime HoldDate { get; set; }
        /// <summary>
        /// 0 = value not set, 
        /// 1 = perm pmt arrangement, 
        /// 2 = follow up to update billed amount when reduced arrangement expires
        /// </summary>
        public PPAIOptions PPAI { get; set; }
        public string EmployerID { get; set; }
        public int RehabPmts { get; set; }
        public string MinPmtOvrd { get; set; }
        public string ABal { get; set; }
        public string MoInt { get; set; }
        public string BilledAmt { get; set; }
        public string Balance { get; set; }
        public string NextDue { get; set; }
        public string MinEnd { get; set; }
        public string UpdateNextDue { get; set; }
        public double AmtLastPay { get; set; }  
        public double ARate { get; set; }
        public List<string> ClaimID { get; set; }
        public List<double> WT { get; set; }
        public string LastPayDate { get; set; }
        public double CurrExpPmt { get; set; }
        public bool ExtendedPaymentElig { get; set; }
        public string ArngTyp { get; set; }
        public string LC34DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpDate { get; set; }
        public string UpdateBillAmt { get; set; }

        public BrwInfo()
        {
            SSN = string.Empty;
            PPAI = BrwInfo.PPAIOptions.ValNotSet;
            EmployerID = string.Empty;
            MinPmtOvrd = string.Empty;
            ARate = 0;
            ClaimID = new List<string>();
            WT = new List<double>();
            ExtendedPaymentElig = true;
        }

    }
}
