using PMTLNHIST;
using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string scriptId = "PMTLNHIST";

            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ri.LogRun = logRun;
            BatchProcessingLoginHelper.Login(logRun, ri, scriptId, "BatchUheaa");

            new LoanPaymentHistory(ri).Main();

            logRun.LogEnd();
            ri.CloseSession();
        }
    }
}