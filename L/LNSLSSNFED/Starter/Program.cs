using System;
using System.Reflection;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.Scripts;



namespace LNSLSSNFED
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            if (!DataAccessHelper.StandardArgsCheck(args, "LNSLSSNFED"))
                return;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            ProcessLogRun logRun = new ProcessLogRun("LNSLSSNFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            Uheaa.Common.Scripts.ReflectionInterface reflection = new Uheaa.Common.Scripts.ReflectionInterface();

            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, reflection, "LNSLSSNFED", "BatchCornerStone");
            
            ProcessLoans proc = new ProcessLoans(reflection);

            proc.Main();
        }
    }
}
