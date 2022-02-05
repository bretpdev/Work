using ACHSETUP;
using System;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACH_Test
{
    [TestClass]
    public class ACHTest
    {
        [TestMethod]
        public void CheckSessionRegion_ShouldBeUheaaRegion_ReturnTrue()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            Uheaa.Common.Scripts.ReflectionInterface ri = new Uheaa.Common.Scripts.ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("ACHSETUP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, "ACHSETUP", "BatchUheaa");
            CompassAchSetup setup = new CompassAchSetup(ri);

            Assert.IsTrue(setup.CheckRegion());

            ri.CloseSession();
        }

        [TestMethod]
        public void CheckSessionRegion_ShouldBeUheaaRegion_ReturnFalse()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            Uheaa.Common.Scripts.ReflectionInterface ri = new Uheaa.Common.Scripts.ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("ACHSETUP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, "ACHSETUP", "BatchUheaa");
            CompassAchSetup setup = new CompassAchSetup(ri);

            Assert.IsFalse(setup.CheckRegion());

            ri.CloseSession();
        }
    }
}