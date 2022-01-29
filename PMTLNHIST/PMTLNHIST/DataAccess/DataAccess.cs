using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace PMTLNHIST
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) =>
            LDA = lda;

        /// <summary>
        /// Gets the borrowers Loan Data and inserts it into the LoanData table in OLS.pmtlnhist
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        [UsesSproc(Ols, "pmtlnhist.InsertLoanData")]
        public void InsertLoanData(string accountNumber) =>
            LDA.Execute("pmtlnhist.InsertLoanData", Ols,
                Sp("AccountNumber", accountNumber));

        /// <summary>
        /// Inserts the borrower data into the BorrowerData table in OLS.pmtlnhist
        /// </summary>
        /// <returns>The SCOPE_IDENTITY of the inserted data</returns>
        [UsesSproc(Ols, "pmtlnhist.InsertBorrowerData")]
        public void InsertBorrowerData(BorrowerData borrowerData) =>
            LDA.Execute("pmtlnhist.InsertBorrowerData", Ols,
                Sp("AccountNumber", borrowerData.AccountNumber),
                Sp("Name", borrowerData.Name),
                Sp("Principal", borrowerData.Principal),
                Sp("Interest", borrowerData.Interest),
                Sp("LegalCosts", borrowerData.LegalCosts),
                Sp("OtherCosts", borrowerData.OtherCosts),
                Sp("CollectionCosts", borrowerData.CollectionCosts),
                Sp("ProjectedCollectionCosts", borrowerData.ProjectedCollectionCosts));

        private SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}