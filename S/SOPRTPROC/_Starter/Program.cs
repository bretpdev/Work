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

            var plr = new ProcessLogRun("SOPRTPROC", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var ri = new ReflectionInterface();
            var login = BatchProcessingLoginHelper.Login(plr, ri, "CLMLOGON", "BatchUHEAA");
            new SOPRTPROC.StateOffset(ri).Main();
            BatchProcessingHelper.CloseConnection(login);
            ri.CloseSession();
            plr.LogEnd();
        }
    }
}
