using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ri.Login(args[0], args[1]);
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            new LS008.LS008(ri).Main();
            

        }
    }
}
