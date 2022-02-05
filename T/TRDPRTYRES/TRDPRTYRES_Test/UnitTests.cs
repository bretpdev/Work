using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace TRDPRTYRES_Test
{
    [TestClass]
    public class UnitTests
    {
        public TRDPRTYRES.DataAccess DA { get; set; }
        public UnitTests ()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun("TRDPRTYTST", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new TRDPRTYRES.DataAccess(logRun.LDA);
            logRun.LogEnd();
        }

        [TestMethod]
        public void HasOpenLoans()
        {
            Assert.IsTrue(DA.HasOpenLoans("3592804074"));
        }

        [TestMethod]
        public void NoOpenLoans()
        {
            Assert.IsFalse(DA.HasOpenLoans("9042726863"));
        }

        [TestMethod]
        public void HasOnelinkReference()
        {
            Assert.IsTrue(DA.GetOnelinkReferences("3473822556").Count > 0);
        }

        [TestMethod]
        public void HasCompassReference()
        {
            Assert.IsTrue(DA.GetCompassReferences("9042726863").Count > 0);
        }

        [TestMethod]
        public void HasOnelinkSources()
        {
            Assert.IsTrue(DA.GetSources(true).Count > 0);
        }

        [TestMethod]
        public void HasCompassSources()
        {
            Assert.IsTrue(DA.GetSources(false).Count > 0);
        }

        [TestMethod]
        public void HasOnelinkRelationships()
        {
            Assert.IsTrue(DA.GetRelationships(true).Count > 0);
        }

        [TestMethod]
        public void HasCompassRelationships()
        {
            Assert.IsTrue(DA.GetRelationships(false).Count > 0);
        }
    }
}
