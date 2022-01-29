using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UECORBORNO
{
    class Program
    {
        public static string ScriptId = "UECORBORNO";
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            Console.WriteLine("Starting application");
            bool showPrompts = (args.Any() && args.Length > 1);
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, showPrompts))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), showPrompts))
                return 1;

            ProcessLogRun plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
            int retVal = new Emailer(plr).Process();

            Console.WriteLine(retVal);

            plr.LogEnd();
            Console.WriteLine("Closing Database Connections.  Please wait.");

            return retVal;
        }
    }
}
