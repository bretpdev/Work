using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SKIPACTS
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetAccountNumberFromSSN")]
        public string GetAccountNumberFromSsn(string ssn)
        {
            string result = LogRun.LDA.ExecuteSingle<string>("spGetAccountNumberFromSSN", DataAccessHelper.Database.Udw, SqlParams.Single("Ssn", ssn)).CheckResult();
            return result.IsNullOrEmpty() ? "" : result;
        }
    }
}
