using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;


namespace LENDERUPDT
{
    public class Program
    {
        public static int Main(string[] args)
        {
            int returnValue = 0;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "LENDERUPDT"))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun("LENDERUPDT", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            LENDERUPDT lndrupdt = new LENDERUPDT(logRun);
            returnValue = lndrupdt.Process();

            DataAccessHelper.CloseAllManagedConnections();
            return returnValue;
        }
    }
}
