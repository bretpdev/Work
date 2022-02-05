using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DOCIDFED
{
    public class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            string scriptId = "DOCIDFED";

            if (!DataAccessHelper.StandardArgsCheck(args, scriptId) && !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live && InvalidStartingLocation())
                return;

            ProcessLogRun logRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true);

            DataAccess da = new DataAccess(logRun);

            Selection sel = new Selection(logRun, da);
            sel.ShowDialog();

            logRun.LogEnd();
        }

        /// <summary>
        /// Checks the starting location and makes sure the user does not start from a network drive
        /// </summary>
        private static bool InvalidStartingLocation()
        {
            var location = Assembly.GetEntryAssembly().Location;
            if (location.StartsWith("cs1") || location.StartsWith("Y:") || location.StartsWith("Z:"))
            {
                Dialog.Info.Ok(@"You are attempting to start this from a network drive. Please copy this down to your local machine and start it from there.");
                return true;
            }
            return false;
        }
    }
}