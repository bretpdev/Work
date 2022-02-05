using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Start
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            var ri = new ReflectionInterface();
            ri.Login(args[0], args[1]);
            var script = new CRTREFQTSK.CreateReferenceQueueTask(ri);
            script.Main();
        }
    }
}
