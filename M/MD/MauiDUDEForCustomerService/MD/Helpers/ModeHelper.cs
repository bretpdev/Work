using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MD
{
    public static class ModeHelper
    {
        private static int? currentProcessLogId;

        private static ProcessLogRun cornestoneProcessLogRun;
        private static ProcessLogRun uheaaProcessLogRun;



        public static void TestMode()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ModeSet();
            Hlpr.RH.SendMessage("CMD:TESTMODE");
        }
        public static void LiveMode()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            ModeSet();
            Hlpr.RH.SendMessage("CMD:LIVEMODE");
        }
        private static void ModeSet()
        {
            if (currentProcessLogId == null)
            { 
                var pld = ProcessLogger.RegisterApplication("MD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
                currentProcessLogId = pld.ProcessLogId;
            }

        }

        public static int ProcessLogId
        {
            get
            {
                ModeSet();
                return currentProcessLogId.Value;
            }
        }

        public static ProcessLogRun GetProcessLogRun(DataAccessHelper.Region region)
        {
            if(region == DataAccessHelper.Region.CornerStone)
            {
                if (cornestoneProcessLogRun == null)
                {
                    cornestoneProcessLogRun = new ProcessLogRun("MD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, false, true);
                }
                return cornestoneProcessLogRun;
            }
            else
            {
                if (uheaaProcessLogRun == null)
                {
                    uheaaProcessLogRun = new ProcessLogRun("MD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
                }
                return uheaaProcessLogRun;
            }
        }


    }
}
