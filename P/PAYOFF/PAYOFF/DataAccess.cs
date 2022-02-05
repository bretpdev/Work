using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PAYOFF
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        /// <summary>
        /// Gets the borrowers demos
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetDemos(string accountIdentifier)
        {
            return LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", DataAccessHelper.Database.Udw, SP("AccountIdentifier", accountIdentifier)).Result;
        }

        /// <summary>
        /// Gets a list of all the Payoff data for the borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "payoff.GetLoanInformation")]
        public List<Loan> GetLoanInformation(string accountNumber)
        {
            return LDA.ExecuteList<Loan>("payoff.GetLoanInformation", DataAccessHelper.Database.Udw, SP("AccountNumber", accountNumber)).Result;
        }

        /// <summary>
        /// Gets a list of all the Payoff data for the borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Udw, "payoff.GetCoborrowerLoanInformation")]
        public List<CoborrowerLoan> GetCoborrowerLoanInformation(string accountNumber)
        {
            return LDA.ExecuteList<CoborrowerLoan>("payoff.GetCoborrowerLoanInformation", DataAccessHelper.Database.Udw, SP("AccountNumber", accountNumber)).Result;
        }

        private SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
