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
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            var ri = new ReflectionInterface();
            ri.Login(args[0], args[1]);
            new EMAILBTCF.EmailBatchScript(ri).Main();
        }
    }
}
