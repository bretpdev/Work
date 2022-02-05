using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace PHONESUCSN
{
    public class Program
    {
        public static string ScriptId { get; set; } = "PHONESUCSN";

        public static int Main(string[] args)
        {
            WriteLine($"{ScriptId} :: Version:{Assembly.GetExecutingAssembly().GetName().Version}");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "PHONESUSCN", false))
                return 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            return Process(args);
        }

        private static int Process(string[] args)
        {
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            BatchProcessingHelper login = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            if (login.UserName.IsNullOrEmpty())
            {
                logRun.AddNotification("There was an error retreiving a batch id to log into a session, please try again.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return 1;
            }
            else
                WriteLine($"Logging into the session with ID: {login.UserName}");
            ri.LogRun = logRun;
            DataAccess da = new DataAccess(logRun);
            int count = args.Length > 2 ? args[2].ToInt() : 1000; //Process 1000 per day but can be changed to more or less
            UpdateHelper helper = new UpdateHelper(ri, da);

            if (args.Length > 1) //This is for testing and allows the tester to run any process they want
            {
                if (args[1].ToLower() == "succession")
                    new ProcessSuccession(helper, count).Process();
                else if (args[1].ToLower() == "duplicate")
                    new InvalidateDuplicate(helper, count).Process();
            }
            else
            {
                TimeSpan current = DateTime.Now.TimeOfDay;
                TimeSpan noon = new TimeSpan(12, 0, 0);
                //When running in the morning, do the phone succession. Invalidate duplicate after noon.
                if (current < noon)
                    new ProcessSuccession(helper, count).Process();
                else if (current > noon)
                    new InvalidateDuplicate(helper, count).Process();
            }

            WriteLine("Processing Complete");
            DataAccessHelper.CloseAllManagedConnections();
            ri.CloseSession();
            return 0;
        }
    }
}