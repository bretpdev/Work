using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace XmlGeneratorECorr
{
    static class Program
    {
        public static ProcessLogData LogData { get; set; }
        /// <summary>
        /// Args: {datamode} {programmode} {onlyrunbilling}
        /// Ex: live console cornerstone = console in live mode billing
        /// Ex: test gui = gui in test mode
        /// </summary>
        [STAThread]
        static int Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Region will be set as DataAccess.CurrentRegion.
            if (DataAccessHelper.StandardArgsCheck(args, "ECORXMLFED"))
            {
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
                //Determines if the batch or GUI mode will be selected.
                string programMode = args.Skip(1).FirstOrDefault();
                bool onlyProcessBills = !args.Skip(2).FirstOrDefault().IsNullOrEmpty();
                LogData = ProcessLogger.RegisterApplication("E-Corr XML Generator - FED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), false);

                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                    return 1;

                bool console = programMode == "console";

                if (programMode.IsNullOrEmpty())
                {
                    using (ModeChooser modeChooser = new ModeChooser())
                    {
                        if (modeChooser.ShowDialog() != DialogResult.OK)
                            return 0;

                        console = modeChooser.Console;
                    }
                }

                int? access = AccessCheck(console);
                if (access.HasValue) return access.Value;
                if (!console) //gui
                    StartGuiMode();
                else //command line, although technically we don't use the console
                    StartXmlGenerator(onlyProcessBills);

                ProcessLogger.LogEnd(LogData.ProcessLogId);
                return 0;
            }
            else
                return 1;
        }

        /// <summary>
        /// Starts the GUI part of the application.
        /// </summary>
        private static void StartGuiMode()
        {
            Application.Run(new EcorrUpdater());
        }

        private static void StartXmlGenerator(bool onlyRunBilling)
        {
            XmlGenerator.GenerateXml(LogData, onlyRunBilling);
        }

        /// <summary>
        /// Checks to see if the user has access to the stored procedures this applicaion uses
        /// </summary>
        /// <param name="console">If true will not provide prompt to the user.</param>
        /// <returns>1 if user does not have access</returns>
        private static int? AccessCheck(bool console)
        {
            string alert = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            if (!alert.IsNullOrEmpty())
            {
                if (console)
                {
                    Console.WriteLine("Current Username: " + Environment.UserName);
                    Console.WriteLine("Current HostName: " + Dns.GetHostName());
                    Console.WriteLine("Connection String: " + DataAccessHelper.GetConnectionString(DataAccessHelper.Database.ECorrFed));
                    Console.WriteLine(alert);
                }
                else
                    MessageBox.Show(alert);
                return 1;
            }
            return null;
        }
    }
}
