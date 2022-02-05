using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PRETNFRNOT
{
    public static class Program
    {
        public static ProcessLogRun PL { get; set; }
        public static string ScriptId = "PRETNFRNOT";
        public static string letterToProcess { get; set; }
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId) || !DataAccessHelper.CheckSprocAccess(assembly, false))
                return 1;

            PL = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, assembly, DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);

            if (args.Length > 1)
                letterToProcess = args[1];

            new Process(ScriptId, PL).Run(letterToProcess);
            Console.WriteLine("Processing complete");
            Thread.Sleep(5000);
            PL.LogEnd();
            return 0;
        }
    }
}