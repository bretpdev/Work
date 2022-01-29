using System;
using System.Reflection;
using TILPCROST;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    public class Program
    {
        private const string ScriptId = "TILPCROST";

        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            ri.LogRun = logRun;

            new TilpCreditPost(ri).Main();
            ri.CloseSession();
            logRun.LogEnd();
        }
    }
}