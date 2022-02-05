using System;
using System.Linq;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace RTNEMLINVF
{
    public class Program
    {
        public const string ScriptId = "RTNEMLINVF";

        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false) && !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);


            int returnValue = new ReturnEmailInvalidation(logRun, ScriptId).Process();

            logRun.LogEnd();
            return returnValue;
        }
    }
}