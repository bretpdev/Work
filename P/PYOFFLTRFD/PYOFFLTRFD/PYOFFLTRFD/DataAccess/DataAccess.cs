using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PYOFFLTRFD
{
    public class DataAccess
    {

        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }


        /// <summary>
        /// Gets a list of all the Payoff data for the borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "PYOF_GetLoanInformation")]
        public List<LoanSelection> GetLoanSelectionData(string accountNumber) => LDA.ExecuteList<LoanSelection>("PYOF_GetLoanInformation", DataAccessHelper.Database.Cdw,
                SP("DF_SPE_ACC_ID", accountNumber)).Result;

        /// <summary>
        /// Gets the borrowers demos
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cdw, "GetSystemBorrowerDemographics")]
        public SystemBorrowerDemographics GetDemos(string accountIdentifier) => LDA.ExecuteSingle<SystemBorrowerDemographics>("GetSystemBorrowerDemographics", DataAccessHelper.Database.Cdw,
                SP("AccountIdentifier", accountIdentifier)).Result;

        private SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}