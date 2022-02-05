using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ELSYSDDARC
{
    public static class Program
    {
        const string ScriptId = "ELSYSDDARC";

        [STAThread]
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "ELSYSDDARC") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1; //Not all the arguments were supplied

            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DataAccess da = new DataAccess(new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, true), logRun);
            return new EliminateARCs(ScriptId, logRun, da).Run();
        }
    }
}