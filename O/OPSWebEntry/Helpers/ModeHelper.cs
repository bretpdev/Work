using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace OPSWebEntry
{
    public static class ModeHelper
    {
        static ModeHelper()
        {
            var exePath = Assembly.GetExecutingAssembly().Location.ToLower();
            if (exePath.EndsWith(".livetest.exe"))
            {
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
                IsLiveTest = true;
            }
            if (exePath.EndsWith(".test.exe"))
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            else if (exePath.EndsWith(".qa.exe"))
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.QA;
            else if (exePath.EndsWith(".dev.exe"))
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            else
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
        }

        public static bool IsTest
        {
            get
            {
                return DataAccessHelper.TestMode;
            }
        }

        public static bool IsLiveTest { get; set; }
    }
}
