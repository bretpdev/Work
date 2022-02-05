using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SOALETTERU
{
    class Program
    {
        public static readonly string ScriptId = "SOALETTERU";
        public static ProcessLogRun PL { get; set; }
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            bool showPrompts = (args.Any() && args.Length == 1);
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPrompts))
            {
                Console.WriteLine("Unable to parse command line args to determine mode.");
                return 1;
            }

            var assembly = Assembly.GetExecutingAssembly();
            if (!DataAccessHelper.CheckSprocAccess(assembly, showPrompts))
                return 1;

            PL = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, assembly, DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false);

            int returnVal = new StatementOfAccount(PL).Process();

            PL.LogEnd();

            Console.WriteLine("Return Value:{0}", returnVal);
            Console.WriteLine("Closing Database Connection. (This may take a minute)");
            DataAccessHelper.CloseAllManagedConnections();

            return returnVal;
        }
    }
}
