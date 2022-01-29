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
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ReflectionInterface ri = new ReflectionInterface();
            ri.Login("", "");
            ri.PauseForInsert();
            Application.EnableVisualStyles();
            new I1I2CHREV.ClearingHouseReview(ri).Main();
            ri.CloseSession();
        }
    }
}
