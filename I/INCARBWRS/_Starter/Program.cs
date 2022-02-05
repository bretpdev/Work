using System;
using System.Reflection;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {

            if (!DataAccessHelper.StandardArgsCheck(args, "INCARBWRS", false))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ProcessLogRun logRun = new ProcessLogRun("INCARBWRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);

            ReflectionInterface RI = new ReflectionInterface();

            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, RI, "INCARBWRS", "BatchUheaa");

            INCARBWRS.IncarceratedBorrowers obj = new INCARBWRS.IncarceratedBorrowers(RI);
            obj.Main();

            RI.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
