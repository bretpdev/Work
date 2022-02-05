using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace AESXMTDIAL
{
    public class Program
    {
        public static readonly string ScriptId = "AESXMTDIAL";
        public static int ERROR = 1;
        public static int SUCCESS = 0;

        static int Main(string[] args)
        {
            WriteLine($"{ScriptId} - Version :: {Assembly.GetExecutingAssembly().GetName().Version}");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return ERROR;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return ERROR;

            DateTime createdAt = DateTime.Now.Date;
            if (args.Length > 1 && args[1].ToDateNullable().HasValue && args[1].ToDate() < DateTime.Now.Date)
                createdAt = args[1].ToDate();

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            new CreateFiles(logRun).Process(createdAt);

            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return SUCCESS;
        }
    }
}