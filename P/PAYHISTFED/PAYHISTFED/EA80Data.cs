using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAYHISTFED
{
    public class EA80Data
    {
        public string BF_SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocType { get; set; }
        public string LoanId { get; set; }

        public string DocDate
        {
            get; set;
        }

        public string LoanProgramType { get; set; }

        public string GuarantyDate { get; set; }
        public string SaleDate { get; set; }
        public string DealId { get; set; } //PSAOH for LNC
        public string FileName { get; set; }

        public int LP { get; set; }


        //var indexPieces = new string[] { borrower.Ssn, borrower.LastName, borrower.FirstName, document.DOC_TYPE,
        //                        borrower.LoanId ?? document.LoanId, document.DOC_DATE?.ToString("MM/dd/yyyy"), "DIRL", document.VS_DTWHEN.ToString("MM/dd/yyyy"), DTA.SaleDateFormatted, DTA.DealId, filename  };
        //indexLines.Add(new IndexLineAndLocation() { IndexLine = string.Join("|", indexPieces), VS_LOCATION = document.VS_LOCATION });

    }
}
