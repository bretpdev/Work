using LENDERLTRS;
using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    public class Program
    {

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "LENDERLTRS") && !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ProcessLogRun logRun = new ProcessLogRun("LENDERLTRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface RI = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(logRun, RI, "LENDERLTRS", "BatchUheaa");
            RI.LogRun = logRun;

            new LenderLetters(RI).Main();

            RI.CloseSession();

            return 0;
        }
    }
}