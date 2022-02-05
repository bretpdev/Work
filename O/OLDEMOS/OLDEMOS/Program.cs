using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace OLDEMOS
{
    static class Program
    {
        public const string ScriptId = "OLDEMOS";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, true))
                return;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetEntryAssembly(), true))
                return;

            //Create the session then launch the login form
            Helper.Instantiate();
            using var login = Helper.UI.CreateAndShow<LoginForm>();
            Application.Run();
        }
    }
}