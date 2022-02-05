using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IMGEMAILAR
{
    static class Program
    {
        const string ScriptId = "IMGEMAILAR";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (DataAccessHelper.StandardArgsCheck(args, ScriptId))
            {
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

                ProcessLogRun plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(),
                    DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

                Application.Run(new ImagingForm(plr, ScriptId));

                plr.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
            }
        }
    }
}
