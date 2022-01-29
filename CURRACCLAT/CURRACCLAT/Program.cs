using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CURRACCLAT
{
    public class Program
    {
        public static string ScriptId { get; set; } = "CURRACCLAT";
        public static int ERROR { get; set; } = 1;

        [STAThread]
        public static int Main(string[] args)
        {
            Console.WriteLine($"WG000001 Create Tasks Application :: Version {Assembly.GetEntryAssembly().GetName().Version}");
            var argResults = KvpArgValidator.ValidateArguments<Args>(args);
            var passedArgs = new Args(args);
            DataAccessHelper.CurrentMode = passedArgs.Mode;
            if (!argResults.IsValid)
            {
                Console.WriteLine($"{ScriptId} was started without the mode being set. Please pass in the mode and try again.");
                return ERROR;
            }
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                Console.WriteLine($"{ScriptId} does not have access to execute the stored procedures in the curracclat schema.");
                return ERROR;
            }

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ReflectionInterface RI = new ReflectionInterface();
            RI.LogRun = logRun;
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, RI, ScriptId, "BatchUheaa");

            Console.WriteLine("Logging into session with user id: {0}", helper.UserName);

            int run = new CreateTasks(ScriptId, RI).Process(passedArgs.PauseBetweenRecords);

            Console.WriteLine("Finished processing. Closing session and setting Process Log end time");
            logRun.LogEnd();
            RI.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();

            return run;
        }
    }
}