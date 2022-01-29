using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SCRAINTUPD
{
    public class Program
    {
        public static int Main(string[] args)
        {
            string scriptId = "SCRAINTUPD";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            Console.WriteLine($"SCRA Interest Update from DOD :: Version {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            int process = new ScraProcess(LogRun, DataAccessHelper.CurrentRegion).Process();
            Console.WriteLine($"return value:{process}");
            Console.WriteLine("Script Complete, logging end of process logger.");
            
            Thread.Sleep(1500);
            return process;
        }
    }
}