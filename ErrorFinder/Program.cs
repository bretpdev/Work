using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ErrorFinder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #if !DEBUG
            Application.ThreadException += (sender, ex) =>
            {
                MessageBox.Show("Unhandled Exception: " + ex.Exception.ToString());
            };
            #endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoadForm());
        }
    }
}
