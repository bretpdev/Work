using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace PHONE4RMVL
{
    public class DataAccess
    {
        private ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun LogRun)
        {
            this.LogRun = LogRun;
        }

        [UsesSproc(Udw, "[phone4rmvl].[PHONE4RMVL_CountPrsPhone]")]
        public int GetRowCount(string ssn)
        {
            return LogRun.LDA.ExecuteSingle<int>("[phone4rmvl].[PHONE4RMVL_CountPrsPhone]", Udw,
                SqlParams.Single("Ssn", ssn)).Result;
        }


        [UsesSproc(Udw, "[phone4rmvl].PHONE4RMVL_GetBorrowers")]
        public List<Borrowers> GetValues()
        {
            return LogRun.LDA.ExecuteList<Borrowers>("[phone4rmvl].PHONE4RMVL_GetBorrowers", Udw).CheckResult();
        }

    }
}
