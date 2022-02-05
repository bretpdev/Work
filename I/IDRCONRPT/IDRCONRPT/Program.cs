using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace IDRCONRPT
{
    static class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (DataAccessHelper.StandardArgsCheck(args, Processor.ScriptId))
            {
                var plr = new ProcessLogRun(Processor.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
                new Processor(plr).Process();
                plr.LogEnd();
            }
        }
    }
}
