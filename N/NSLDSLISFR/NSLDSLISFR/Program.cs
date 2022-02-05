using System;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace NSLDSLISFR
{
    static class Program
    {
        public const string ScriptId = "NSLDSLISFR";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun plr = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true, false);

            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper batchId = BatchProcessingLoginHelper.Login(plr, ri, ScriptId, "BatchCornerstone");
            if (batchId == null)
            {
                plr.AddNotification("There were no available ID's in the database to log into a session", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return 1;
            }
            string singleBorrowerIdentifier = null;
            if (args.Count() > 1) // Checking args to ensure non-malevolent input. Second arg should be account identifier
            {
                if (((args[1].Length != 9 && args[1].Length != 10) || !args[1].IsNumeric()) || args.Count() > 2)
                {
                    plr.AddNotification("Incorrect arguments passed into the application", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    BatchProcessingHelper.CloseConnection(batchId);
                    ri.CloseSession();
                    plr.LogEnd();
                    return 1;
                }
                singleBorrowerIdentifier = args.Skip(1).FirstOrDefault();
            }
            new NSLDSLISFR(ri, plr).Process(singleBorrowerIdentifier);

            BatchProcessingHelper.CloseConnection(batchId);
            ri.CloseSession();
            plr.LogEnd();

            return 0;
        }
    }
}