using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;
using static System.Console;

namespace RCDIALER
{
    public class Program
    {
        public const string ScriptId = "RCDIALER";

        public static int Main(string[] args)
        {
            WriteLine($"{ScriptId} :: Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            string overrideSproc = "";
            if (args.Length > 1)
                overrideSproc = args[1];

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            new RepayDialerFiles(logRun).Process(overrideSproc);

            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}