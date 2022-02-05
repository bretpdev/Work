using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NTISFCFED
{
    class Program
    {
        public static string ScriptId = "NTISFCFED";
        public static ProcessLogData LogData { get; set; }

        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (CheckArgsStartPL(args) == 1)
                return 1;

            int returnCode = 0; // Initiate to a success code
            var plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, true, true);
            DataAccess da = new DataAccess(plr);

            if (args[1].ToUpper() == "UPLOAD")//Becuase we are checking the args in CheckArgsStartPL, it's safe to use the else here
               returnCode = new UploadProcessor(plr, da).StartProcess();
            else
                returnCode = new DownloadProcessor(plr, da).StartProcess();

            plr.LogEnd();
            
            return returnCode;
        }

        private static int CheckArgsStartPL(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("The application was not started with the correct command line args.");
                return 1;
            }

            if (!args[1].ToUpper().IsIn("UPLOAD", "DOWNLOAD"))
            {
                Console.WriteLine("The second command line arg must be UPLOAD or DOWNLOAD.");
                return 1;
            }

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            return 0;
        }
    }
}
