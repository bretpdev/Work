using DEMUPDTFED;
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
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            var ri = new ReflectionInterface();
            ri.Login("", "", DataAccessHelper.Region.CornerStone);
            new DemographicUpdate(ri).Main();
        }
    }
}
