using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.IO;
using System.Text;

namespace SCHRPT_Batch
{
    class Program
    {
        public static int Main(string[] args)
        {
            int numberOfSchoolsPerEmail = 200;
            const string scriptId = "SCHRPT_Batch";
            if (!DataAccessHelper.StandardArgsCheck(args, scriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false) || args.Length < 1)
            {
                return 1;
            }

            if(args.Length > 1)
            {
                int tempNumSchools;
                bool success = int.TryParse(args[1], out tempNumSchools);
                if(success)
                {
                    numberOfSchoolsPerEmail = tempNumSchools;
                }
                else
                {
                    Console.WriteLine("Unable to parse second parameter as int, defaulting schools per email to 200.");
                }
            }

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("SCHRPT Batch Report :: Version {0}:{1}:{2}:{3}", version.Major, version.Minor, version.Build, version.Revision));
            Console.WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            //TestEmailHelper.EmailHelper.SendMail(DataAccessHelper.TestMode, /*DO NOT TOUCH recipient*/"dhschrpt@gmail.com"/*DO NOT TOUCH*/, "donotreply@mycornerstoneloan.org", "[SECURE] TEST MAIL", "TESTBODY", "", @"Q:\Support Services\Jacob\BigTestDoc.txt", TestEmailHelper.EmailHelper.EmailImportance.Normal, true);
            int process = new SchrptProcess(LogRun, DataAccessHelper.CurrentRegion, numberOfSchoolsPerEmail).Process(); //PATH: @"c:\SCHRPT_Batch\schrpt_report.csv"
            Console.WriteLine("Script Complete, logging end of process logger.");

            Thread.Sleep(1500);
            return process;
        }
    }
}
