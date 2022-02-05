using CSLSLTRFED;
using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Start
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            if (!DataAccessHelper.StandardArgsCheck(args, "CSLSLTRFED") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            ProcessLogData logData = ProcessLogger.RegisterScript("CSLSLTRFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            ProcessLogRun logRun = new ProcessLogRun(logData.ProcessLogId, "CSLSLTRFED", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false);
            ReflectionInterface ri = new ReflectionInterface();
            ri.ProcessLogData = logData;
            BatchProcessingLoginHelper.Login(logRun, ri, "CSLSLTRFED", "BatchCornerstone", true);

            new CornerstoneLoanServicingLetters(ri).Main();
            logRun.LogEnd();
            ri.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();
        }
    }
}