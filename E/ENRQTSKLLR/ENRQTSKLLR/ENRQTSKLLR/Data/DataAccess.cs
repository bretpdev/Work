using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ENRQTSKLLR
{
    public class DataAccess
    {
        ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "enrqtskllr.InsertProcessingQueueRecords")]
        public int? InsertProcessingQueueRecords()
        {
            return LogRun.LDA.ExecuteSingle<int?>("enrqtskllr.InsertProcessingQueueRecords", DataAccessHelper.Database.Ols).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "enrqtskllr.SetRecordProcessed")]
        public bool SetRecordProcessed(int processingQueueId)
        {
            return LogRun.LDA.Execute("enrqtskllr.SetRecordProcessed", DataAccessHelper.Database.Ols,
                SP("ProcessingQueueId", processingQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "enrqtskllr.SetArcAddedAt")]
        public bool SetArcAddedAt(int processingQueueId)
        {
            return LogRun.LDA.Execute("enrqtskllr.SetArcAddedAt", DataAccessHelper.Database.Ols,
               SP("ProcessingQueueId", processingQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Ols, "enrqtskllr.GetUnprocessedRecords")]
        public List<EnrollmentTask> GetUnprocessedRecords()
        {
            return LogRun.LDA.ExecuteList<EnrollmentTask>("enrqtskllr.GetUnprocessedRecords", DataAccessHelper.Database.Ols).Result;
        }

        /// <summary>
        /// SQL parameterization wrapper: 
        /// parameterizes a string as the field name and
        /// an object as the value to be used for DB calls.
        /// </summary>
        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}
