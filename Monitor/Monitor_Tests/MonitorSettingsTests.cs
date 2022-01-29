using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor;

namespace Monitor_Tests
{
    [TestClass]
    public class MonitorSettingsTests
    {
        [TestMethod]
        public void ForceAddedCountMatches()
        {
            var ms = new MonitorSettings();
            ms.ForceAdded("111111111");
            Assert.AreEqual(ms.TotalForceCounter, 1);
            ms.ForceAdded("222222222");
            Assert.AreEqual(ms.TotalForceCounter, 2);
        }

        [TestMethod]
        public void PrenoteAddedCountMatches()
        {
            var ms = new MonitorSettings();
            ms.PrenoteAdded("111111111");
            Assert.AreEqual(ms.TotalPreNoteCounter, 1);
            ms.PrenoteAdded("222222222");
            Assert.AreEqual(ms.TotalPreNoteCounter, 2);
        }

        [TestMethod]
        public void ForcePrenoteNoInterference()
        {
            var ms = new MonitorSettings();
            ms.ForceAdded("111111111");
            ms.PrenoteAdded("222222222");
            ms.ForceAdded("222222222");
            Assert.AreEqual(ms.TotalForceCounter, 2);
            Assert.AreEqual(ms.TotalPreNoteCounter, 1);
        }
    }
}
