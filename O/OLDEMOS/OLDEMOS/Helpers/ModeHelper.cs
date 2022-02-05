using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace OLDEMOS
{
    public static class ModeHelper
    {
        public static ProcessLogRun LogRun { get; set; }

        public static void TestMode()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            LogRun = new ProcessLogRun("OLDEMOS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            Helper.DA = new DataAccess();
        }

        public static void LiveMode()
        {
            //TODO: Uncomment for live
            //DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            //LogRun = new ProcessLogRun("OLDEMOS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true);
            //Helper.DA = new DataAccess();
        }
    }
}