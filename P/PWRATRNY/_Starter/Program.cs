using PWRATRNY;
using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            const string ScriptId = "PWRATRNY";

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return;

            Console.WriteLine($"PWRATRNY :: Version {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine("");

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            ri.LogRun = logRun;
            new PowerOfAttorney(ri).Main();
            ri.CloseSession();
        }
    }
}