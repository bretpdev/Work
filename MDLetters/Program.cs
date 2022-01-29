using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MDLetters
{
    static class Program
    {
        public static readonly string ScriptId = "MDLetters";
        public static ProcessLogData LogData { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// args[0] mode as string
        /// args[1] region as string
        /// args[2] account number (not required)
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ValidateApp(args))
                return;

            LogData = ProcessLogger.RegisterApplication(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            using (LetterSelection letter = new LetterSelection(args.Length == 3 ? args[2] : string.Empty))
            {
                letter.ShowDialog();
            }
        }

        private static bool ValidateApp(string[] args)
        {
            if (args.Length < 2)
            {
                Dialog.Error.Ok("You need to specify a region before running this application.");
                return false;
            }

            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return false;

            if (args[1].ToUpper() == "CORNERSTONE")
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            else if (args[1].ToUpper() == "UHEAA")
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            else
            {
                Dialog.Error.Ok(string.Format("Unknown region {0}, please contact Systems Support", args[1]));
                return false;
            }

            return true;
        }
    }
}
