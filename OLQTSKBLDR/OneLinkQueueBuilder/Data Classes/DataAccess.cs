using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OLQBuilder
{
    class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        private ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olqtskbldr.InsertToQueues")]
        public bool InsertToQueues(List<QueueTableRecord> records, string filename)
        {
            return LDA.Execute("olqtskbldr.InsertToQueues", DataAccessHelper.Database.Ols, SqlParams.Single("QueueTable", records.ToDataTable()), SqlParams.Single("SourceFilename", filename));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olqtskbldr.MarkQueueRecordProcessed")]
        public bool MarkQueueRecordProcessed(int queueId)
        {
            return LDA.Execute("olqtskbldr.MarkQueueRecordProcessed", DataAccessHelper.Database.Ols, SqlParams.Single("QueueId", queueId));

        }

        [UsesSproc(DataAccessHelper.Database.Ols, "olqtskbldr.UpdateProcessingAttempts")]
        public bool UpdatingProcessingAttempts(int queueId)
        {
            return LDA.Execute("olqtskbldr.UpdateProcessingAttempts", DataAccessHelper.Database.Ols, SqlParams.Single("QueueId", queueId));
        }

            [UsesSproc(DataAccessHelper.Database.Ols, "olqtskbldr.GetUnprocessedQueues")]
        public List<QueueRecord> GetUnprocessedQueues()
        {
            var result = LDA.ExecuteList<QueueRecord>("olqtskbldr.GetUnprocessedQueues", DataAccessHelper.Database.Ols);
            if(!result.DatabaseCallSuccessful)
            {
                LogRun.AddNotification("Call to GetUnprocessedQueues failed.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            if(result.Result.Count == 0)
            {
                LogRun.AddNotification("No queues to process.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            return result.Result;
        }


        [UsesSproc(DataAccessHelper.Database.Bsys, "olqtskbldr.GetSasList")]
        public List<SasInstructions> GetSasList()
        {
            var result = LDA.ExecuteList<QueueBuilderList>("olqtskbldr.GetSasList", DataAccessHelper.Database.Bsys);
            if (!result.DatabaseCallSuccessful)
            {
                LogRun.AddNotification("Call to GetSasList failed.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return null;
            }
            List<SasInstructions> instructions = new List<SasInstructions>();
            foreach(QueueBuilderList qb in result.Result)
            {
                instructions.Add(new SasInstructions(qb));
            }
            return instructions;
        }
    }
}
