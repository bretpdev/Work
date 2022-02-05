using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECORRSLFED
{
    static class Program
    {
        public static string ScriptId = "ECORRSLFED";
        public static ProcessLogRun PL { get; set; }
        public static int NumberOfThreads { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            bool showPrompts = (args.Any() && args.Length == 1);
            NumberOfThreads = args.Skip(2).Any() ? args[2].ToInt() : int.MaxValue;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPrompts))
            {
                Console.WriteLine("Unable to parse command line args to determine mode.");
                return 1;
            }

            var assembly = Assembly.GetExecutingAssembly();
            if (!DataAccessHelper.CheckSprocAccess(assembly, showPrompts))
                return 1;

            PL = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, assembly, DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, showPrompts);
            var dir = new DirectoryInfo(EnterpriseFileSystem.TempFolder);
            foreach(var file in dir.EnumerateFiles("SystemLetterGeneratorFed.*"))
                Repeater.TryRepeatedly(() => file.Delete()); //remove files left from previous crash if any
            int returnVal = new LetterGenerator(PL).Generate();
            PL.LogEnd();
            Console.WriteLine(returnVal);
            DataAccessHelper.CloseAllManagedConnections();

            return returnVal;
        }
    }
}
