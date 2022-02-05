using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;

namespace RCCALLHIST
{
    public class Program
    {
        public static readonly string ScriptId = "RCCALLHIST";


        public enum Status
        {
            Success = 0,
            Failure = 1
        }

        static int Main(string[] args)
        {
            Console.WriteLine($"Version:: {Assembly.GetExecutingAssembly().GetName().Version}");
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true))
                return (int)Status.Failure;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), DataAccessHelper.TestMode))
                return (int)Status.Failure;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, false);

            var result = new Processor(logRun).Process();

            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return (int)result;

        }
    }
}
