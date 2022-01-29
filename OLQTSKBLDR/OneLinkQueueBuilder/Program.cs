using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace OLQBuilder
{
    class Program
    {
        public static readonly string ScriptId = "OLQTSKBLDR";
        public static int Main(string[] args)
        {
            if(!DataAccessHelper.StandardArgsCheck(args,ScriptId))
            {
                return 1;
            }

            if(!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
            {
                return 1;
            }

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode,false, true);
            BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa", true);

            OneLinkQueueBuilder qBuilder = new OneLinkQueueBuilder(ri,logRun);
            qBuilder.Process();

            ri.CloseSession();
            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
