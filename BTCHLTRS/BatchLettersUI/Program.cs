using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BatchLettersUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!DataAccessHelper.StandardArgsCheck(args, "BatchLettersUI"))
                return;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ProcessLogger.RegisterExceptionOnly("BTCHLTRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            Application.Run(new BatchLettersUI());
        }
    }
}
