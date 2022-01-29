using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace AACQRYTILP
{
    class Program
    {
        public static readonly string ScriptId = "AACQRYTILP";

        public static int Main(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return 1;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return 1;
            }

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false);

            Extractor processor = new Extractor(logRun);
            processor.Process();
            logRun.LogEnd();

            return 0;
        }

    }
}
