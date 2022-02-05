using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.ProcessLoggerRC
{
    public enum NotificationSeverityType
    {
        Informational,
        Warning,
        Critical
    }

    public enum NotificationType
    {
        NoFile = 0,
        FileFormatProblem,
        EmptyFile,
        ErrorReport,
        EndOfJob,
        Other,
        HandledException
    }

    public static class ProcessLogger
    {
        /// <summary>
        /// Logs the application start time
        /// </summary>
        /// <param name="applicationName">Script Id from SACKER</param>
        /// <param name="domain">AppDomain.CurrentDomain</param>
        /// <param name="assembly">Assembly.GetExecutingAssembly</param>
        /// <returns>ProcessLogsData object with Id and start time, current domain, and executing assembly</returns>
        public static ProcessLogData RegisterApplication(string applicationName, AppDomain domain, Assembly assembly)
        {
            return Register(applicationName, domain, assembly, true);
        }

        public static ProcessLogData RegisterApplication(string applicationName, AppDomain domain, Assembly assembly, bool showMessage)
        {
            return Register(applicationName, domain, assembly, showMessage);
        }

        /// <summary>
        /// Logs the scripts start time
        /// </summary>
        /// <param name="scriptId">Script Id from SACKER</param>
        /// <param name="domain">AppDomain.CurrentDomain</param>
        /// <param name="assembly">Assembly.GetExecutingAssembly</param>
        /// <returns>ProcessLogsData object with Id and start time, current domain, and executing assembly</returns>
        public static ProcessLogData RegisterScript(string scriptId, AppDomain domain, Assembly assembly)
        {
            return Register(scriptId, domain, assembly, true);
        }

        public static ProcessLogData RegisterQScript(bool testMode, string region, string scriptId, AppDomain domain, Assembly assembly)
        {
            if (testMode)
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            else
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            return Register(scriptId, domain, assembly, true);
        }

        public static void RegisterExceptionOnly(string id, AppDomain domain, Assembly assembly)
        {
#if !DEBUG
            domain.UnhandledException += (o, ea) =>
            {
                ProcessLogData logData = LogStart(id);
                int? exceptionId = LogException((Exception)ea.ExceptionObject, assembly, logData.ProcessLogId);
                LogEnd(logData.ProcessLogId, exceptionId);
                MessageBox.Show(string.Format("An unexpected error has occurred.  Please contact Systems Support and reference Log Id: {0}", logData.ProcessLogId), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            };
#endif
        }

        private static ProcessLogData Register(string id, AppDomain domain, Assembly assembly, bool showMessage)
        {
            ProcessLogData logData = LogStart(id);
            logData.Domain = domain;
            logData.ExecutingAssembly = assembly;
#if !DEBUG
            domain.UnhandledException += (o, ea) =>
            {
                int? exceptionId = LogException((Exception)ea.ExceptionObject, assembly, logData.ProcessLogId);
                LogEnd(logData.ProcessLogId, exceptionId);
                string message = string.Format("An unexpected error has occurred.  Please contact Systems Support and reference Log Id: {0}", logData.ProcessLogId);
                if (showMessage)
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    Console.WriteLine(message);
                Environment.Exit(1);
            };
#endif

            return logData;
        }

        /// <summary>
        /// Logs an exception to ProcessLogs
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="assembly">Assembly.GetExecutingAssembly</param>
        /// <returns>Exception Log Id from ProcessLogs</returns>
        [UsesSproc(DataAccessHelper.Database.RCProcessLogs, "ExceptionLogInsert")]
        private static int LogException(Exception ex, Assembly assembly, int processLogId, int? notificationId = null)
        {
            var parameters = new
            {
                ExceptionType = ex.GetType().FullName,
                AssemblyLocation = assembly.IsNull(o => o.Location),
                AssemblyFullName = assembly.IsNull(o => o.FullName),
                AssemblyLastModified = assembly != null ? assembly.IsNull(o => File.GetLastWriteTime(o.Location), DateTime.Now) : new DateTime(1900, 1, 1),
                ExceptionSource = ex.Source,
                ExceptionMessage = ex.Message,
                StackTrace = "Executing code\r\n\r\n" + ex.StackTrace + "\r\n\r\nEnvironment Stack Trace\r\n\r\n" + Environment.StackTrace,
                FullDetails = ex.ToString(),
                ProcessLogId = processLogId,
                ProcessNotificationId = notificationId

            };
            int exceptionLogId = 0;
            Repeater.TryRepeatedly(() =>
            {
                exceptionLogId = DataAccessHelper.ExecuteSingle<int>("ExceptionLogInsert", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters());
            });
            return exceptionLogId;
        }

        /// <summary>
        /// Adds a Notification to the List of Notifications that will be added to the database
        /// </summary>
        /// <param name="message">Reason for the notification</param>
        /// <param name="notifyType">Type of Notification</param>
        /// <param name="severityType">Level of severity</param>
        public static void AddNotification(int processLogId, string message, NotificationType notifyType, NotificationSeverityType severityType, DataAccessHelper.Region region, Assembly assembly = null, Exception ex = null)
        {
            Console.WriteLine(message);
            //Insert the notification
            var parameters = new
            {
                NotificationTypeId = (int)notifyType,
                NotificationSeverityTypeId = (int)severityType,
                ScriptLogId = processLogId
            };
            int notificationId = DataAccessHelper.ExecuteSingle<int>("ProcessNotificationInsert", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters());

            //Insert the Notification Message
            var messageParams = new
            {
                ProcessNotificationId = notificationId,
                LogMessage = string.Format("{0}; {1}", DateTime.Now.ToString(), message)
            };

            Repeater.TryRepeatedly(() =>
            {
                DataAccessHelper.Execute("[log].NotificationInsert", DataAccessHelper.Database.Voyager, messageParams.SqlParameters());
            });
            if (ex != null)
                LogException(ex, assembly, processLogId, notificationId);
        }

        /// <summary>
        /// Adds a Notification to the List of Notifications that will be added to the database
        /// </summary>
        /// <param name="message">Reason for the notification</param>
        /// <param name="notifyType">Type of Notification</param>
        /// <param name="severityType">Level of severity</param>
        public static void AddNotification(int processLogId, string message, NotificationType notifyType, NotificationSeverityType severityType, Assembly assembly = null, Exception ex = null)
        {
            AddNotification(processLogId, message, notifyType, severityType, DataAccessHelper.CurrentRegion, assembly, ex);
        }

        /// <summary>
        /// Logs the EndTime for a given ProcessLogId
        /// </summary>
        /// <param name="processLogId">Current ProcessLogId</param>
        /// <param name="exceptionId">Exception Id if an exception was logged.</param>
        public static void LogEnd(int processLogId, int? exceptionId = null)
        {
            var parameters = new
            {
                ProcessLogId = processLogId,
                ExceptionId = exceptionId
            };

            DataAccessHelper.Execute("EndScriptRun", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters());
        }

        private static ProcessLogData LogStart(string scriptId)
        {
            var parameters = new
            {
                ScriptId = scriptId,
                Region = ResolveRegion(DataAccessHelper.CurrentRegion),
                RunBy = Environment.UserName
            };

            return DataAccessHelper.ExecuteSingle<ProcessLogData>("InsertProcessStart", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters());
        }

        private static string ResolveRegion(DataAccessHelper.Region region)
        {
            return region.ToString().ToLower();
        }


    }
}
