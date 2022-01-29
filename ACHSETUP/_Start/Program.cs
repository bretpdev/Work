using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Start
{
    class Program
    {
        public static readonly string ScriptId = "ACHSETUP";

        static void Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("ACHSETUP :: Version {0}:{1}:{2}:{3}", version.Major, version.Minor, version.Build, version.Revision));
            Console.WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
#if DEBUG
            ProcessLogRun logRun = new ProcessLogRun("ACHSETUP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            ReflectionInterface uri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, uri, "ACHSETUP", "BatchUheaa");

            new ACHSETUP.CompassAchSetup(uri).Main();

            uri.CloseSession();
            logRun.LogEnd();
#endif
        }
    }
}