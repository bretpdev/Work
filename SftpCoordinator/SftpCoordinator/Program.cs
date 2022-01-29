using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    static class Program
    {
        public static ProcessLogRun PLR { get; set; }

        /// <summary>
        /// Args: {datamode} {programmode}
        /// Ex: live console logging = console in live mode
        /// Ex: test gui = gui in test mode
        /// </summary>

        [STAThread]
        static int Main(params string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "SftpCoordinator", false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string programMode = args.Skip(1).FirstOrDefault();
            bool console = programMode == "console";

            PLR = new ProcessLogRun("SftpCoordi", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true, true);

            //string filename = "test.txt";
            //File.WriteAllText(filename, "hellotest");
            //string encryptedFilename = CryptographyHelper.Encrypt(filename);
            //File.Delete(filename);
            //filename = CryptographyHelper.Decrypt(encryptedFilename);



            try
            {
                if (!console) //gui
                    StartDashboard();
                else //command line, although technically we don't use the console
                    StartConsole();
            }
            catch (Exception ex)
            {
                PLR.AddNotification($"Unable to launch {args[1]} application with the following arguements: {args[0]}, {args[1]}", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                if (!console)
                    MessageBox.Show(ex.ToString());
                PLR.LogEnd();
                return 1;
            }
            PLR.LogEnd();
            return 0;
        }

        private static void StartDashboard()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Dashboard.Instance);
        }

        private static void StartConsole()
        {
            Coordinator coordinator = new Coordinator();
            coordinator.Run(PLR);
        }
    }
}
