using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace SOPRMCRD
{
    public class Program
    {
        public static string ScriptId = "SOPRMCRD";
        public static int ERROR = 1;
        public static int SUCCESS = 0;

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return ERROR;

            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            ri.LogRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            BatchProcessingLoginHelper.Login(ri.LogRun, ri, ScriptId, "BatchUheaa");
            if (ri.UserId.IsNullOrEmpty())
            {
                WriteLine("Unable to get a UT ID from the database. Script will now end");
                return ERROR;
            }
            else
                WriteLine($"Logging into session with UT ID: {ri.UserId}.");

            int result = new StateOffsetUpdate(ri).Process();

            ri.CloseSession();
            ri.LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();

            return result;
        }
    }
}