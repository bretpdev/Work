using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CNCRTRNSFR
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "cncrtrnsfr.GetDefForbData")]
        public List<Borrower> GetDefForbData(string ssn)
        {
            return LogRun.LDA.ExecuteList<Borrower>("cncrtrnsfr.GetDefForbData", DataAccessHelper.Database.Cdw,
                SqlParams.Single("Ssn", ssn)).Result;
        }
    }
}