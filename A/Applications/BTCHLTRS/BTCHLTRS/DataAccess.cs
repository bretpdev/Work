using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace BTCHLTRS
{
    public static class DataAccess
    {
        /// <summary>
        /// Gets all active records from BatchLetters in ULS
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls,"GetActiveBatchLetters")]
        public static List<DatabaseData> GetLettersFromDb()
        {
            return DataAccessHelper.ExecuteList<DatabaseData>("GetActiveBatchLetters", DataAccessHelper.Database.Uls);
        }

        /// <summary>
        /// Gets the information about a letter based on the batchLetterId
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "GetBatchLetterRecordForRecovery")]
        public static DatabaseData GetRecoveryFileData(int id)
        {
            return DataAccessHelper.ExecuteSingle<DatabaseData>("GetBatchLetterRecordForRecovery", DataAccessHelper.Database.Uls, new SqlParameter("Id", id));
        }
    }
}
