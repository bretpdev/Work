using System;
using System.Reflection;
using System.Threading;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;

            string scriptId = "REPAYTERM";

            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            BatchProcessingLoginHelper.Login(LogRun, ri, scriptId, "BatchUheaa");

            new REPAYTERM.RepayTerm(ri);

            ri.CloseSession();
            LogRun.LogEnd();
        }
    }
}