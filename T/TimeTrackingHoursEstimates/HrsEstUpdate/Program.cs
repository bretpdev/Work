using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace HrsEstUpdate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Hrs h = new Hrs())
            {
                h.ShowDialog();
            }
        }
    }
}
