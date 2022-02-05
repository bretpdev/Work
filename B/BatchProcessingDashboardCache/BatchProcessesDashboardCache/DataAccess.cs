using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace BatchProcessesDashboardCache
{
    class DataAccess
    {
        private LogDataAccess lda;
        public DataAccess(LogDataAccess lda)
        {
            this.lda = lda;
        }

        [UsesSproc(DB.BatchProcessing, "dashcache.GetDashboardItems")]
        public List<DashboardItem> GetDashboardItems()
        {
            return lda.ExecuteList<DashboardItem>("dashcache.GetDashboardItems", DB.BatchProcessing).Result;
        }

        public string GetSprocDefinition(string sprocName)
        {
            if (!sprocName.StartsWith("dashcache."))
                sprocName = "dashcache." + sprocName;
            var result = lda.ExecuteList<string>("sp_helptext", DB.BatchProcessing, Sp("objname", sprocName));
            string definition = "";
            bool foundAs = false;
            foreach (var line in result.Result)
            {
                if (line.ToLower().Trim().StartsWith("return "))
                    continue;
                if (foundAs)
                    definition += line;
                else if (line.ToLower().Trim() == "as")
                        foundAs = true;
            }
            return definition;
        }

        [UsesSproc(DB.BatchProcessing, "[dashcache].[AddCacheResult]")]
        public void AddCacheHistory(int dashboardItemId, int? uheaaCount, int? cornerstoneCount, int? uheaaElapsedTimeInMilliseconds, int? cornerstoneElapsedTimeInMilliseconds)
        {
            lda.Execute("[dashcache].[AddCacheResult]", DB.BatchProcessing, 
                Sp("DashboardItemId", dashboardItemId), Sp("UheaaCount", uheaaCount), Sp("CornerstoneCount", cornerstoneCount), 
                Sp("UheaaElapsedTimeInMilliseconds", uheaaElapsedTimeInMilliseconds), Sp("CornerstoneElapsedTimeInMilliseconds", cornerstoneElapsedTimeInMilliseconds));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
