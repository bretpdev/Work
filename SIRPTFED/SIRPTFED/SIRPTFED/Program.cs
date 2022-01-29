using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SIRPTFED
{
    static class Program
    {
        private static string ScriptId = "SIRPTFED";
        public static ProcessLogRun PLR { get; set; }
        public static DataAccess DA { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.First().IsNumeric())//this is to handle new ACDC and old ACDC
            {
                int mode = args.First().ToInt();
                string argsMode = mode == 0 ? "live" : "dev";
                args = new string[1] {argsMode};
            }
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return;

            if(!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

#if DEBUG
            DA = new DataAccess(PLR, "dmoon");
#else
            DA = new DataAccess(PLR, Environment.UserName);
#endif

            using (MetricSelection ms = new MetricSelection(DA))
            {
                ms.ShowDialog();
            }
        }
    }
}
