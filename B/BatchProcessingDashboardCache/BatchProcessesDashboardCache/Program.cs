using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BatchProcessesDashboardCache
{
    class Program
    {
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            var pld = ProcessLogger.RegisterApplication("BPDCache", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), false);
            var da = new DataAccess(new LogDataAccess(DataAccessHelper.Mode.Dev, pld.ProcessLogId, true, true));
            var itemsToProcess = da.GetDashboardItems();
            Parallel.ForEach(itemsToProcess, dash =>
            {
                Console.WriteLine("Beginning dashboard " + dash.ItemName);
                DateTime uheaaStart = DateTime.Now;
                int? uheaaCount = GetErrorCount(pld, da, dash.UheaaDatabase, dash.UheaaSprocName);
                int? uheaaElapsed = null;
                if (uheaaCount.HasValue)
                    uheaaElapsed = (int)(DateTime.Now - uheaaStart).TotalMilliseconds;

                DateTime cornerstoneStart = DateTime.Now;
                int? csCount = GetErrorCount(pld, da, dash.CornerstoneDatabase, dash.CornerstoneSprocName);
                int? cornerstoneElapsed = null;
                if (csCount.HasValue)
                    cornerstoneElapsed = (int)(DateTime.Now - cornerstoneStart).TotalMilliseconds;

                da.AddCacheHistory(dash.DashboardItemId, uheaaCount, csCount, uheaaElapsed, cornerstoneElapsed);
                Console.WriteLine("Finished dashboard " + dash.ItemName);
            });
            ProcessLogger.LogEnd(pld.ProcessLogId);

            DataAccessHelper.CloseAllManagedConnections();

            return 0;

        }

        private static int? GetErrorCount(ProcessLogData pld, DataAccess da, string databaseName, string sprocName)
        {
            if (databaseName == null || sprocName == null)
                return null;
            int? result = null;
            var repRes = Repeater.TryRepeatedly(() =>
            {
                var db = (DataAccessHelper.Database)Enum.Parse(typeof(DataAccessHelper.Database), databaseName, true);
                string definition = da.GetSprocDefinition(sprocName);
                using (var comm = DataAccessHelper.GetCommand(definition, db))
                {
                    comm.CommandType = System.Data.CommandType.Text;
                    comm.CommandTimeout = 0;
                    result = (int)comm.ExecuteScalar();
                }
            });
            if (!repRes.Successful)
            {
                foreach (var ex in repRes.CaughtExceptions)
                {
                    var message = string.Format("Unable to get error count for {0}.{1}.", databaseName, sprocName);
                    ProcessLogger.AddNotification(pld.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                }
            }
            else if (repRes.CaughtExceptions.Any())
            {
                var message = string.Format("Successfully processed {0}.{1}, but the process failed {2} time(s) before succeeding.", databaseName, sprocName, repRes.CaughtExceptions.Count);
                ProcessLogger.AddNotification(pld.ProcessLogId, message, NotificationType.EndOfJob, NotificationSeverityType.Informational, Assembly.GetExecutingAssembly());
            }
            return result;
        }
    }
}
