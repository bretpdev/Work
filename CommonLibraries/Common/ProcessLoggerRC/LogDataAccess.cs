using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Uheaa.Common.DataAccess;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;


namespace Uheaa.Common.ProcessLoggerRC
{
    /// <summary>
    /// An interface to DataAccessHelper that automatically logs exceptions to process logger
    /// </summary>
    public class LogDataAccess
    {
        public DataAccessHelper.Mode Mode { get; private set; }
        public int ProcessLogId { get; private set; }
        public bool TryRepeatedly { get; private set; }
        public bool UseManagedConnections { get; private set; }
        private DataAccessHelper.Region? Region { get; set; }
        public LogDataAccess(DataAccessHelper.Mode mode, int processLogId, bool tryRepeatedly, bool useManagedConnections)
        {
            Mode = mode;
            ProcessLogId = processLogId;
            TryRepeatedly = tryRepeatedly;
            UseManagedConnections = useManagedConnections;
        }

        public LogDataAccess(DataAccessHelper.Mode mode, int processLogId, bool tryRepeatedly, bool useManagedConnections, DataAccessHelper.Region region) : this(mode, processLogId, tryRepeatedly, useManagedConnections)
        {
            Region = region;
        }


        public bool Execute(string commandName, DB db, params SqlParameter[] parameters)
        {
            return Execute(commandName, db, 1800, parameters);
        }
        public bool Execute(string commandName, DB db, int connectionTimeout, params SqlParameter[] parameters)
        {
            var getResults = new Action(() =>
            {
                DataAccessHelper.Execute(commandName, db, connectionTimeout, parameters);
            });
            var logException = new Action<Exception>(ex =>
            {
                string args = string.Join(";", parameters.Select(o => o.ParameterName + ":" + (o.Value ?? "").ToString()));
                string message = string.Format("Error attempting to execute sproc {0} on database {1} in mode {2} with these arguments: {3}", commandName, db, this.Mode, args);
                if (Region == null)
                    ProcessLogger.AddNotification(ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetEntryAssembly(), ex);
                else
                    ProcessLogger.AddNotification(ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Region.Value, Assembly.GetEntryAssembly(), ex);
            });
            if (TryRepeatedly)
            {
                var repeaterResult = Repeater.TryRepeatedly(getResults);
                if (!repeaterResult.Successful)
                {
                    foreach (var exception in repeaterResult.CaughtExceptions)
                        logException(exception);
                    return false;
                }
            }
            else
            {
                try
                {
                    getResults();
                }
                catch (Exception ex)
                {
                    logException(ex);
                    return false;
                }
            }
            return true;
        }

        public ManagedDataResult<T> ExecuteSingle<T>(string commandName, DB db, params SqlParameter[] parameters)
        {
            var managedResult = ExecuteList<T>(commandName, db, parameters);
            var newResult = new ManagedDataResult<T>();
            newResult.DatabaseCallSuccessful = managedResult.DatabaseCallSuccessful;
            newResult.CaughtException = managedResult.CaughtException;
            if (managedResult.Result != null)
                newResult.Result = managedResult.Result.SingleOrDefault();
            return newResult;
        }

        public ManagedDataResult<DataTable> ExecuteDataTable(string commandName, DB db, bool throwEx = true, params SqlParameter[] parameters)
        {
            var managedResult = new ManagedDataResult<DataTable>() { DatabaseCallSuccessful = true };
            var getResults = new Action(() =>
            {
                if (UseManagedConnections)
                    managedResult.Result = DataAccessHelper.ExecuteDataTable(commandName, DataAccessHelper.GetManagedConnection(db, Mode), throwEx, parameters);
                else
                    managedResult.Result = DataAccessHelper.ExecuteDataTable(commandName, db, throwEx, parameters);
            });

            var logException = new Action<Exception>(ex =>
            {
                managedResult.DatabaseCallSuccessful = false;
                string args = string.Join(";", parameters.Select(o => o.ParameterName + ":" + (o.Value ?? "").ToString()));
                string message = string.Format("Error attempting to execute sproc {0} on database {1} in mode {2} with these arguments: {3}", commandName, db, this.Mode, args);
                if (Region == null)
                    ProcessLogger.AddNotification(ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetEntryAssembly(), ex);
                else
                    ProcessLogger.AddNotification(ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Region.Value, Assembly.GetEntryAssembly(), ex);
            });
            if (TryRepeatedly)
            {
                var repeaterResult = Repeater.TryRepeatedly(getResults);
                if (!repeaterResult.Successful)
                {
                    foreach (var exception in repeaterResult.CaughtExceptions)
                        logException(exception);
                }
            }
            else
            {
                try
                {
                    getResults();
                }
                catch (Exception ex)
                {
                    logException(ex);
                }
            }
            return managedResult;
        }


        public ManagedDataResult<List<T>> ExecuteList<T>(string commandName, DB db, params SqlParameter[] parameters)
        {
            var managedResult = new ManagedDataResult<List<T>>() { DatabaseCallSuccessful = true };
            var getResults = new Action(() =>
            {
                if (UseManagedConnections)
                    managedResult.Result = DataAccessHelper.ExecuteList<T>(commandName, DataAccessHelper.GetManagedConnection(db, Mode), parameters);
                else
                    managedResult.Result = DataAccessHelper.ExecuteList<T>(commandName, db, Mode, parameters);
            });
            var logException = new Action<Exception>(ex =>
            {
                managedResult.DatabaseCallSuccessful = false;
                string args = string.Join(";", parameters.Select(o => o.ParameterName + ":" + (o.Value ?? "").ToString()));
                string message = string.Format("Error attempting to execute sproc {0} on database {1} in mode {2} with these arguments: {3}", commandName, db, this.Mode, args);
                if (Region == null)
                    ProcessLogger.AddNotification(ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetEntryAssembly(), ex);
                else
                    ProcessLogger.AddNotification(ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Critical, Region.Value, Assembly.GetEntryAssembly(), ex);
            });
            if (TryRepeatedly)
            {
                var repeaterResult = Repeater.TryRepeatedly(getResults);
                if (!repeaterResult.Successful)
                {
                    foreach (var exception in repeaterResult.CaughtExceptions)
                        logException(exception);
                }
            }
            else
            {
                try
                {
                    getResults();
                }
                catch (Exception ex)
                {
                    managedResult.CaughtException = ex;
                    logException(ex);
                }
            }
            return managedResult;
        }

        public int ExecuteId<T>(string commandName, DB db, params SqlParameter[] parameters)
        {
            //SCOPE_IDENTITY() returns a decimal, so we convert to string and them back to int
            //to ensure we get a normal primary key value
            object id = ExecuteSingle<object>(commandName, db, parameters).Result;
            return id.ToString().ToInt();
        }

        public int? ExecuteIdNullable<T>(string commandName, DB db, params SqlParameter[] parameters)
        {
            //SCOPE_IDENTITY() returns a decimal, so we convert to string and them back to int
            //to ensure we get a normal primary key value
            object id = ExecuteSingle<object>(commandName, db, parameters).Result;
            return id.ToString().ToIntNullable();
        }
    }

    public class ManagedDataResult<T>
    {
        public bool DatabaseCallSuccessful { get; set; }
        public Exception CaughtException { get; set; }

        public T Result { get; set; }

        public T CheckResult()
        {
            if (DatabaseCallSuccessful)
                return Result;
            else
                return default(T);
        }
    }
}