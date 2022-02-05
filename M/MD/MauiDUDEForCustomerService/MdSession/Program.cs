using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MdSession
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SessionForm(oleGuid: args[0], ordinal: args[1], region: args[2], mode: args[3]));

            //DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            //DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            //var data = ProcessLogger.RegisterApplication("MDSession", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());

            //if (args.Length == 0) return;
            //try
            //{
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new SessionForm(oleGuid: args[0], ordinal: args[1]));

            //}
            //catch (Exception e)
            //{
            //    ProcessLogger.AddNotification(data.ProcessLogId, "Session Exception", NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), e);
            //}
            //ProcessLogger.LogEnd(data.ProcessLogId);

        }
    }
}
