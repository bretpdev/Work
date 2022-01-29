using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace Starter
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            string scriptId = "MANMAIL";
            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper login = BatchProcessingLoginHelper.Login(logRun, ri, scriptId, "BatchUheaa");

            Application.EnableVisualStyles();
            new MANMAIL.ManualReturnMail(ri).Main();
            ri.CloseSession();
        }
    }
}