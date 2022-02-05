using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Web;
using Uheaa.Common.DataAccess;

namespace SchedulerWeb
{
    public class SchedulerHelper
    {
        private readonly string schedulerLocation;
        public SchedulerHelper()
        {
            schedulerLocation = Path.Combine(System.Web.HttpRuntime.BinDirectory, "Scheduler.exe");
        }
        public bool IsRunning
        {
            get
            {
                return Process.GetProcessesByName("Scheduler").Any();
            }
        }

        [UsesSproc(DataAccessHelper.Database.ProcessLogs, "GetLastEndTime")]
        public DateTime? GetLastRunTime()
        {
            return DataAccessHelper.ExecuteList<DateTime?>("GetLastEndTime", DataAccessHelper.Database.ProcessLogs, SqlParams.Single("ScriptId", "Scheduler")).SingleOrDefault();
        }

        public void Run()
        {
            Process.Start(schedulerLocation, DataAccessHelper.CurrentMode.ToString().ToLower()).WaitForExit();
        }
    }
}