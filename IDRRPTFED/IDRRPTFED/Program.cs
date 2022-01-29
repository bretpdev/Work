using System;
using Uheaa.Common;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Collections.Generic;
using System.Linq;

namespace IDRRPTFED
{
    static class Program
    {
        public static string ScriptId = "IDRRPTFED";

        public static bool Legacy { get; set; }
        public static ProcessLogData LogData { get; set; }
        public static Arguments ApplicationArgs { get; set; }
        public const int SUCCESS = 0;
        public const int FAILURE = 1;

        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                Dialog.Error.Ok("MISSING ARGS!\nArgs should be:\n mode\n legacy (optional)\n showprompts (optional)\n runtype (optional)\n file (optional).");
                return FAILURE;
            }

            List<string> applicationIds = new List<string>();
            if (args[0].ToLower() == "mode:dev" && args[2].ToLower() == "runtype:byapp")
            {
                applicationIds.AddRange(args.Skip(3));
                string[] arguments = new string[3];
                Array.Copy(args, 0, arguments, 0, 3);
                args = arguments;
            }

            var results = KvpArgValidator.ValidateArguments<Arguments>(args);
            if (results.IsValid)
            {
                ApplicationArgs = new Arguments(args);
                ApplicationArgs.SetDataAccessValues(args);

                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), Arguments.ShowPrompts))
                    return FAILURE;

                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
                LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), Arguments.ShowPrompts);
                var lda = new LogDataAccess(DataAccessHelper.CurrentMode, LogData.ProcessLogId, false, false);
                DataAccess da = new DataAccess(Legacy, lda);

                int scriptRunVal;

                if (Arguments.RunType == "bydate")
                    scriptRunVal = new IdrNsldsReportGeneration(da).Run(); //Regular date selection run
                else if (Arguments.RunType == "byapp")
                    scriptRunVal = new IdrNsldsReportGeneration(da).Run(applicationIds);  //Run with passed in app IDs in command line args (for BA testing)
                else
                    scriptRunVal = new IdrNsldsReportGeneration(da).Run(Arguments.File);  //Run with passed in file that contains app IDs separated by line breaks

                ProcessLogger.LogEnd(LogData.ProcessLogId);
                Dialog.Info.Ok($"Processing complete for {ScriptId}");
                return scriptRunVal; //0 = Success; 1 = Failure
            }
            else
            {
                Console.WriteLine(results.ValidationMesssage);
                Dialog.Error.Ok(results.ValidationMesssage);
                return FAILURE;
            }
        }
    }
}
