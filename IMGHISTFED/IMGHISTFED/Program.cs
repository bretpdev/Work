using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace IMGHISTFED
{
    public class Program
    {
        public static string ScriptId = "IMGHISTFED";
        public static int Main(string[] args)
        {
            if(!DataAccessHelper.StandardArgsCheck(args,ScriptId))
            {
                return 1;
            }
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchCornerStone", true);

            ActivityHistoryReport report = new ActivityHistoryReport(ri);
            report.Main();
            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
