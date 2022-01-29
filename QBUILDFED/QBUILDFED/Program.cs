using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace QBUILDFED
{
    public class Program
    {
        public static string ScriptId { get; set; } = "QBUILDFED";

        public static int Main(string[] args)
        {
            Console.WriteLine("Starting the Q Task Builder Fed script");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;
            
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            int processed = new QueueTaskBuilder(logRun, ScriptId).Process();
            logRun.LogEnd();

            return processed;
        }
    }
}