using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;


namespace _Starter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Pheaa;
            ReflectionInterface ri = new ReflectionInterface();
           // Dialog.Info.Ok("Please log into the session then press the Insert key");
            //ri.PauseForInsert();
            new TPERM.TPERM(ri).Main();
        }
    }
}