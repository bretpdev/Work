using ACHSETUPFD;
using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using System.Collections.Generic;
using System.Windows.Forms;
using ACHSETUPFD.DataClasses;

namespace _Start
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            EndorserRecord e1 = new EndorserRecord();
            e1.BF_SSN = "111111111";
            e1.DM_PRS_1 = "john";
            e1.DM_PRS_LST = "doe";

            EndorserRecord e2 = new EndorserRecord();
            e2.BF_SSN = "111111112";
            e2.DM_PRS_1 = "jane";
            e2.DM_PRS_LST = "doe";

            List<EndorserRecord> recs = new List<EndorserRecord> { e1, e2 };

            SelectEndorser selectEndorser = new SelectEndorser(ref recs);
            if (selectEndorser.ShowDialog() == DialogResult.Cancel)
            {
                recs = selectEndorser.recs;
            }*/

            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun("ACHSETUPFD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, "ACHSETUPFD", "BatchCornerstone");

            CompassAchSetupFed setup = new CompassAchSetupFed(ri);
            setup.Main();
            logRun.LogEnd();
            ri.CloseSession();

            /*
            recs = selectEndorser.recs;
            */
        }
    }
}