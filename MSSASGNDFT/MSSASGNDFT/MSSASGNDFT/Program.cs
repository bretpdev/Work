using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace MassAssignBatch
{
    public static class Program
    {
    

        public static string ScriptId { get; set; } = "MSSASGNDFT";
        [STAThread]
        public static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), args.Count() > 0 ? args[0] : "dev", true);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogRun LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, false);
            WriteLine("Mass Assign Batch :: " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            DataAccess da = new DataAccess(LogRun);
            List<MassAssignRangeAssignment.SqlUser> users = da.GetUsers();
            int returnCode = 1;
            if(da.HasAccess("MassAssignRun", users.Where(p => p.WindowsUserName == Environment.UserName).SingleOrDefault()) || Environment.UserName.ToLower() == "batchscripts")
                returnCode = new MassAssignBatchProcess(LogRun, users, da, args.Count() > 1 ? args[1] : "").Process();
           else if (da.HasAccess("MassAssignRange", users.Where(p => p.WindowsUserName == Environment.UserName).SingleOrDefault()))
            {
                returnCode = new UserProcessing().Process(LogRun, users);
            }
            else
            {
                string message = string.Format("{0} tried running the Mass Assign Batch application but does not have access.", Environment.UserName);
                WriteLine(message);
                ProcessLogger.AddNotification(LogRun.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }

            LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            WriteLine(returnCode.ToString());
            return returnCode;
        }

      
    }
}