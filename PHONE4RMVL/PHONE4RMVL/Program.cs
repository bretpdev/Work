using System;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace PHONE4RMVL
{
    class Program
    {
        private static string Script { get; } = "PHONE4RMVL";

        [STAThread]
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, Script, false))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(Script, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true, true);

            int returnValue = new RemovePhoneFour(logRun, Script).processPhones();

            Console.WriteLine($"Exit program with returnValue = {returnValue}.");

            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return returnValue;
        }
    }
}
