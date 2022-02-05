using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BTCHPWUPD
{
    class Program
    {


        public static string ScriptId = "BTCHPWUPD";
        public static ProcessLogRun PLR { get; set; }
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId))
                return 1;

            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;

            PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            DataAccess da = new DataAccess(PLR.ProcessLogId);
            if (args.Skip(1).Any())
            {
                using (BaseWordAndFormat b = new BaseWordAndFormat(da))
                {
                    b.ShowDialog();
                    return 0;
                }
            }
            else
                return new PasswordUpdater(da).Process();
        }
    }
}
