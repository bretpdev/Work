using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using SIRPTFED.Models;


namespace SIRPTFED
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public string WindowsUser { get; set; }

        public DataAccess(ProcessLogRun PLR, string user)
        {
            LDA = PLR.LDA;
            WindowsUser = user;
        }

        /// <summary>
        /// Updates an existing metric summary data for a given user.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ServicerInventoryMetrics, "UpdateExistingRecord")]
        public void UpdateMetric(int Id, int Total, int Compliant, int Avg, object suspenseAmount, object suspenseTotal)
        {
            LDA.Execute("UpdateExistingRecord", DataAccessHelper.Database.ServicerInventoryMetrics,
                SqlParams.Single("MetricSummaryId", Id), SqlParams.Single("TotalRecords", Total),
                SqlParams.Single("ComplaintRecords", Compliant), SqlParams.Single("AvgBacklog", Avg),
                SqlParams.Single("SuspenseAmount", suspenseAmount), SqlParams.Single("SuspenseTotal", suspenseTotal));

        }


        /// <summary>
        /// Inserts a new metric summary data for a given user.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ServicerInventoryMetrics, "InsertMetricSummaryData")]
        public void InsertMetric(SqlParameter[]  sqlParams)
        {
            LDA.Execute("InsertMetricSummaryData", DataAccessHelper.Database.ServicerInventoryMetrics, sqlParams);
         
        }


        /// <summary>
        /// Gets the current users info from the ServicerMetric Db
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ServicerInventoryMetrics, "GetCurrentUser")]
        public CurrentUser GetCurrentUser()
        {
            try
            {
                return LDA.ExecuteSingle<CurrentUser>("GetCurrentUser", DataAccessHelper.Database.ServicerInventoryMetrics, SqlParams.Single("WindowsUserId", WindowsUser)).CheckResult();
            }
            catch (InvalidOperationException)
            {
                return null;//Nothing was returned
            }
        }


        /// <summary>
        /// Gets all the servicer categories for a given user
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ServicerInventoryMetrics, "GetServicerCategories")]
        public List<ServicerCategories> PopulateCategories(int Id)
        {
            return LDA.ExecuteList<ServicerCategories>("GetServicerCategories", DataAccessHelper.Database.ServicerInventoryMetrics, SqlParams.Single("AllowedUserId", Id)).CheckResult();
        }

        /// <summary>
        /// Gets metrics for a given user and category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ServicerInventoryMetrics, "GetCategoryMetrics")]
        public List<ServicerMetricsData> PopulateMetrics(int user, int category)
        {
            return LDA.ExecuteList<ServicerMetricsData>("GetCategoryMetrics", DataAccessHelper.Database.ServicerInventoryMetrics, SqlParams.Single("AllowedUserId", user), SqlParams.Single("CategoryId", category)).CheckResult();
        }


        /// <summary>
        /// Gets all metric summary date for a given user.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.ServicerInventoryMetrics, "GetMetricDataForUser")]
        public List<MetricSummaryData> Populate()
        {
            return LDA.ExecuteList<MetricSummaryData>("GetMetricDataForUser", DataAccessHelper.Database.ServicerInventoryMetrics, SqlParams.Single("User", WindowsUser)).Result;
        }
    }
}
