using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DeskAudits
{
    static class Program
    {
        public static string ScriptId = "DeskAudits";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true))
                return;

            string errorMessage = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage);
                return;
            }
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            DataAccess da = new DataAccess(logRun);

            string testUserName = null;
            if (DataAccessHelper.TestMode && args.Count() > 1)
            {
                testUserName = args[1].SafeSubString(0, 50); // Used by tester to simulate access for a different user other than themselves
            }
            string userName = !string.IsNullOrWhiteSpace(testUserName) ? testUserName : Environment.UserName; // For testing purposes, override user with passed in test user name

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AuditMainScreen(logRun, da, userName));
        }
    }
}
