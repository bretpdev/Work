using System;
using System.Linq;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHRIRDF
{
    class Program
    {
        public const string ScriptId = "ACHRIRDF";
        const string SkipWorkCode = "skipworkadd";

        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ProcessLogRun plr = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true, false);
            var data = new DataAccess(plr.ProcessLogId);


            if (!args.Contains(SkipWorkCode))
            {
                Console.WriteLine("Adding new work to queue...");
                data.AddNewWorkToQueue();
            }

            var ri = new ReflectionInterface();
            BatchProcessingHelper batchId = BatchProcessingLoginHelper.Login(plr, ri, ScriptId, "BatchUheaa");
            if (batchId == null)
            {
                plr.AddNotification("There were no available ID's in the database to log into a session", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return 1;
            }

            new ACHRIRDF(ri, plr, data).Process();

            ri.CloseSession();

            DataAccessHelper.CloseAllManagedConnections();
            BatchProcessingHelper.CloseConnection(batchId);
            plr.LogEnd();
            return 0;
        }
    }
}