using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACTDUTRPT
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.GetUnprocessedBenefitsRecords")]
        public List<ActiveDutyRecord> GetUnprocessedRecords()
        {
            return LDA.ExecuteList<ActiveDutyRecord>("scra.GetUnprocessedBenefitsRecords", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetTXCXIndicators")]
        public void SetProcessedTXCX(ActiveDutyRecord record, bool success)
        {
            LDA.Execute("scra.SetTXCXIndicators", DataAccessHelper.Database.Uls, SqlParams.Single("Success", success), SqlParams.Single("ActiveDutyReportingId", record.ActiveDutyReportingId));
        }
    }
}