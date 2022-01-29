using BARCODEFED;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            string scriptId = "BARCODEFED";
            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper login = BatchProcessingLoginHelper.Login(logRun, ri, scriptId, "BatchCornerstone");
            
            new BARCODEFED.BarcodeScanner(ri).Main();
            ri.CloseSession();
        }
    }
}