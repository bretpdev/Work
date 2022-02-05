using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Start
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;

            string scriptId = "CFUTDTQTSK";
            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false);
            
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, scriptId, "BatchUheaa");

            new CFUTDTQTSK.CreateFutureDatedQueueTasks(ri).Main();
            ri.LogOut();
            ri.CloseSession();
        }
    }
}