using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace EcorrLetterSetup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "Ecorr Letter Setup"))
                return;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProcessLogger.RegisterExceptionOnly("EcorrLtr", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            Application.Run(new LetterSearch());
        }
    }
}
