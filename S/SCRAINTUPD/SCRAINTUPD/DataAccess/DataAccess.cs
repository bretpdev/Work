using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SCRAINTUPD
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public static DataAccessHelper.Database DB { get; set; }

        public DataAccess(LogDataAccess lda, DataAccessHelper.Database db)
        {
            LDA = lda;
            DB = db;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.GetUnprocessedRecords")]
        public List<ScraRecord> GetUnprocessedRecords()
        {
            return LDA.ExecuteList<ScraRecord>("scra.GetUnprocessedRecords", DB).Result; 
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.GetEmergencyPeriods")]
        public List<EmergencyPeriod> GetEmergencyPeriods()
        {
            return LDA.ExecuteList<EmergencyPeriod>("scra.GetEmergencyPeriods", DB).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetTS06Indicators")]
        public void SetProcessedTS06(ScraRecord record, bool success)
        {
            LDA.Execute("scra.SetTS06Indicators", DB, SqlParams.Single("Success", success), SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetTS0NIndicators")]
        public void SetProcessedTS0N(ScraRecord record, bool success)
        {
            LDA.Execute("scra.SetTS0NIndicators", DB, SqlParams.Single("Success", success), SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetErroredAt")]
        public void SetErroredAtDate(ScraRecord record)
        {
            LDA.Execute("scra.SetErroredAt", DB, SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.AddProcessNotificationId")]
        public void AddProcessLoggerMapping(ScraRecord record, int notificationId, int processLogId)
        {
            LDA.Execute("scra.AddProcessNotificationId", DB, SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId), SqlParams.Single("ProcessLogId", processLogId), SqlParams.Single("ProcessNotificationId", notificationId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetArcAddId")]
        public void SetArcAddId(ScraRecord record)
        {
            LDA.Execute("scra.SetArcAddId", DB, SqlParams.Single("ArcAddProcessingId", record.ArcAddProcessingId), SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetRecordSpecialBypass")]
        public void SetRecordSpecialBypass(int scriptProcessingId)
        {
            LDA.Execute("scra.SetRecordSpecialBypass", DB, SqlParams.Single("ScriptProcessingId", scriptProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetTS0NIndicators")]
        public void SetAllSpecial(ScraRecord record)
        {
            LDA.Execute("scra.SetTS0NIndicators", DB, SqlParams.Single("Success", false), SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "scra.SetTSX0TIndicators")]
        public void SetScheduleCompleted(ScraRecord record, bool success)
        {
            LDA.Execute("scra.SetTSX0TIndicators", DB, SqlParams.Single("Success", success), SqlParams.Single("ScriptProcessingId", record.ScriptProcessingId));
        }
    }
}