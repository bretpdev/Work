using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ELSYSDDARC
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; private set; }
        public DataAccessHelper.Database DB { get; private set; }
        public ProcessLogRun PLR { get; private set; }
        public DataAccess(LogDataAccess lda, ProcessLogRun plr)
        {
            LDA = lda;
            DB = DataAccessHelper.Database.Uls;
            PLR = plr;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "[fp].GetRecordCount")]
        public int GetRecordCount(string scriptId)
        {
            return LDA.ExecuteSingle<int>("[fp].GetRecordCount", DB, Sp("ScriptId", scriptId)).Result;
        }

        public Borrower GetNextBorrower()
        {
            Borrower bor = new Borrower();
            try
            {
                bor = LDA.ExecuteSingle<Borrower>("fp.GetAndUpdateUnprocessedRecord", DB).Result;
                List<string> lineData = LDA.ExecuteSingle<string>("fp.GetLineDataForAccount", DB, Sp("FileProcessingId", bor.FileProcessingId)).Result.SplitAndRemoveQuotes(",");
                bor.ARC = lineData[1];
                bor.ArcDate = lineData[2];
                bor.Requestor = lineData[3];
            }
            catch (Exception)
            {
                if (bor != null && bor.FileProcessingId > 0)
                {
                    string message = string.Format("Error getting line data for FileProcessingId: {0}, SSN: {1}", bor.FileProcessingId, bor.GroupKey);
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                return null;
            }
            return bor;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}