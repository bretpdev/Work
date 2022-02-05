using System;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace LetterTrackingDataTransfer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.None;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process p = new Process();
            p.Start();
        }
    }
}