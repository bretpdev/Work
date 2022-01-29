using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace QWORKERLGP
{
    class DataAccess
    {
        private LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        [UsesSproc(Ols, "[qworkerlgp].[GetNextQueue]")]
        public QueueData GetNextQueue()
        {
            return LDA.ExecuteSingle<QueueData>("[qworkerlgp].[GetNextQueue]", Ols).CheckResult();
        }

        [UsesSproc(Ols, "[qworkerlgp].[UpdateProcessedAt]")]
        public void UpdateProcessedAt(int queueId, bool hadError)
        {
            LDA.Execute("[qworkerlgp].[UpdateProcessedAt]", Ols, 
                SqlParams.Single("QueueId", queueId),
                SqlParams.Single("HadError", hadError));
        }

        [UsesSproc(Ols, "[qworkerlgp].[UpdateWasFound]")]
        public void UpdateWasFound(int queueId, bool wasFound)
        {
            LDA.Execute("[qworkerlgp].[UpdateWasFound]", Ols,
                SqlParams.Single("QueueId", queueId),
                SqlParams.Single("WasFound", wasFound));
        }

        [UsesSproc(Ols, "[qworkerlgp].[CleanUpRecords]")]
        public void CleanUpRecords()
        {
            LDA.Execute("[qworkerlgp].[CleanUpRecords]", Ols);
        }
    }
}
