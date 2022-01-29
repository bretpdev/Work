using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("WHOAMI", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ri.LogRun = logRun;
            BatchProcessingLoginHelper.Login(logRun, ri, "WHOAMI", "BatchUheaa");

            new WHOAMI.BorrowerSearch(ri).Main();
            logRun.LogEnd();
            ri.CloseSession();
        }
    }
}