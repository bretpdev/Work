using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using System.IO;

namespace Uheaa.Common.ProcessLoggerRC
{
    public class ProcessLogRun
    {
        public int ProcessLogId { get; set; }
        public string ScriptId { get; set; }
        public AppDomain AppDomain { get; set; }
        public Assembly Assembly { get; set; }
        public DataAccessHelper.Region Region { get; set; }
        public DataAccessHelper.Mode Mode { get; set; }
        public LogDataAccess LDA { get; set; }
        public ProcessLogRun(string scriptId, AppDomain domain, Assembly assembly, DataAccessHelper.Region region, DataAccessHelper.Mode mode, bool dataAccessTryRepeatedly = false, bool dataAccessUseMonitoredConnections = false)
            : this(scriptId, domain, assembly, region, mode, false, dataAccessTryRepeatedly, dataAccessUseMonitoredConnections)
        {

        }

        public ProcessLogRun(string scriptId, AppDomain domain, Assembly assembly, DataAccessHelper.Region region, DataAccessHelper.Mode mode, bool showDialog, bool dataAccessTryRepeatedly = false, bool dataAccessUseMonitoredConnections = false)
        {
            this.ScriptId = scriptId;
            this.AppDomain = domain;
            this.Assembly = assembly;
            this.Region = region;
            this.Mode = mode;
            ProcessLogId = LogStart(scriptId);
            LDA = new LogDataAccess(mode, ProcessLogId, dataAccessTryRepeatedly, dataAccessUseMonitoredConnections);
#if !DEBUG
            domain.UnhandledException += (o, ea) =>
            {
                int? exceptionId = LogException((Exception)ea.ExceptionObject, assembly);
                LogEnd(exceptionId);
                string message = string.Format("An unexpected error has occurred.  Please contact Systems Support and reference Log Id: {0}", this.ProcessLogId);
                Console.WriteLine(message);
                if (showDialog)
                    Dialog.Error.Ok(message);

                Environment.Exit(1);
            };
#endif
        }

        /// <summary>
        ///  Use this constructor ONLY if using a user script AND it inherits ScriptBase.
        ///  This will use ProcessLogRun and use the ProcessLogData Id from ScriptBase.
        /// </summary>
        /// <param name="processLogId"></param>
        /// <param name="scriptId"></param>
        /// <param name="domain"></param>
        /// <param name="assembly"></param>
        /// <param name="region"></param>
        /// <param name="mode"></param>
        /// <param name="showDialog"></param>
        /// <param name="dataAccessTryRepeatedly"></param>
        /// <param name="dataAccessUseMonitoredConnections"></param>
        public ProcessLogRun(long processLogId, string scriptId, AppDomain domain, Assembly assembly, DataAccessHelper.Region region, DataAccessHelper.Mode mode, bool showDialog, bool dataAccessTryRepeatedly = false, bool dataAccessUseMonitoredConnections = false)
        {
            this.ScriptId = scriptId;
            this.AppDomain = domain;
            this.Assembly = assembly;
            this.Region = region;
            this.Mode = mode;
            this.ProcessLogId = (int)processLogId;
            LDA = new LogDataAccess(mode, ProcessLogId, dataAccessTryRepeatedly, dataAccessUseMonitoredConnections);
#if !DEBUG
            domain.UnhandledException += (o, ea) =>
            {
                int? exceptionId = LogException((Exception)ea.ExceptionObject, assembly);
                LogEnd(exceptionId);
                string message = string.Format("An unexpected error has occurred.  Please contact Systems Support and reference Log Id: {0}", this.ProcessLogId);
                Console.WriteLine(message);
                if (showDialog)
                    Dialog.Error.Ok(message);

                Environment.Exit(1);
            };
#endif
        }

        /// <summary>
        ///  Use this constructor ONLY if using a user script AND it inherits ScriptBase.
        ///  This will use ProcessLogRun and use the ProcessLogData Id from ScriptBase.
        /// </summary>
        /// <param name="processLogId"></param>
        /// <param name="scriptId"></param>
        /// <param name="domain"></param>
        /// <param name="assembly"></param>
        /// <param name="region"></param>
        /// <param name="mode"></param>
        /// <param name="showDialog"></param>
        /// <param name="dataAccessTryRepeatedly"></param>
        /// <param name="dataAccessUseMonitoredConnections"></param>
        public ProcessLogRun(string scriptId, AppDomain domain, Assembly assembly, DataAccessHelper.Region region, DataAccessHelper.Mode mode, bool showDialog, bool dataAccessTryRepeatedly = false, bool dataAccessUseMonitoredConnections = false, string runBy = "")
        {
            this.ScriptId = scriptId;
            this.AppDomain = domain;
            this.Assembly = assembly;
            this.Region = region;
            this.Mode = mode;
            this.ProcessLogId = LogStart(ScriptId, runBy);
            LDA = new LogDataAccess(mode, ProcessLogId, dataAccessTryRepeatedly, dataAccessUseMonitoredConnections);
#if !DEBUG
            domain.UnhandledException += (o, ea) =>
            {
                int? exceptionId = LogException((Exception)ea.ExceptionObject, assembly);
                LogEnd(exceptionId);
                string message = string.Format("An unexpected error has occurred.  Please contact Systems Support and reference Log Id: {0}", this.ProcessLogId);
                Console.WriteLine(message);
                if (showDialog)
                    Dialog.Error.Ok(message);

                Environment.Exit(1);
            };
#endif
        }

        /// <summary>
        /// Logs an exception to ProcessLogs
        /// </summary>
        /// <param name="ex">Exception to log</param>
        /// <param name="assembly">Assembly.GetExecutingAssembly</param>
        /// <returns>Exception Log Id from ProcessLogs</returns>
        [UsesSproc(DataAccessHelper.Database.RCProcessLogs, "ExceptionLogInsert")]
        private int LogException(Exception ex, Assembly assembly, int? notificationId = null)
        {
            var parameters = new
            {
                ExceptionType = ex.GetType().FullName,
                AssemblyLocation = assembly.IsNull(o => o.Location),
                AssemblyFullName = assembly.IsNull(o => o.FullName),
                AssemblyLastModified = assembly.IsNull(o => File.GetLastWriteTime(o.Location), DateTime.Now),
                ExceptionSource = ex.Source,
                ExceptionMessage = ex.Message,
                StackTrace = ex.StackTrace,
                FullDetails = ex.ToString(),
                ProcessLogId = this.ProcessLogId,
                ProcessNotificationId = notificationId

            };
            return LDA.ExecuteSingle<int>("ExceptionLogInsert", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters()).Result;
        }

        /// <summary>
        /// Adds a Notification to the List of Notifications that will be added to the database
        /// </summary>
        /// <param name="message">Reason for the notification</param>
        /// <param name="notifyType">Type of Notification</param>
        /// <param name="severityType">Level of severity</param>
        public int AddNotification(string message, NotificationType notifyType, NotificationSeverityType severityType, Exception ex = null)
        {
            message = string.Format("{0}", message);
            //Console.WriteLine(message);
            //Insert the notification
            var parameters = new
            {
                NotificationTypeId = (int)notifyType,
                NotificationSeverityTypeId = (int)severityType,
                ScriptLogId = this.ProcessLogId
            };
            int notificationId = DataAccessHelper.ExecuteSingle<int>("ProcessNotificationInsert", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters());

            //Insert the Notification Message
            var messageParams = new
            {
                ProcessNotificationId = notificationId,
                LogMessage = string.Format("{0}; {1}", DateTime.Now.ToString(), message)
            };

            LDA.Execute("[log].NotificationInsert", DataAccessHelper.Database.Voyager, messageParams.SqlParameters());

            if (ex != null)
                LogException(ex, Assembly, notificationId);

            return notificationId;
        }

        /// <summary>
        /// Logs the EndTime for a given ProcessLogId
        /// </summary>
        /// <param name="processLogId">Current ProcessLogId</param>
        /// <param name="exceptionId">Exception Id if an exception was logged.</param>
        public void LogEnd(int? exceptionId = null)
        {
            var parameters = new
            {
                ProcessLogId = this.ProcessLogId,
                ExceptionId = exceptionId
            };

            LDA.Execute("EndScriptRun", DataAccessHelper.Database.RCProcessLogs, parameters.SqlParameters());
        }

        private int LogStart(string scriptId)
        {
            var parameters = new
            {
                ScriptId = scriptId,
                Region = ResolveRegion(Region),
                RunBy = Environment.UserName
            };
            return DataAccessHelper.ExecuteSingle<ProcessLogData>("InsertProcessStart", DataAccessHelper.Database.RCProcessLogs, Mode, parameters.SqlParameters()).ProcessLogId;
        }
        private int LogStart(string scriptId, string runBy)
        {
            var parameters = new
            {
                ScriptId = scriptId,
                Region = ResolveRegion(Region),
                RunBy = runBy
            };
            return DataAccessHelper.ExecuteSingle<ProcessLogData>("InsertProcessStart", DataAccessHelper.Database.RCProcessLogs, Mode, parameters.SqlParameters()).ProcessLogId;
        }

        private string ResolveRegion(DataAccessHelper.Region region)
        {
            return region.ToString().ToLower();
        }
    }
}
