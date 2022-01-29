using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, "PAYOFFFED"))
                return 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("PYOFFLTRFD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ri.LogRun = logRun;
            BatchProcessingLoginHelper.Login(logRun, ri, "PYOFFLTRFD", "BatchCornerstone", true);

            new PYOFFLTRFD.PayOffLetterFed(ri).Main();

            ri.CloseSession();

            return 0;
        }
    }
}
