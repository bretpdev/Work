using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace NSLDSCONSO
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (DataAccessHelper.StandardArgsCheck(args, "NSLDSCONSO"))
            {
                if (DatabaseAccessHelper.StandardSprocAccessCheck(Assembly.GetExecutingAssembly()))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    var plr = new ProcessLogRun("NSLDSCONSO", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
                    Application.Run(new MainForm(plr));
                    plr.LogEnd();
                }
            }
        }
    }
}
