using System;
using System.Linq;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace COMPCB
{
    class Program
    {
        static readonly string ScriptId = "COMPCB";
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false)) //Verify first arg is the mode
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true, true);
            Console.WriteLine($"Established PL # {logRun.ProcessLogId} for this run.");
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper loginHelper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            if (!string.IsNullOrWhiteSpace(loginHelper.UserName))
                Console.WriteLine($"Logged into the session with Batch ID: {loginHelper.UserName}");

            bool result = new TaskProcessor(ScriptId, ri, logRun).Run();

            if (ri != null)
                ri.CloseSession();
            if (loginHelper != null) 
                BatchProcessingHelper.CloseConnection(loginHelper);
            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();

            return result == true ? 0 : 1;
        }
    }
}
