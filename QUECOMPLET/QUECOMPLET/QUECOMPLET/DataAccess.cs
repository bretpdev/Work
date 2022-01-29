using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace QUECOMPLET
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        
        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        [UsesSproc(Uls, "[quecomplet].[GetNextQueue]")]
        public QueueData GetNextQueue() => LDA.ExecuteSingle<QueueData>("[quecomplet].[GetNextQueue]", Uls).CheckResult();

        [UsesSproc(Uls, "[quecomplet].[UpdateProcessedAt]")]
        public void UpdateProcessed(int queueId, bool hadError) => LDA.Execute("[quecomplet].[UpdateProcessedAt]", Uls, SqlParams.Single("QueueId", queueId),
                SqlParams.Single("HadError", hadError));

        [UsesSproc(Uls, "[quecomplet].[UpdateWasFound]")]
        public void UpdateWasFound(int bit, int queueid) => LDA.Execute("[quecomplet].[UpdateWasFound]", Uls, SqlParams.Single("WasFound", bit),
                SqlParams.Single("QueueId", queueid));

        [UsesSproc(Uls, "[quecomplet].[CleanUpRecords]")]
        public void CleanUpRecords() => LDA.Execute("[quecomplet].[CleanUpRecords]", Uls);

        [UsesSproc(Uls, "[quecomplet].[GetRecordsToProcessCount]")]
        public int GetProcessingCount() => LDA.ExecuteSingle<int>("[quecomplet].[GetRecordsToProcessCount]", Uls).CheckResult();
    }
}