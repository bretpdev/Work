using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace BatchLettersUI
{
    public static class DataAccess
    {
        [UsesSproc(DataAccessHelper.Database.Cls, "GetAllBatchLetters")]
        public static List<DatabaseData> SetDataSource()
        {
            return DataAccessHelper.ExecuteList<DatabaseData>("GetAllBatchLetters", DataAccessHelper.Database.Cls);
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "UpdateBatchLettersFed")]
        public static void UpdateBatchLetters(SqlParameter[] sqlParms)
        {
            DataAccessHelper.Execute("UpdateBatchLettersFed", DataAccessHelper.Database.Cls, sqlParms.ToArray());
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "AddRecordBatchLetter")]
        public static void AddBatchLetter(SqlParameter [] sqlParms)
        {
            DataAccessHelper.Execute("AddRecordBatchLetter", DataAccessHelper.Database.Cls, sqlParms);
        }
    }
}
