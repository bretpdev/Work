using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace DIACTCMTS
{
    class DialerActivityComments
    {
        public static readonly string ScriptId = "DIACTCMTS";
        public static ProcessLogRun LogRun { get; set; }
        public static int? StartTime { get; set; }
        public static int? EndTime { get; set; }

        /// <summary>
        /// main method
        /// </summary>
        /// <param name="args">args[0] mode
        /// args[1] sets whether it uses the inbound or the outbound process
        /// args[2] if populates will use this value as the start time</param>
        /// args[3] if populates the app will do arc add processing instead of generating a file to send to AES</param>
        /// <returns>0 if success 1 if failure</returns>
        static int Main(string[] args)
        {
            WriteLine($"Version:: {Assembly.GetExecutingAssembly().GetName().Version}");
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), DataAccessHelper.TestMode))
                return 1;


            if (args.Length == 4)//Allow the app to send a specific time in case they want to re-run a time
            {
                StartTime = args[2].ToIntNullable();
                EndTime = args[3].ToIntNullable();
            }

            LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);

            int result = 1;
            bool? outbound = null;
            bool hasArg = args[1].ToLower().IsIn("inbound", "outbound");
            if (args.Length > 1 && hasArg)
                outbound = args[1].ToLower() == "outbound";
            else if (args.Length == 1 || (args.Length > 1 && !hasArg))
            {
                string message = "The script requires a starting process to be set but there is no argument set for determine which process. Please add either inbound or outbound as the second argument.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                WriteLine(message);
            }

            if (outbound.HasValue)
                result = new CommentProcessor().Process(LogRun, outbound.Value);

            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return result;
        }
    }
}