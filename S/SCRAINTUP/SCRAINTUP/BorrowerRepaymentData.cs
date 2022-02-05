using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace SCRAINTUP
{
    class BorrowerRepaymentData
    {
        public string AccountNumber { get; set; }
        public int LoanSeq { get; set; }
        public string ScheduleType { get; set; }
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Executes GetRepaymentInformationForBorrower to populate the list.
        /// </summary>
        /// <param name="accountNumber">Borrowers Account Number</param>
        /// <returns>List of BorrowerRepaymentdata</returns>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetRepaymentInformationForBorrower")]
        public static List<BorrowerRepaymentData> Populate(string accountNumber)
        {
            return DataAccessHelper.ExecuteList<BorrowerRepaymentData>("GetRepaymentInformationForBorrower",
                DataAccessHelper.Database.Udw, accountNumber.ToSqlParameter("AccountNumber"));
        }
    }
}
