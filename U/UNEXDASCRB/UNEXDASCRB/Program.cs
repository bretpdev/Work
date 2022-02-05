using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UNEXDASCRB
{
    class Program
    {
        private const string ScriptId = "UNEXDASCRB";
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true))
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun run = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var result = new Scrubber(run).ScrubData();
            run.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return result;
        }
    }
}
