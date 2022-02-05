using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;


namespace FAFSADBLOD
{
    public class FAFSADBLOD
    {
        public static int Main(string[] args)
        {

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "FAFSADBLOD") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun("FAFSADBLOD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true, false);
            int returnValue = new IsirLoad(logRun, args).Run();
            DataAccessHelper.CloseAllManagedConnections();
            return returnValue;
        }

    }
}
