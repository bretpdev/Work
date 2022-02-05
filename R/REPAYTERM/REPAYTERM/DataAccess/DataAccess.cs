using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace REPAYTERM
{

    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) =>
            LDA = lda;

        /// <summary>
        /// Gets the borrowers repayment data to display
        /// </summary>
        [UsesSproc(Odw, "repayterm.GetRepaymentData")]
        public RepaymentData GetRepayData(string accountIdentifier) =>
            LDA.ExecuteSingle<RepaymentData>("repayterm.GetRepaymentData", Odw, Sp("AccountIdentifier", accountIdentifier)).Result;

        public SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}