using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ClosureTests
{
    [TestClass]
    public class AccessTest
    {
        public CCCLOSURES.DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public List<CCCLOSURES.Regions> Regions { get; set; }

        public AccessTest()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun("ClosureTests", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            DA = new CCCLOSURES.DataAccess(LogRun);
        }

        [TestMethod]
        public void HasAccess()
        {
            Regions = DA.PopulateRegions("kjorgensen");
            Assert.IsTrue("UHEAA LGP PRE".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsTrue("UHEAA LGP DFT".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Regions = new List<CCCLOSURES.Regions>();

            Regions = DA.PopulateRegions("ccole");
            Assert.IsTrue("CornerStone".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsTrue("UHEAA LPP".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsTrue("UHEAA LGP PRE".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsTrue("UHEAA LGP DFT".IsIn(Regions.Select(p => p.RegionName).ToArray()));
        }

        [TestMethod]
        public void DoesNotHaveAccess()
        {
            Regions = DA.PopulateRegions("kjorgensen");
            Assert.IsFalse("CornerStone".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsFalse("UHEAA LPP".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Regions = new List<CCCLOSURES.Regions>();

            Regions = DA.PopulateRegions("bpehrson");
            Assert.IsFalse("CornerStone".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsFalse("UHEAA LPP".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsFalse("UHEAA LGP PRE".IsIn(Regions.Select(p => p.RegionName).ToArray()));
            Assert.IsFalse("UHEAA LGP DFT".IsIn(Regions.Select(p => p.RegionName).ToArray()));
        }
    }
}