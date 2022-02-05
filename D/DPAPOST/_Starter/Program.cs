using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        private static string ScriptId { get; set; } = "DPAPOST";

        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return;

            ReflectionInterface ri = new ReflectionInterface();
            ri.LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingLoginHelper.Login(ri.LogRun, ri, ScriptId, "BatchUheaa");

            new DPAPOST.DPAPosting(ri).Main();

            ri.CloseSession();
        }
    }
}