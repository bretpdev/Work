using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BBONTMDISQ
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "bbontmdisq.GetDisqualificationRecords")]
        public List<DisqualificationRecord> GetDisqualificationRecords()
        {
            return LogRun.LDA.ExecuteList<DisqualificationRecord>("bbontmdisq.GetDisqualificationRecords", DataAccessHelper.Database.Udw).CheckResult();   
        }
    }
}
