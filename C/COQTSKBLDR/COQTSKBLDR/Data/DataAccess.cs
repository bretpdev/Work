using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace COQTSKBLDR
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public DataAccess(ProcessLogRun PLR)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, PLR.ProcessLogId, true, true);
        }
      
        [UsesSproc(DataAccessHelper.Database.Uls, "[fp].QBLDR_GetUnprocessedRecords")]
        public List<FileProcessingRecord> GetUnprocessedRecords(string scriptId)
        {
            return LDA.ExecuteList<FileProcessingRecord>("[fp].QBLDR_GetUnprocessedRecords", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptId", scriptId)).Result ?? null;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[fp].SetProcessedAt")]
        public void UpdateProcessed(int fileProcessingId)
        {
            LDA.Execute("[fp].SetProcessedAt",DataAccessHelper.Database.Uls, SqlParams.Single("FileProcessingId",fileProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[fp].GetCriticalIfMissingFiles")]
        public List<CriticalIfMissing> GetRequiredFiles(string scriptId)
        {
            return LDA.ExecuteList<CriticalIfMissing>("[fp].GetCriticalIfMissingFiles", DataAccessHelper.Database.Uls, SqlParams.Single("ScriptId", scriptId)).Result ?? null;
        }
    }
}
