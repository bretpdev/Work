using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace HRBRIDGE
{
    class Program
    {
        public readonly static string ScriptId = "HRBRIDGE";
        public static ProcessLogRun LogRun { get; set; }

        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
            {
                return 1;
            }
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);

            BambooHRProcessor bambooProcessor = new BambooHRProcessor(LogRun);
            bambooProcessor.Process();

            //This should come last as data is written to here from the other websites
            BridgeProcessor bridgeProcessor = new BridgeProcessor(LogRun, bambooProcessor.AliasDictionary);
            bridgeProcessor.Process();

            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
