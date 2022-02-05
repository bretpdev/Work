using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace SOALTRFED
{
    class Program
    {
        public static ProcessLogRun PLR { get; set; }
        private static readonly string ScriptId = "SOALTRFED";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            bool showPrompts = (args.Any() && args.Length == 1);
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPrompts))
            {
                Console.WriteLine("Unable to parse command line args to determine mode.");
                return 1;
            }

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), showPrompts))
                return 1;

            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, showPrompts);
            int returnVal = new StatementOfAccount(PLR).Process();
            PLR.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            Console.WriteLine(returnVal);
            return returnVal;
        }
    }
}
