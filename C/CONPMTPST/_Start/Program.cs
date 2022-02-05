using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Start
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //UNDONE: hard coded mode
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ReflectionInterface ri = new ReflectionInterface();
            ri.Login("", "");
            ri.PauseForInsert();
            Application.EnableVisualStyles();
            new CONPMTPST.ConsolPaymentPosting(ri).Main();
            ri.CloseSession();
        }
    }
}