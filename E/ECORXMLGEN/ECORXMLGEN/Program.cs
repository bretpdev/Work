using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECORXMLGEN
{
    class Program
    {
        
        public static string ScriptId = "ECORXMLGEN";
        public static ProcessLogRun PL { get; set; }
        public static bool SendFiles { get; set; }
        public static int NumberOfThreads { get; set; }
        public static int SkipDays { get; set; }
        //ARGS --[0] mode; [1] region; [2] showPromts; [3] Number of thread for ECORXMLGEN; [4] SkipDays for Extractor
        static int Main(string[] args)
        {
            bool showPrompts = (args.Any() && args[2].ToUpper() == "SHOWPROMPTS");
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPrompts))
                return 1;

            if (args.Count() < 2)
            {
                Console.WriteLine("REGION NOT PROVIDED.");
                return 1;
            }
            if (args[1].ToUpper() == "CORNERSTONE")
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            else if (args[1].ToUpper() == "UHEAA")
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            else
            {
                Console.WriteLine("UNVALID REGION PARAMETER: {0}", args[1].ToUpper());
                return 1;
            }

            SetNumberOfThreads(args);
            SkipDays = args.Length > 3 ? args[4].ToInt() : 7;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), showPrompts))
                return 1;

            PL = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            List<Process> adProcess = Process.GetProcesses().ToList();
            adProcess = adProcess.Where(p => p.ProcessName.Contains(ScriptId)).ToList();

            if (adProcess.Count() > 1)
            {
                ReportAndEnd(showPrompts);
                return 0;
            }

            SetSendFiles();
            int retVal = new XMLGenerator(PL).GenerateXml();

            Console.WriteLine(retVal);

            PL.LogEnd();
            Console.WriteLine("Closing Database Connections.  Please wait.");

            return retVal;
        }

        private static void SetNumberOfThreads(string[] args)
        {
            if (args.Count() == 4)
                NumberOfThreads = args[3].ToInt();
            else
                NumberOfThreads = 50;
        }

        private static void SetSendFiles()
        {
            if (DataAccessHelper.TestMode)
                SendFiles = Dialog.Info.YesNo("Do you want to send the test files created to AES?");
            else
                SendFiles = true;
        }

        private static void ReportAndEnd(bool showPrompts)
        {
            string message = "ECORXMLGEN is already running running.";
            PL.AddNotification(message, NotificationType.Other, NotificationSeverityType.Informational);
            if (showPrompts)
                Dialog.Error.Ok(message);
            else
                Console.WriteLine(message);

            PL.LogEnd();
        }
    }
}
