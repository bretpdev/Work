using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ELSYSDDARC
{
    public class Borrower
    {
        public int FileProcessingId { get; set; }
        public string GroupKey { get; set; }
        public string SourceFile { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string ARC { get; set; }
        public string ArcDate { get; set; }
        public string Requestor { get; set; }

        public Borrower() { }

        public Borrower(SqlConnection conn, ProcessLogRun logRun)
        {
            SetData(conn, logRun);
        }

        /// <summary>
        /// Gets all the data for the next available borrower
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "fp.GetAndUpdateUnprocessedRecord")]
        [UsesSproc(DataAccessHelper.Database.Uls, "fp.GetLineDataForAccount")]
        private void SetData(SqlConnection conn, ProcessLogRun logRun)
        {
            Borrower bor = new Borrower();
            try
            {
                bor = DataAccessHelper.ExecuteSingle<Borrower>("fp.GetAndUpdateUnprocessedRecord", conn);
                FileProcessingId = bor.FileProcessingId;
                GroupKey = bor.GroupKey;
                SourceFile = bor.SourceFile;
                ProcessedAt = bor.ProcessedAt;
                List<string> lineData = DataAccessHelper.ExecuteSingle<string>("fp.GetLineDataForAccount", conn, SqlParams.Single("FileProcessingId", bor.FileProcessingId)).SplitAndRemoveQuotes(",");
                ARC = lineData[1];
                ArcDate = lineData[2];
                Requestor = lineData[3];
            }
            catch (InvalidOperationException)
            {
                if (bor.FileProcessingId > 0)
                {
                    string message = string.Format("Error getting line data for FileProcessingId: {0}, SSN: {1}", bor.FileProcessingId, bor.GroupKey);
                    ProcessLogger.AddNotification(logRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
                bor = null;
            }
        }
    }
}