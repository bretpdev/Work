using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CMP30DYSKP
{
    /// <summary>
    /// Entry point of script.
    /// </summary>
    class Program
    {
        static readonly string ScriptId = "CMP30DYSKP";
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false)) //Verify first arg is the mode
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper loginHelper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            int result = new FileProcessor(ScriptId, ri, logRun).Run(); 

            if (ri != null)
                ri.CloseSession();
            if (loginHelper != null)
                BatchProcessingHelper.CloseConnection(loginHelper);
            logRun.LogEnd();

            return result;
        }
    }
}
