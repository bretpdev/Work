using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace EBILL
{
    class Program
    {
        public static string ScriptId = "EBILL";
        public static ProcessLogData LogData { get; set; }

        [STAThread]
        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), DataAccessHelper.TestMode))
                return 1;

            LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), false);

            int returnValue = new RequestUpdater().Run();
            Console.WriteLine("Programs return value:{0}", returnValue);
            ProcessLogger.LogEnd(LogData.ProcessLogId);
            return returnValue;
        }
    }
}
