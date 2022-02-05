using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace CLSCHLLNFD
{
    public class Program
    {
        public static string ScriptId { get; set; } = "CLSCHLLNFD";

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true);
            bool consoleWait = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && args.Length > 1 && args[1].ToLower() == "usewait";
            bool createFileForTest = DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && args.Length > 1 && args[1].ToLower() == "createfile";

            if (createFileForTest)
            {
                Console.WriteLine("About to create a file.  Once completed, cross your fingers and check your C-drive for the 502_TestFiles folder.");
                return new TestHelper().CreateTestFile(args, ri, logRun);
            }

            BatchProcessingHelper login = BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchCornerstone", true);
           
            int returnCode = new ClosedSchoolLoan(ri, logRun).Run(consoleWait);
            logRun.LogEnd();
            ri.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();
            return returnCode;
        }
    }
}