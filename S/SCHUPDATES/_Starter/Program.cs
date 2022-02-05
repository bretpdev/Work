using System;
using System.Reflection;
using Uheaa.Common;
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
            if (args[1].IsPopulated())
                DataAccessHelper.CurrentRegion = args[1].ToLower() == "uheaa" ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;

            if (!DataAccessHelper.StandardArgsCheck(args, "SCHUPDATES") && !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return;

            string scriptId = "SCHUPDATES";

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = null;
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
            {
                logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
                BatchProcessingLoginHelper.Login(logRun, ri, scriptId, "BatchUheaa");
                new SCHUPDATES.SchoolUpdatesUheaa(ri).Main();
            }
            else
            {
                logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode);
                BatchProcessingLoginHelper.Login(logRun, ri, scriptId, "BatchCornerstone");
                new SCHUPDATES.SchoolUpdatesFed(ri).Main();
            }

            ri.CloseSession();
        }
    }
}