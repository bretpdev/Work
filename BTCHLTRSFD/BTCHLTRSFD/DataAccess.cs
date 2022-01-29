using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;

namespace BTCHLTRSFD
{
    public static class DataAccess
    {
        [UsesSproc(DataAccessHelper.Database.Cls,"GetActiveBatchLetters")]
        public static List<DatabaseData> GetLettersFromDb()
        {
            return DataAccessHelper.ExecuteList<DatabaseData>("GetActiveBatchLetters", DataAccessHelper.Database.Cls);
        }

        public static DatabaseData GetRecoveryFileData(int id)
        {
            return DataAccessHelper.ExecuteSingle<DatabaseData>("GetBatchLetterRecordForRecovery", DataAccessHelper.Database.Cls, new SqlParameter("Id", id));
        }
    }
}
