using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace AESXMTDIAL
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) =>
            LDA = lda;

        [UsesSproc(NobleCalls, "aesxmtdial.GetCallHistory")]
        public List<FileData> GetFileData(DateTime createdAt) =>
            LDA.ExecuteList<FileData>("aesxmtdial.GetCallHistory", NobleCalls,
                Sp("CreatedAt", createdAt)).Result;

        public SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);
    }
}