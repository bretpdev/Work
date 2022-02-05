using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace VERFORBUH
{
    public class Program
    {
        public static readonly string ScriptId = "VERFORBUH";
        public static ProcessLogData LogData { get; set; }

        public ProcessLogRun logRun { get; set; }
        private static ReflectionInterface RI { get; set; }
        

        public static void MDMain(DataAccessHelper.Mode mode, DataAccessHelper.Region region, ReflectionInterface ri, string accountNumber)
        {
            RI = ri;
            SharedMain(new string[] { mode.ToString().ToLower(), region.ToString(), accountNumber });
        }

        /// <summary>
        /// The main entry point for the application.
        /// args[0] mode as string
        /// args[1] account number (not required)
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            List<Process> adProcess = Process.GetProcesses().ToList();
            adProcess = adProcess.Where(p => p.ProcessName.Contains(ScriptId)).ToList();

            if (adProcess.Count() > 1)
            {
                Dialog.Error.Ok("You already have a verbal forbearance script running.");
                return 0;
            }

            return SharedMain(args);
        }

        static int SharedMain(string[] args)
        {
            if (!ValidateApp(args))
                return 1;

            //if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            //    return 1;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true, false);
            bool createdSession = false;
            if (RI == null)
            {
                RI = new ReflectionInterface();
                createdSession = true;
                bool loggedIn = false;
                while (!loggedIn)
                {
                    using (Login login = new Login())
                    {
                        if (login.ShowDialog() == DialogResult.Cancel)
                        {
                            RI.CloseSession();
                            return 0;
                        }
                        loggedIn = RI.Login(login.txtUtId.Text, login.txtPassword.Text);
                    }
                }
            }

            var result = new VerbalForbearanceScript(RI, args.Length != 3 ? GetAccountNumber() : args[2], logRun).Run();
            DataAccessHelper.CloseAllManagedConnections();
            if (createdSession)
                RI.CloseSession();
            return result;
        }

        private static string GetAccountNumber()
        {
            string accountNumber = string.Empty;
            while (accountNumber.Length != 10)
            {
                string message = "Unable to grab account number from MauiDude.  Pleae enter the account number.";
                using (var input = new InputBox<AccountNumberTextBox>(message, "Enter Account Number"))
                {
                    if (input.ShowDialog() == DialogResult.OK)
                        accountNumber = input.InputControl.Text;
                    else
                        Environment.Exit(1);
                }

                if (accountNumber.Trim().Length != 10)
                    Dialog.Error.Ok("You must enter an account number.");
            }
            return accountNumber;
        }

        private static bool ValidateApp(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return false;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            return true;
        }
    }
}
