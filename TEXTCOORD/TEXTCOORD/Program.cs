using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace TEXTCOORD
{

    static class Program
    {
        private static readonly string ScriptId = "TEXTCOORD";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            ProcessLogRun plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DataAccess da = new DataAccess(plr.LDA);
            if (!da.InsertApp1Data())
            {
                string message = $"There was an issue loading the data from the UheaaAPP1 server. Please try again or contact System Support for assistance.";
                plr.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(message);
                plr.LogEnd();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using Search sr = new Search(da);
            sr.ShowDialog();
            plr.LogEnd();
        }
    }
}