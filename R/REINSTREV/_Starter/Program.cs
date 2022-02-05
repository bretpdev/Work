using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            var plr = new ProcessLogRun("REINSTREV", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), Uheaa.Common.DataAccess.DataAccessHelper.Region.Uheaa, Uheaa.Common.DataAccess.DataAccessHelper.Mode.Dev);
            var ri = new ReflectionInterface();
            var login = BatchProcessingLoginHelper.Login(plr, ri, "REINSTREV", "BatchUheaa");

            new REINSTREV.ReinstatementReview(ri).Main();

            BatchProcessingHelper.CloseConnection(login);
            ri.CloseSession();
            plr.LogEnd();
        }
    }
}
