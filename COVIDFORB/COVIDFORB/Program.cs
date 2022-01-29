using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace COVIDFORB
{
    class Program
    {
        public static readonly string ScriptId = "COVIDFORB";

        public static int SessionCount = 1;

        static int Main(string[] args)
        {
            if(!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return 1;
            }

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return 1;
            }
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            //Session count arg
            if(args.Length > 1)
            {
                SessionCount = args[1].ToInt();
            }

            ProcessLogRun logRun = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            //Add new forbearances
            new ForbearanceGenerator(logRun).AddForbearances();
            //Process all forbearances
            new ForbearanceProcessor(logRun).Process();
            //Add aggregate arcs
            new ArcProcessor(logRun).Process();
            logRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
