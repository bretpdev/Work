using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ImagingTransferFileBuilder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            if (DataAccessHelper.StandardArgsCheck(args) && DatabaseAccessHelper.StandardSprocAccessCheck(Assembly.GetExecutingAssembly()))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                //save settings after exit
                Properties.Settings.Default.Save();
            }
        }
    }
}
