using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace FEDECORPRT
{
    class Program
    {
        public static readonly string ScriptId = "FEDECORPRT";
        public static ProcessLogRun PL { get; set; }
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid Args provided, the second argument must specify FILELOAD or PRINT");
                return 1;
            }
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            int returnVal = 0;

            if (args[1] == "PRINT")
            {
                PL = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
                var DA = new DataAccess(PL.ProcessLogId);
                int numberOfProcesses = 20;
                if (args.Length > 2)
                    numberOfProcesses = args[2].ToInt();

                returnVal = new BatchPrinting(numberOfProcesses, DA, PL).RunPrinting();
            }
            else if (args[1] == "FILELOAD")
            {
                PL = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
                var DA = new DataAccess(PL.ProcessLogId);
                returnVal = new FileLoader().RunFileLoader(DA);
            }
            else
            {
                Console.WriteLine("Invalid Args provided, the second argument was {0} but you must specify FILELOAD or PRINT", args[1]);
                return 1;
            }

            Console.WriteLine("Return Value:{0}", returnVal);
            PL.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
            return returnVal;
        }
    }
}