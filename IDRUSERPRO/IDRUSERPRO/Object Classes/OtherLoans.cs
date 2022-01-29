using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using System.Linq;

namespace IDRUSERPRO
{
    public class OtherLoans
    {
        [DataGridView(0, "Application Id", false)]
        public int ApplicationId { get; set; }
        [DataGridView(1, "Loan Type", true)]
        public string LoanType { get; set; }
        [DataGridView(2, "Owner/Lender", true)]
        public string OwnerLender { get; set; }
        [DataGridView(3, "Outstanding Balance", true)]
        public decimal OutstandingBalance { get; set; }
        [DataGridView(4, "Outstanding Interest", true)]
        public decimal OutstandingInterest { get; set; }
        [DataGridView(5, "Monthly Payment", true)]
        public decimal? MonthlyPay { get; set; }
        [DataGridView(6, "Interest Rate", true)]
        public decimal? InterestRate { get; set; }
        [DataGridView(7, "Is FFELP", true)]
        public bool Ffelp { get; set; }
        [DataGridView(8, "Is Spouse", false)]
        public bool SpouseIndicator { get; set; }
        [DataGridView(11, "", false)]
        public string LF_FED_AWD { get; set; }
        [DataGridView(12, "", false)]
        public string DF_PRS_ID { get; set; }
        [DataGridView(13, "", false)]
        public int LN_FED_AWD_SEQ { get; set; }

        public OtherLoanCoordinator Coordinator { get; set; }

        public decimal CalculatedOutstandingBalance
        {
            get
            {
                bool spouseHasLoans = Coordinator?.SpouseLoans.Any() ?? false;
                if (spouseHasLoans)
                    return OutstandingBalance + OutstandingInterest;
                return OutstandingBalance;
            }
        }

        public void SetMonthlyPay()
        {
            if (CalculatedOutstandingBalance > 0 && InterestRate.HasValue && !MonthlyPay.HasValue)
                MonthlyPay = (decimal)Math.Round(Math.Abs(Financial.Pmt((((double)InterestRate.Value / 100) / 12), 120, (double)CalculatedOutstandingBalance)), 2);
        }

        public void SetFfelp(List<LoanPrograms> programs)
        {
            var isDirect = programs.SingleOrDefault(o => o.LoanProgram == LoanType)?.IsDirect ?? false;
            Ffelp = !isDirect;
        }
    }
}