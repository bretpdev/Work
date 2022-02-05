using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsBankImport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string mode = (args.FirstOrDefault() ?? "").ToLower();
            bool testMode;
            if (mode == "live")
                testMode = false;
            else if (mode == "test")
                testMode = true;
            else
            {
                MessageBox.Show("Please start this application with a first argument of test or live.");
                return;
            }
            string ui = (args.Skip(1).FirstOrDefault() ?? "").ToLower();
            if (ui == "console")
            {
                var coord = new Coordinator(new Locations(testMode), new Log(testMode, null));
                coord.LoadPendingZips();
                coord.BeginProcessing(threaded: false);
            }
            else if (ui == "gui")
            {
                var form = new MainForm();
                form.SetTestMode(testMode);
                Application.Run(form);
            }
            else
            {
                MessageBox.Show("Please start this application with a second argument of gui or console");
                return;
            }
        }
    }
}
