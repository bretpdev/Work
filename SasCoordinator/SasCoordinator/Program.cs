using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SasCoordinator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            var validationResults = KvpArgValidator.ValidateArguments<Args>(args);
            if (validationResults.IsValid)
            {
                var arguments = new Args(args);
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live; // We are not doing duster test through this
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

                if (string.IsNullOrEmpty(arguments.ProcessLoggerScriptId))
                    arguments.ProcessLoggerScriptId = "SasCoordinator";
                var pld = ProcessLogger.RegisterApplication(arguments.ProcessLoggerScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), false);
                string username = null; string password = null;
                ProcessLogRun plr = new ProcessLogRun(pld.ProcessLogId, arguments.ProcessLoggerScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
                DataAccess da = new DataAccess(plr);

                var loginHelper = BatchProcessingHelper.GetNextAvailableId("SasCoordinator", "SAS");
                username = loginHelper.UserName;
                password = da.GetBatchProcessingPassword(loginHelper.UserName);
                plr.AddNotification("Using Credentials: " + username, NotificationType.EndOfJob, NotificationSeverityType.Informational);

                var coordinator = new Coordinator(username, password, arguments.SysParm, arguments.SasFileLocation, arguments.SasRegion, pld);
                if (coordinator.InitializationErrors.Any())
                {
                    Console.WriteLine(string.Join(Environment.NewLine, coordinator.InitializationErrors));
                    return 1;
                }
                plr.AddNotification("Launching Sas File " + arguments.SasFileLocation, NotificationType.EndOfJob, NotificationSeverityType.Informational);
                coordinator.Coordinate();
                plr.LogEnd();
            }
            else
            {
                Console.WriteLine(validationResults.ValidationMesssage);
                return 1;
            }
            return 0;
        }
    }
}
