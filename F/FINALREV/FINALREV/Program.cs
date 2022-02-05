using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace FINALREV
{
    public class Program
    {
        public const string ScriptId = "FINALREV";
        public static int ERROR { get; set; } = 1;

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!KvpArgValidator.ValidateArguments<Args>(args).IsValid)
            {
                WriteLine("The mode was not passed into the script, please set the mode and try again.");
                return ERROR;
            }
            var passedArgs = new Args(args);
            DataAccessHelper.CurrentMode = passedArgs.Mode;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return ERROR;

            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ri.LogRun = logRun;
            if (BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa") != null)
            {
                WriteLine($"Created session and logged in with {ri.UserId}");

                int hadError =new FinalReview(ri).Process(passedArgs.PauseBetweenRecords);
                ri.CloseSession();
                logRun.LogEnd();

                WriteLine($"Process complete{(hadError == 1 ? " with errors" : "")}. Closing session and ending process log.");
                return hadError;
            }
            else
            {
                logRun.LogEnd();
                ri.CloseSession();
                return ERROR;
            }
        }
    }
}