using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCRAINTUP
{
    class BorrowerData
    {
        public string AccountNumber { get; set; }
        public string Ssn { get; set; }
        public DateTime ScraBeginDate { get; set; }
        public DateTime? ScraEndDate { get; set; }
        public List<BorrowerLoanData> EligibleLoanData { get; set; }
        public List<BorrowerLoanData> NonEligibleLoanData { get; set; }

        /// <summary>
        /// Populates a BorrowerData object based upon the data passed in
        /// </summary>
        /// <param name="accnumberNumber">Borrowers Account Number</param>
        /// <param name="ssn">Borrowers Ssn</param>
        /// <param name="scraBeginDate">Begin date gathered from the queue task</param>
        /// <param name="scraEndDate">End Date gathered from queue task if the date is null the value will be 1 year from today.</param>
        /// <param name="eliLoans">List of all loans eligible for SCRA</param>
        /// <param name="nonELiLoans">List of loans not eligible for SCRA</param>
        /// <returns>BorrowerData Object</returns>
        public static BorrowerData Populate(string accnumberNumber,string ssn, DateTime scraBeginDate, DateTime? scraEndDate, List<BorrowerLoanData> eliLoans, List<BorrowerLoanData> nonELiLoans)
        {
            return new BorrowerData()
            {
                AccountNumber = accnumberNumber,
                Ssn = ssn,
                ScraBeginDate = scraBeginDate,
                ScraEndDate = scraEndDate == null ? DateTime.Now.AddYears(1) : scraEndDate,
                EligibleLoanData = eliLoans,
                NonEligibleLoanData = nonELiLoans
            };
        }
    }
}
