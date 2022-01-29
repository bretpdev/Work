using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PRIDRCRP
{
    class Program
    {
        public static string ScriptId { get; set; } = "PRIDRCRP";

        public enum FileSelectionMethod
        {
            OpenFileDialog,
            DirectoryWithUnZip,
            None
        }

        [STAThread]
        static void Main(string[] args)
        {
            FileSelectionMethod fsMethod = FileSelectionMethod.None;
            int directoryNumber = 1;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false) || args.Length < 1)
            {
                return;
            }

            if(args.Length > 1)
            {
                if(args[1].ToUpper().Contains("OPENFILEDIALOG"))
                {
                    fsMethod = FileSelectionMethod.OpenFileDialog;
                }
                else if (args[1].Contains("DIRECTORYWITHUNZIP"))
                {
                    fsMethod = FileSelectionMethod.DirectoryWithUnZip;
                }
                else
                {
                    Dialog.Info.Ok("Please provide a method of execution as a parameter. |OpenFileDialog, DirectoryWithUnZip|");
                    return;
                }
            }
            else
            {
                Dialog.Info.Ok("Please provide a method of execution as a parameter. |OpenFileDialog, DirectoryWithUnZip|");
                return;
            }

            if(args.Length > 2)
            {
                directoryNumber = args[2].ToInt();
                if(directoryNumber > 5 || directoryNumber < 1)
                {
                    Dialog.Info.Ok("The directory number argument must be a number between 1 and 5");
                    return;
                }
            }

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine(string.Format("PRIDRCRP :: Version {0}:{1}:{2}:{3}", version.Major, version.Minor, version.Build, version.Revision));
            Console.Write($"Executing on Directory Number {directoryNumber.ToString()}");
            Console.WriteLine("");

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            Processor processor = new Processor(logRun, fsMethod, directoryNumber);
            processor.Process();
            
            DataAccessHelper.CloseAllManagedConnections();
            logRun.LogEnd();
        }
    }
}
