using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace EA80Reconciliation
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
			if (DataAccessHelper.StandardArgsCheck(args, "EA80TAXREC") && DatabaseAccessHelper.StandardSprocAccessCheck(Assembly.GetExecutingAssembly()))
            {
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
				ProcessLogData LogData = ProcessLogger.RegisterApplication("EA80TAXREC", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm(LogData));
                //save settings after exit
                Properties.Settings.Default.Save();
				ProcessLogger.LogEnd(LogData.ProcessLogId);
            }
        }
    }
}
