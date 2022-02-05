using System;

using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class InvalidFile
    {
        [PrimaryKey]
        public int InvalidFileId { get; set; }
        public string FilePath { get; set; }
        public DateTime FileTimestamp { get; set; }
        public string ErrorMessage { get; set; }
        [DbReadOnly]
        public string ResolvedBy { get; set; }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "InsertInvalidFile")]
        public static InvalidFile Insert(InvalidFile file)
        {
            var parameters = SqlParams.Insert(file);
            file.InvalidFileId = Program.PLR.LDA.ExecuteSingle<int>("InsertInvalidFile", DataAccessHelper.Database.SftpCoordinator, parameters).Result;
            return file;
        }
    }
}
