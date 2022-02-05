using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ONELINKAAP
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        private DataAccessHelper.Database DB { get; set; }
        public DataAccess(int processLogId, DataAccessHelper.Region region, DataAccessHelper.Database db)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, processLogId, false, true, region);
            DB = db;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[onelinkaap].GetNextArc")]
        public ArcRecord GetNextArc()
        {
            return LDA.ExecuteSingle<ArcRecord>("[onelinkaap].GetNextArc", DB).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Udw, "spGetSSNFromAcctNumber")]
        public string GetSSNFromAccoutNumber(string accountNumber)//This is only needed for OneLINK
        {
            return LDA.ExecuteSingle<string>("spGetSSNFromAcctNumber", DataAccessHelper.Database.Udw, SqlParams.Single("AccountNumber", accountNumber)).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[onelinkaap].GetArcAddCount")]
        public int GetArcCount()
        {
            return LDA.ExecuteSingle<int>("[onelinkaap].GetArcAddCount", DB).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[onelinkaap].AddProcessNotificationId")]
        public void AddProcessLogsMapping(long arcAddProcessingId, int processLogId, int? processNotificationId)
        {
            LDA.Execute("[onelinkaap].AddProcessNotificationId", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("ProcessLogId", processLogId),
                SqlParams.Single("ProcessNotificationId", processNotificationId));
        }


        [UsesSproc(DataAccessHelper.Database.Uls, "[onelinkaap].UpdateLN_ATY_SEQ")]
        public void UpdateLN_ATY_SEQ(long arcAddProcessingId, int ln_aty_seq)
        {
            LDA.Execute("[onelinkaap].UpdateLN_ATY_SEQ", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("LN_ATY_SEQ", ln_aty_seq));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[onelinkaap].UpdateProcessOn")]
        public void RequeueARC(long arcAddProcessingId, int hours)
        {
            LDA.Execute("[onelinkaap].UpdateProcessOn", DB, SqlParams.Single("ArcAddProcessingId", arcAddProcessingId), SqlParams.Single("Hours", hours));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[onelinkaap].CleanUpArcs")]
        public void CleanUpArcs()
        {
            LDA.Execute("[onelinkaap].CleanUpArcs", DB);
        }
    }
}
