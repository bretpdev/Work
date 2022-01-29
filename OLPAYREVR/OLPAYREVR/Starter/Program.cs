using OLPAYREVR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace Starter
{
    class Program
    {
        public static readonly string ScriptId = "OLPAYREVR";
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            ReflectionInterface ri = new ReflectionInterface();
            ri.LogRun = logRun;

            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && args.Any() && args.Contains("login")) // Purely a way to bypass manually logging in while testing script
            {
                BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            }
            else
            {
                Dialog.Info.Ok("Please log in and then press the \"Insert key\".");
                ri.PauseForInsert(); // Waiting for user to login
            }

            new OneLinkPaymentReversal(ri).Main();
            ri.CloseSession();
        }
    }
}
