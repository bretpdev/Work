using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ACTDUTRPT
{
    public class Program
    {
        public static int Main(string[] args)
        {
            string scriptId = "ACTDUTRPT";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("Active Duty Reporting Update from DOD :: Version {0}:{1}:{2}:{3}", version.Major, version.Minor, version.Build, version.Revision));
            Console.WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            int process = new ActiveDutyReportProcess(LogRun).Process();
            Console.WriteLine("Return value:{0}", process);
            Console.WriteLine("Script Complete, logging end of process logger.");
            
            return process;
        }
    }
}