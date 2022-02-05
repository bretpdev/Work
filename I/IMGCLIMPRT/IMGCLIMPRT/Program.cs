using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace IMGCLIMPRT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string id = "IMGCLIMPRT";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (DataAccessHelper.StandardArgsCheck(args, id))
            {
                string ui = (args.Skip(1).FirstOrDefault() ?? "").ToLower();
                var x = new Locations().ImageLocation;
                if (ui == "console")
                {
                    var coord = new Coordinator(id, new Locations(), new Log(id, null));
                    coord.LoadPendingZips();
                    coord.BeginProcessing(threaded: false);
                }
                else if (ui == "gui")
                {
                    var form = new MainForm(id);
                    Application.Run(form);
                }
                else
                {
                    string message = "Please start this application with a second argument of gui or console";
                    Console.WriteLine(message);
                    MessageBox.Show(message);
                    return;
                }
            }
        }

    }
}
