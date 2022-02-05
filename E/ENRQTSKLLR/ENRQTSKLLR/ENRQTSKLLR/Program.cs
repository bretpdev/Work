using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ENRQTSKLLR
{
    class Program
    {
        public static readonly string ScriptId = "ENRQTSKLLR";
        static int Main(string[] args)
        {
            Console.WriteLine($"Version:: {Assembly.GetExecutingAssembly().GetName().Version}");
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            ReflectionInterface ri = new ReflectionInterface();
            ri.LogRun = logRun;

            Console.WriteLine("Logging into session.");
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");

            bool result = new EnrollmentTaskKiller(ri, logRun).Process();
            Console.WriteLine("Script run complete. Closing PL.");
            logRun.LogEnd();

            DataAccessHelper.CloseAllManagedConnections();
            Console.WriteLine("Closing session.");
            ri.CloseSession();

            return result == true ? 0 : 1;
        }
    }
}
