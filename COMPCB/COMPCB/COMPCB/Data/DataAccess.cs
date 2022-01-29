using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace COMPCB
{
    public class DataAccess
    {
        private ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.AddNewTasks")]
        public bool LoadNewWork()
        {
            return LogRun.LDA.Execute("compcb.AddNewTasks", DataAccessHelper.Database.Uls);
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.GetUnprocessedTasks")]
        public List<TaskInfo> GetUnprocessedWork()
        {
            return LogRun.LDA.ExecuteList<TaskInfo>("compcb.GetUnprocessedTasks", DataAccessHelper.Database.Uls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.SetProcessedAt")]
        public bool SetProcessedAt(int processingQueueId)
        {
            return LogRun.LDA.Execute("compcb.SetProcessedAt", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", processingQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.SetDeletedAt")]
        public bool SetDeletedAt(int processingQueueId)
        {
            return LogRun.LDA.Execute("compcb.SetDeletedAt", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", processingQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.SetArcAddId")]
        public bool SetArcAddProcessingId(TaskInfo task)
        {
            return LogRun.LDA.Execute("compcb.SetArcAddId", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", task.ProcessingQueueId), SP("ArcAddProcessingId", task.ArcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "dbo.spGENR_GetStateCodes")]
        public List<string> GetDomesticStateCodes()
        {
            return LogRun.LDA.ExecuteList<string>("dbo.spGENR_GetStateCodes", DataAccessHelper.Database.Bsys, SP("IncludeTerritories", false)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.SetForeignAddressFlag")]
        public bool UpdateForeignAddressIndicator(int processingQueueId)
        {
            return LogRun.LDA.Execute("compcb.SetForeignAddressFlag", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", processingQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "compcb.IncrementProcessingAttempts")]
        public bool UpdateProcessingAttempts(int processingQueueId)
        {
            return LogRun.LDA.Execute("compcb.IncrementProcessingAttempts", DataAccessHelper.Database.Uls, SP("ProcessingQueueId", processingQueueId));
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "dbo.BusinessUnitsByScript")]
        public BusinessUnitInfo GetBusinessUnitForScript(string scriptId)
        {
            return LogRun.LDA.ExecuteSingle<BusinessUnitInfo>("dbo.BusinessUnitsByScript", DataAccessHelper.Database.Bsys, SP("ScriptId", scriptId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "dbo.GetManagerOfBusinessUnit")]
        public string GetManagerId(string businessUnitName)
        {
            return LogRun.LDA.ExecuteSingle<string>("dbo.GetManagerOfBusinessUnit", DataAccessHelper.Database.Bsys, SP("BusinessUnit", businessUnitName)).Result;
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
