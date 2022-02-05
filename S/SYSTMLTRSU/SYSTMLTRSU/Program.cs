using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace SYSTMLTRSU
{
    
    class Program
    {
        public static readonly string ScriptId = "SYSTMLTRSU";
        public static ProcessLogRun PL { get; set; }
        public static int NumberOfWinwords { get; set; }
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            bool showPrompts = (args.Any() && args.Length == 1);
            NumberOfWinwords = args.Skip(2).Any() ? args[2].ToInt() : 20;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPrompts))
            {
                Console.WriteLine("Unable to parse command line args to determine mode.");
                return 1;
            }

            var assembly = Assembly.GetExecutingAssembly();
            if (!DataAccessHelper.CheckSprocAccess(assembly, showPrompts))
                return 1;

            PL = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, assembly, DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, showPrompts);
            int returnVal = new LetterGenerator(PL).Generate();
            PL.LogEnd();
            Console.WriteLine(returnVal);
            DataAccessHelper.CloseAllManagedConnections();

            return returnVal;
        }
    }
}
