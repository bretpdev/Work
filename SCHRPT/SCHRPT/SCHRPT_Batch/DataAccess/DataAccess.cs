using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SCHRPT_Batch
{
    /// <summary>
    /// Accessor methods for CLS.schrpt SPROCS
    /// </summary>
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public static DataAccessHelper.Database DB { get; set; }

        public DataAccess(LogDataAccess lda, DataAccessHelper.Database db)
        {
            LDA = lda;
            DB = db;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetRecipients")]
        public List<SchrptRecord> GetRecipients()
        {
            return LDA.ExecuteList<SchrptRecord>("schrpt.GetRecipients", DB).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetSchools")]
        public List<SchrptRecord> GetSchools()
        {
            return LDA.ExecuteList<SchrptRecord>("schrpt.GetSchools", DB).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetSchoolRecipients")]
        public List<SchrptRecord> GetSchoolRecipients()
        {
            return LDA.ExecuteList<SchrptRecord>("schrpt.GetSchoolRecipients", DB).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetReportTypes")]
        public List<SchrptRecord> GetReportTypes()
        {
            return LDA.ExecuteList<SchrptRecord>("schrpt.GetReportTypes", DB).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetSchool")]
        public List<SchrptRecord> GetSchool(int schoolId)
        {
            return LDA.ExecuteList<SchrptRecord>("schrpt.GetSchool", DB, SqlParams.Single("SchoolId", schoolId )).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetSchoolEmailHistory")]
        public List<SchrptRecord> GetSchoolEmailHistory()
        {
            return LDA.ExecuteList<SchrptRecord>("schrpt.GetSchoolEmailHistory", DB).Result;
        }

        /// <summary>
        /// Executes an arbitrary sproc from the ReportTypes table
        /// </summary>
        /// <param name="schoolCode">The combined school code and branch code or the desired school to execute on</param>
        /// <param name="storedProcedure">The stored procedure to execute</param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.GetBorrowersForSchool")]
        public DataTable GetStoredProcedure(string schoolCode , string storedProcedure)
        {
            return LDA.ExecuteDataTable("schrpt." + storedProcedure, DB, false, SqlParams.Single("School", schoolCode)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "schrpt.AddSchoolEmailHistory")]
        public void AddSchoolEmailHistory(int schoolRecipientId, DateTime emailSentAt, string file)
        {
            LDA.Execute("schrpt.AddSchoolEmailHistory", DB, SqlParams.Single("SchoolRecipientId", schoolRecipientId), SqlParams.Single("EmailSentAt", emailSentAt), SqlParams.Single("FileSent", file), SqlParams.Single("WindowsUserName", Environment.UserName));
        }




    }
}
