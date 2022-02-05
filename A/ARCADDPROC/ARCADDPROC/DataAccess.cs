using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ARCADDPROCESSING
{
    public class DataAccess
    {
        private ProcessLogRun LogRun { get; set; }
        private DataAccessHelper.Database DB { get; set; } = DataAccessHelper.Database.Uls;
        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_GetNextArc")]
        public ArcRecord GetNextArc()
        {
            return LogRun.LDA.ExecuteSingle<ArcRecord>("ArcAdd_GetNextArc", DB).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_GetLoanProgramRecords")]
        public List<string> GetLoanPgms(long arcAddProcessingId)
        {
            return LogRun.LDA.ExecuteList<string>("ArcAdd_GetLoanProgramRecords", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_GetLoanSequenceRecords")]
        public List<int> GetLoanSeqs(long arcAddProcessingId)
        {
            return LogRun.LDA.ExecuteList<int>("ArcAdd_GetLoanSequenceRecords", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "GetArcAddCount")]
        public int GetArcCount()
        {
            return LogRun.LDA.ExecuteSingle<int>("GetArcAddCount", DB).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_AddProcessNotificationId")]
        public void AddProcessLogsMapping(long arcAddProcessingId, int processLogId, int? processNotificationId)
        {
            LogRun.LDA.Execute("ArcAdd_AddProcessNotificationId", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("ProcessLogId", processLogId),
                SqlParams.Single("ProcessNotificationId", processNotificationId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_GetNextArc")]
        public void UpdateLN_ATY_SEQ(long arcAddProcessingId, int ln_aty_seq)
        {
            LogRun.LDA.Execute("ArcAdd_UpdateLN_ATY_SEQ", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("LN_ATY_SEQ", ln_aty_seq));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_UpdateProcessOn")]
        public void RequeueARC(long arcAddProcessingId, int hours)
        {
            LogRun.LDA.Execute("ArcAdd_UpdateProcessOn", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("Hours", hours));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_CleanUpArcs")]
        public void CleanUpArcs()
        {
            LogRun.LDA.Execute("ArcAdd_CleanUpArcs", DB);
        }
    }
}
