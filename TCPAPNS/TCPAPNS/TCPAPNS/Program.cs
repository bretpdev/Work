using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace TCPAPNS
{
    class Program
    {
        public static readonly string ScriptId = "TCPAPNS";

        static int Main(string[] args)
        {
            WriteLine($"{ScriptId} :: Version {Assembly.GetExecutingAssembly().GetName().Version}");
            WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var results = KvpArgValidator.ValidateArguments<Args>(args);

            if (results.IsValid)
            {
                Args appArgs = new Args(args);
                DataAccessHelper.CurrentMode = appArgs.Mode.ToLower() == "live" ? DataAccessHelper.Mode.Live : DataAccessHelper.Mode.Dev;
                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                    return 1;
                ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
                if (appArgs.Fileload)
                    new TCPAFileLoad(logRun, appArgs.Onelink).LoadFiles();
                else
                    new TCPAProcessing(logRun, appArgs.Onelink).Process();
                logRun.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
                return 0;
            }
            return 1; //Processing did not happen
        }
    }
}