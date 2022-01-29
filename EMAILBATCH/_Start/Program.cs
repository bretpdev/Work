using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Start
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var ri = new ReflectionInterface();
            var script = new EMAILBATCH.EMAILBATCH(ri);
            script.Main();
        }
    }
}
