using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace Starter
{
    class Program
    {
        public static readonly string ScriptId = "PRECONADJ";

        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            ReflectionInterface ri = new ReflectionInterface();
            ri.LogRun = logRun;
            BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            new PRECONADJ.PRECONADJ(ri).Main();
            ri.CloseSession();
        }
    }
}