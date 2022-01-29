using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace FORBPROFED
{
    class Program
    {
        public static readonly string ScriptId = "FORBPROFED";

        public static readonly int SessionCount = 1;

        static int Main(string[] args)
        {
            if(!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return 1;
            }

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return 1;
            }
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            ProcessLogRun logRun = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            new ForbearanceProcessor(logRun).Process();
            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
