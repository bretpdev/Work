using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLDSCONSO
{
    public class BorrowerDetails
    {
        public List<BorrowerUnderlyingLoan> UnderlyingLoans { get; set; }
        public List<BorrowerUnderlyingLoan> AlternatelyCalculatedUnderlyingLoans { get; set; }
        public List<BorrowerConsolidationLoan> ConsolidationLoans { get; set; }
        public List<Grsp> Grsps { get; set; }
        public List<Grs2> Grs2s { get; set; }
        public decimal MaxTotalAmount { get; set; }
        public class BorrowerUnderlyingLoan
        {
            public int BorrowerUnderlyingLoanId { get; set; }
            public string NsldsLabel { get; set; }
            public string NewLoanId { get; set; }
            public string UnderlyingLoanId { get; set; }
            public string LoanType { get; set; }
            public decimal TotalAmount { get; set; }
            public DateTime? DateFunded { get; set; }
            public DateTime FirstDisbursement { get; set; }
        }
        public class BorrowerConsolidationLoan
        {
            public string NewLoanId { get; set; }
            public decimal GrossAmount { get; set; }
            public decimal InterestRate { get; set; }
            public int Fs10LoanSequence { get; set; }
        }
        public class Grsp
        {
            public string LoanProgram { get; set; }
            public string NsldsLabel { get; set; }
            public string AwardId { get; set; }
            public int AwardSequence { get; set; }
        }
        public class Grs2
        {
            public DateTime? DisbursementDate { get; set; }
            public int DisbursementSequence { get; set; }
            public string NsldsLabel { get; set; }
        }

        public void Sanitize()
        {
            foreach (var grsp in Grsps)
                grsp.LoanProgram = grsp.LoanProgram.Trim();
            foreach (var bul in UnderlyingLoans)
            {
                bul.LoanType = bul.LoanType.Trim();
                bul.UnderlyingLoanId = bul.UnderlyingLoanId.Trim();
            }
        }
    }
}
