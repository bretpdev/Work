using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CNCRTRNSFR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, "CNCRTRNSFR") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            ProcessLogRun logRun = new ProcessLogRun("CNCRTRNSFR", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            new CancerTransferFile(logRun).Run();
            logRun.LogEnd();
#if !DEBUG
            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev)
                Console.ReadKey();
#endif
        }
    }
}