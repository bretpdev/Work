using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace AACDELBATC
{
    class DataAccess
    {
        public static List<MajorBatchInfo> GetRecoveryValues(string userid)
        {
            return DataAccessHelper.ExecuteList<MajorBatchInfo>("spAacDeleteBatchesGetBatches", DataAccessHelper.Database.Uls, new SqlParameter("UserId", userid));
        }

        public static void InsertbatchesToDelete(List<MajorBatchInfo> batchData, string userId)
        {
            foreach (MajorBatchInfo item in batchData)
            {
                DataAccessHelper.Execute("spAacDeleteBatchesInsertBatches", DataAccessHelper.Database.Uls, new SqlParameter("MajorBatch", item.MajorBatchToDelete), new SqlParameter("UserId",userId));
            }
        }

        public static void DeleteProcessedBatch(string majorbatch)
        {
            DataAccessHelper.Execute("spAacDeleteBatch", DataAccessHelper.Database.Uls, new SqlParameter("BatchNumber", majorbatch));
        }

        public static void DeleteAllBatchesForUserId(string userId)
        {
            DataAccessHelper.Execute("spDeleteAllBatchesForUserId", DataAccessHelper.Database.Uls, new SqlParameter("UserId", userId));
        }
    }
}
