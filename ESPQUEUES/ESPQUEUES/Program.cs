using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ESPQUEUES
{

    public class Program
    {
        public static string ScriptId { get; set; } = "ESPQUEUES";
        public const int SUCCESS = 0;
        public const int ERROR = 1;
        public static bool ShowPrompts;
        public static bool SkipTaskClose;
        public static string AccountNumber;

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ShowPrompts = (args.Length > 1 && args.Any(p => p.ToLower() == "showprompts"));
            SkipTaskClose = (args.Length > 1 && args.Any(p => p.ToLower() == "skiptaskclose"));
            AccountNumber = (args.Length > 1 && args.Any(p => p.ToLower().StartsWith("account:"))) ? args.Where(p => p.StartsWith("account:")).FirstOrDefault().ToString().Replace("account:", "") : "";

            if (!HasValidArgsAndAccess(args))
                return ERROR;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            ri.LogRun = logRun;
            BatchProcessingHelper loginHelper = BatchProcessingLoginHelper.Login(ri.LogRun, ri, ScriptId, "BatchUheaa");
            Console.WriteLine($"Logged into session with the ID: {loginHelper.UserName}");

            int runResult = new EspQueues(ri).Run();
            logRun.LogEnd();
            ri.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();

            return runResult;
        }

        private static bool HasValidArgsAndAccess(string[] args)
        {
            bool hasValidArgsAndAccess = true;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, ShowPrompts))
            {
                Console.WriteLine("Script failed to initiate run due to incorrect arguments being passed into it. Please pass \"dev\" or \"live\".");
                hasValidArgsAndAccess = false;
            }
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ShowPrompts))
            {
                Console.WriteLine("Script failed to initiate run due to one or more stored procedures not having access.");
                hasValidArgsAndAccess = false;
            }

            return hasValidArgsAndAccess;
        }
    }
}
