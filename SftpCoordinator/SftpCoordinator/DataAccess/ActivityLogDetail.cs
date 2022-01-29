using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class ActivityLogDetail
    {
        public DateTime CreatedOn { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string DecryptionOK { get; set; }
        public string CopyOK { get; set; }
        public string EncryptionOK { get; set; }
        public string DeleteOK { get; set; }
        public string CompressionOK { get; set; }
        public string Success { get; set; }
        public string PreDecryptionArchive { get; set; }
        public string PreEncryptionArchive { get; set; }
        public string InvalidFilePath { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? FileTimestamp { get; set; }
        public string InvalidFileResolvedBy { get; set; }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetActivityLogsDetailed")]
        public static List<ActivityLogDetail> GetActivityLogsByRunHistory(int runHistoryId)
        {
            return Program.PLR.LDA.ExecuteList<ActivityLogDetail>("GetActivityLogsDetailed", DataAccessHelper.Database.SftpCoordinator, new SqlParameter("RunHistoryId", runHistoryId)).Result;
        }
    }
}
