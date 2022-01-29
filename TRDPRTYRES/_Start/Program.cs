using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace _Starter
{
    public class Program
    {
        [STAThread()]
        public static void Main(string[] args)
        {
            const string ScriptId = "TRDPRTYRES";

            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return;

            //Print the script version stored in assembly info
            WriteLine($"TRDPRTYRES :: Version {Assembly.GetExecutingAssembly().GetName().Version}");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            ri.LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingLoginHelper.Login(ri.LogRun, ri, ScriptId, "BatchUheaa");
            new TRDPRTYRES.ThirdPartyAuthorization(ri).Main();
            ri.CloseSession();
        }
    }
}