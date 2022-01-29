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
            if (!DataAccessHelper.StandardArgsCheck(args, "PAYOFF"))
                return;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("PAYOFF", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ri.LogRun = logRun;
            BatchProcessingLoginHelper.Login(logRun, ri, "PAYOFF", "BatchUheaa", true);

            new PAYOFF.PayoffProcessor(ri).Main();

            ri.CloseSession();

            return;

        }
    }
}
