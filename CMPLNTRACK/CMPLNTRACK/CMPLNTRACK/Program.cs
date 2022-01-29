using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace CMPLNTRACK
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var parseResults = KvpArgValidator.ValidateArguments<Args>(args);
            var parsedArgs = new Args(args);
            if (parseResults.IsValid)
            {
                if (parsedArgs.Mode == "live")
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
                else if (parsedArgs.Mode.IsIn("dev","test"))
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            }
            else if (args.Count() == 1) //launched from ACDC
            {
                DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)Enum.Parse(typeof(DataAccessHelper.Mode), args[0], true);
            }
            else
            {
                Dialog.Warning.Ok(parseResults.ValidationMesssage);
                return;
            }

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;
            Application.Run(new MainForm(parsedArgs));
        }
    }
}