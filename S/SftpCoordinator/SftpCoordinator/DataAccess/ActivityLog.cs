using System;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class ActivityLog
    {
        [PrimaryKey]
        public int ActivityLogId { get; set; }
        public int ProjectFileId { get; set; }
        public int RunHistoryId { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public bool? DecryptionSuccessful { get; set; }
        public bool? EncryptionSuccessful { get; set; }
        public bool CopySuccessful { get; set; }
        public bool? FixLineEndingsSuccessful { get; set; }
        public bool? DeleteSuccessful { get; set; }
        public bool? CompressionSuccessful { get; set; }
        public int? InvalidFileId { get; set; }
        public bool Successful { get; set; }
        public string PreDecryptionArchiveLocation { get; set; }
        public string PreEncryptionArchiveLocation { get; set; }
        [DbReadOnly]
        public DateTime CreatedOn { get; set; }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "InsertActivityLog")]
        public static void LogActivity(ActivityLog al)
        {
            Program.PLR.LDA.Execute("InsertActivityLog", DataAccessHelper.Database.SftpCoordinator, SqlParams.Insert(al));
        }
    }
}
