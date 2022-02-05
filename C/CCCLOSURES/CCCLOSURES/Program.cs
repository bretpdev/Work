using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CCCLOSURES
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            string scriptId = "CCCLOSURES";

            if (!DataAccessHelper.StandardArgsCheck(args, scriptId))
                return;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;
            
            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DataAccess da = new DataAccess(logRun);
            ShowForm(da);
        }

        private static void ShowForm(DataAccess da)
        {
            using (SelectionScreen ss = new SelectionScreen(da))
            {
                ss.ShowDialog();
            }
        }
    }
}