using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace RHBRWINPC
{
    public class Program
    {
        public static readonly string ScriptId = "RHBRWINPC";
        private static int ERROR { get; set; } = 1;

        public static int Main(string[] args)
        {
            WriteLine($"Rehabilitated Borrowers in Preclaim  ::  Version - {Assembly.GetEntryAssembly().GetName().Version}");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return ERROR;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return ERROR;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            int returnCount = new RehabBorrowerInPreClaim(logRun).Main();

            logRun.LogEnd();
            return returnCount;
        }
    }
}