using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace QSTATSEXTR
{
    class Program
    {
        const int SUCCESS = 0;
        const int ERROR = 1;
        const string ScriptId = "QSTATSEXTR";
        static int Main(string[] args)
        {
            if (!KvpArgValidator.ValidateArguments<Args>(args).IsValid)
                return ERROR;
            var arguments = new Args(args);
            DataAccessHelper.CurrentMode = arguments.Mode;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DateTime? runTimeDate = arguments.RunDate?.ToDateNullable();
            if (arguments.RunDate?.ToLower() == "mostrecent")
                runTimeDate = new DataAccess(plr).GetMostRecentRunTimeDate();
            var generator = new ReportGenerator(ScriptId, plr, runTimeDate);
            if (!generator.Generate(runTimeDate.HasValue))
                return ERROR;
            plr.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
#if DEBUG
            Console.ReadKey();
#endif
            return SUCCESS;
        }
    }
}
