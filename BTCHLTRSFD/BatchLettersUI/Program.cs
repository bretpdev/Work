using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace BatchLettersUI
{
    static class Program
    {
        /// <summary>
        /// Args: {datamode}
        /// Ex: live cornerstone = console in live mode cornerstone region
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "Batch Letters Fed UI"))
                return;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogger.RegisterApplication("BTCHLTRSFD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
            string error = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            if (!error.IsNullOrEmpty())
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (BatchLettersUI UI = new BatchLettersUI())
            {
                UI.ShowDialog();
            }
        }
    }
}
