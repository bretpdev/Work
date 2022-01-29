using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security;

namespace MD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if ((DateTime.Now - Properties.Settings.Default.LastRun).TotalSeconds < 3)
                Hlpr.UI.QuitApplication();  //fix for the ACDC double-click issue, suppress double clicking within 3 seconds.
            Properties.Settings.Default.LastRun = DateTime.Now;
            Properties.Settings.Default.Save();
            ReflectionHelper.ErrorHook();
            AppDomain.CurrentDomain.UnhandledException += (o, ea) =>
            {
                if (!(ea.ExceptionObject is COMException))
                    MessageBox.Show(ea.ExceptionObject.ToString());
            };
            Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.ThrowException);
            string location = Assembly.GetExecutingAssembly().Location.ToLower();
            if (location.StartsWith(@"\\uheaa-fs\DEVSEASCS\Codebase\Applications\".ToLower()) || location.StartsWith(@"Y:\Codebase\Applications".ToLower()))
            {
                MessageBox.Show("MD should not be opened from a network drive.  This incident has been reported to your supervisor.");
                return;
            }
#if DEBUG
            // foreach (Process p in Process.GetProcessesByName("R8Win"))
            //       p.FS.Delete();
#endif
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
#else
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
#endif

            Hlpr.Instantiate();
            using (var login = Hlpr.UI.CreateAndShow<LoginForm>())
            {
                if (args.Any())
                {
                    string username = args[0];
                    //string password = args[1] //Not used anymore, removing
                    bool testMode = args[2] != "live";
                    string accountNumber = args[3];
                    login.Hide();
                    login.DemoLogin(username, testMode, accountNumber);
                }
                Application.Run();
            }
        }
    }
}
