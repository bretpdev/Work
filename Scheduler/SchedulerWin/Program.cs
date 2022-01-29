using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SchedulerWeb
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "SchedulerWeb"))
                return 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun plr = new ProcessLogRun("SchedulerWeb", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var lda = new LogDataAccess(DataAccessHelper.CurrentMode, plr.ProcessLogId, false, true);
            var da = new DataAccess(lda);

            var scheduler = new Scheduler(plr, da);
            scheduler.Process();

            plr.LogEnd();
            DataAccessHelper.CloseAllManagedConnections();

            return 0;
        }
    }
}
