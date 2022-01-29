using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace DMCSNTFYFE
{
    public class DMCSNTFYFE
    {
        public static string script = "DMCSNTFYFE";
        public static int Main(string[] args)
        {
            int returnValue = 0;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, script))
                return 1;
            ProcessLogRun logRun = new ProcessLogRun(script, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface RI = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, RI, script, "BatchCornerStone");
            DmcsApprovalNotify dmcs = new DmcsApprovalNotify(RI, helper, logRun, script);
            Console.WriteLine("Processing files.");
            returnValue = dmcs.Process();
            helper.Connection.Close();
            Console.WriteLine($"Login helper closed.");
            DataAccessHelper.CloseAllManagedConnections();
            Console.WriteLine($"ClosedManagedConnections.");
            RI.CloseSession();
            Console.WriteLine($"RI Close Session.");
            Console.WriteLine($"Exit program with returnValue = { returnValue }.");
            return returnValue;
        }
        
    }
}
