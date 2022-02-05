using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IDRRPTFED
{
    public class DataAccess
    {
        private static DataAccessHelper.Database DB { get; set; }
        private static LogDataAccess LDA { get; set; }
        public DataAccess(bool legacy, LogDataAccess lda)
        {
            LDA = lda;
            DB = legacy ? DataAccessHelper.Database.Income_Driven_RepaymentLegacy : DataAccessHelper.Database.Income_Driven_Repayment;
        }
        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "spGetAbRecordData")]
        public List<AbRecordData> GetAbRecords(DateTime? start, DateTime? end, int? appId = null)
        {
            return LDA.ExecuteList<AbRecordData>("spGetAbRecordData", DB, Sp("StartDate", start), Sp("EndDate", end), Sp("AppId", appId ?? -1)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "spGetBdRecordData")]
        public List<BdRecordData> GetBdRecords(DateTime? start, DateTime? end, int? appId = null)
        {
            return LDA.ExecuteList<BdRecordData>("spGetBdRecordData", DB, Sp("StartDate", start), Sp("EndDate", end), Sp("AppId", appId ?? -1)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "spGetBeRecordData")]
        public List<BeRecordData> GetBeRecords(DateTime? start, DateTime? end, int? appId = null)
        {
            return LDA.ExecuteList<BeRecordData>("spGetBeRecordData", DB, Sp("StartDate", start), Sp("EndDate", end), Sp("AppId", appId ?? -1)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "spGetBfRecordData")]
        public List<BfRecordData> GetBfRecords(DateTime? start, DateTime? end, int? appId = null)
        {
            return LDA.ExecuteList<BfRecordData>("spGetBfRecordData", DB, Sp("StartDate", start), Sp("EndDate", end), Sp("AppId", appId ?? -1)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "GetRequestedPlans")]
        public static RequestedPlans GetRequestedPlans(int appId)
        {
            return DataAccessHelper.ExecuteSingle<RequestedPlans>("GetRequestedPlans", DB, new SqlParameter("AppId", appId));
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "spGetLastRunDate")]
        public static List<RunHistory> GetLastRunDate()
        {
            return DataAccessHelper.ExecuteList<RunHistory>("spGetLastRunDate", DB);
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "spSetRunDate")]
        public static void SetRunDate(DateTime runDate, DateTime startDate, DateTime endDate, int? previousRunId)
        {
            DataAccessHelper.Execute("spSetRunDate", DB,
                new SqlParameter("RunDate", runDate),
                new SqlParameter("StartDate", startDate),
                new SqlParameter("EndDate", endDate),
                new SqlParameter("PreviousRunId", previousRunId),
                new SqlParameter("RunBy", Environment.UserName));
        }

        private static SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }

        [UsesSproc(DataAccessHelper.Database.Income_Driven_Repayment, "GetBorrowerSelectedLowestPlan")]
        public static bool GetLowestPlanSelection(int appId)
        {
            return DataAccessHelper.ExecuteSingle<bool>("GetBorrowerSelectedLowestPlan", DB, new SqlParameter("AppId", appId));
        }
    }
}
