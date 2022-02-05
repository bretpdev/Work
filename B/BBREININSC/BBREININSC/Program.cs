using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace BBREININSC
{
    public class BBREININSC
    {
        public static int Main(string[] args)
        {

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "BBREININSC") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun("BBREININSC", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true, false);
            int returnValue = new BBReinstate(logRun, args).Run();
            DataAccessHelper.CloseAllManagedConnections();
            return returnValue;
        }

    }
}
