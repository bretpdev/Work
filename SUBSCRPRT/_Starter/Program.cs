using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    public class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            if (!DataAccessHelper.StandardArgsCheck(args, "SUBSCRPRT") && !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            ProcessLogRun logRun = new ProcessLogRun("SUBSCRPRT", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            ri.LogRun = logRun;
            BatchProcessingLoginHelper.Login(logRun, ri, "SUBSCRPRT", "BatchUheaa");

            new SUBSCRPRT.SubScreenPrint(ri).Main();
            ri.CloseSession();
            logRun.LogEnd();
        }
    }
}