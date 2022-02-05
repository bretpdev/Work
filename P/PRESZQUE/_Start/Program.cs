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
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ReflectionInterface ri = new ReflectionInterface();
            ri.Login(args[0], args[1]);
            new PRESZQUE.PRESZQUE(ri).Main();

        }
    }
}
