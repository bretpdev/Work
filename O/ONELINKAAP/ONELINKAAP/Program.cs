using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ONELINKAAP
{
    static class Program
    {
        const string ScriptId = "ONELINKAAP";

        [STAThread]
        static int Main(string[] args)
        {
			if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return 1;

            if(!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ProcessLogRun logRunUheaa = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true, false);
            int uheaaReturnValue = 1;
            int sessionCount = args.Count() > 1 ? args[1].ToInt() : 15; //Sets how many sessions will be used with a default of 15
            List<Task> threads = new List<Task>();
            threads.Add(Task.Factory.StartNew(() => uheaaReturnValue = new ArcAddProcessing(logRunUheaa, DataAccessHelper.Region.Uheaa, ScriptId).ProcessArcs(sessionCount), TaskCreationOptions.LongRunning));

            Task.WhenAll(threads).Wait();
            Console.WriteLine("UHEAA Return Value: {0}", uheaaReturnValue);

            logRunUheaa.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return uheaaReturnValue; // if the return value is above 0 JAMS will know that an error occurred
        }
    }
}