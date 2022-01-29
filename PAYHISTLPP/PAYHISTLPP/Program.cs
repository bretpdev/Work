using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PAYHISTLPP
{
    public class Program
    {
        public const string ScriptId = "PAYHISTLPP";
        public static DateTime SaleDate { get; set; } = new DateTime(0001, 01, 01);

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true, DataAccessHelper.Database.ProcessLogs);

            new CreateReport(logRun).Main();

            DataAccessHelper.CloseAllManagedConnections();
            logRun.LogEnd();
        }
    }
}