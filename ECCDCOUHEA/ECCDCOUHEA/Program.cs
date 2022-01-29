using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECCDCOUHEA
{
    class Program
    {
        public static readonly string scriptId = "ECCDCOUHEA";
        [STAThread]
        public static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false) || args.Length < 1)
            {
                return 1;
            }

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("ECCDCOUHEA :: Version {0}:{1}:{2}:{3}", version.Major, version.Minor, version.Build, version.Revision));
            Console.WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            EccdcouheaProcess process = new EccdcouheaProcess(LogRun, EnterpriseFileSystem.TempFolder);
            process.Process();
            LogRun.LogEnd();
            return 0; 
        }
    }
}
