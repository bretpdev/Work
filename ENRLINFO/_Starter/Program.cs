using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        public static readonly string ScriptId = "ENRLINFO";

        static void Main(string[] args)
        {
            //Check that first arg is DEV or LIVE
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                return;
            }

            //Check that the user running the script has access to execute the SQL
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
            {
                return;
            }

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = null;
            //Create Session In Test
#if DEBUG
            ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? "BatchUheaa" : "BatchCornerstone");
#endif
            new ENRLINFO.EnrollmentInformationProcessing(ri).Main();
        }
    }
}
