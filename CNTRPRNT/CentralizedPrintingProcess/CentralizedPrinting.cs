using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace CentralizedPrintingProcess
{
    public class CentralizedPrinting
    {
        public const string ScriptId = "CNTRPRNT";

        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            string msg = string.Empty;
            if (!DataAccessHelper.StandardArgsCheck(args, "Centralized Printing"))
                msg += "Operation mode was not passed through default arguments.  Please set the default mode in the command line arguments before running this script";
            msg += DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            if (msg != string.Empty)
            {
                Dialog.Error.Ok(msg);
                return 1;
            }
            if (args.Any(o => o == "console"))
            {
                RunConsole();
            }
            else
            {
                Application.EnableVisualStyles();
                var status = new StatusForm(new MiscDat(ScriptId));
                Application.Run(status);
            }
            return 0;
        }

        private static void RunConsole()
        {
            var util = new ComUtilities(new MiscDat(ScriptId),
                new ConsoleJobStatus("Printing"),
                new ConsoleJobStatus("Faxing")
            );

            util.Printer.Process();
            util.Fax.Process();
            util.LogRun.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();
        }
    }
}