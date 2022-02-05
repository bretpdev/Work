using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public static class DataAccess
    {
        /// <summary>
        /// Gets the password for the batch UserId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public static string GetPassword(string userId) => Program.PLR.LDA.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserId", userId)).Result;
    }
}