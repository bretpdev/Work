using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace RTRNMAILUH
{
    public class Program
    {
        public const int ERROR = 1;
        public static string ScriptId { get; set; } = "RTRNMAILUH";

        public static int Main(string[] args)
        {
            WriteLine($"Return Mail Batch Script - UHEAA Compass ::  Version: {Assembly.GetEntryAssembly().GetName().Version}");
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var argResults = KvpArgValidator.ValidateArguments<Args>(args);
            if (!argResults.IsValid)
            {
                WriteLine($"{ScriptId} was started without the mode being set. Please pass in the mode and try again.");
                return ERROR;
            }
            var parsedArgs = new Args(args);
            DataAccessHelper.CurrentMode = parsedArgs.Mode;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                WriteLine($"{ScriptId} does not have access to execute the stored procedures used in this script.");
                return ERROR;
            }

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            ri.LogRun = logRun;
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa", true);
            
            bool isLoggedIn = true;
            if (helper.UserName.IsNullOrEmpty())
            {
                WriteLine($"There were no available batch id's and a the session could not log in. Please try again later.");
                isLoggedIn = false;
            }
            int run = 0;
            if (isLoggedIn)
            {
                WriteLine($"Logging into session with User ID: {helper.UserName}");
                run = new Process(ri).Run(parsedArgs.PauseBetweenRecords);
            }
            WriteLine("Processing Complete");

            logRun.LogEnd();
            ri.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();

            return run;
        }
    }
}