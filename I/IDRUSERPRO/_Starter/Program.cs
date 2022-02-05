using IDRUSERPRO;
using System;
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

            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            Application.EnableVisualStyles();
            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("IDRUSERPRO", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, "IDRUSERPRO", "BatchUheaa");
            new IDRUSERPRO.IDRProcessingUheaa(ri).Main();
            ri.CloseSession();
        }
    }
}