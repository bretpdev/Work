using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace LNDERLETTR
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Version v = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("Lender Letter Batch  ::  Version - {0}", string.Format("{0}:{1}:{2}", v.Major, v.Minor, v.Revision));
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            
            if (!DataAccessHelper.StandardArgsCheck(args, "LNDERLETTR"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun("LNDERLETTR", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            ProcessQueue procQ = new ProcessQueue(logRun);
            
            int printerSet = procQ.initPrinter(logRun, args[0]); 
            int processCount = 0;
            if(printerSet == 0)
                 processCount = procQ.Process();
            else
                Console.WriteLine($"Printer not set or parameters not correct..");
            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections(); 

            return processCount; 
        }
    }
}