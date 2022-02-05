using System;
using FSAMTHCALL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FSAMTHCALL_Tests
{
    [TestClass]
    public class NobleDataTests
    {
        [TestMethod]
        public void NoTimeTest()
        {
            var data = new NobleData() { CallLength = 0 };
            Assert.AreEqual("00:00:00", data.CalculateTime());
        }
        [TestMethod]
        public void SecondsTest()
        {
            var data = new NobleData() { CallLength = 30 };
            Assert.AreEqual("00:00:30", data.CalculateTime());
        }
        [TestMethod]
        public void MinutesTest()
        {
            var data = new NobleData() { CallLength = 60 * 5 + 15 };
            Assert.AreEqual("00:05:15", data.CalculateTime());
        }

        [TestMethod]
        public void HoursTest()
        {
            var data = new NobleData() { CallLength = 60 * 60 * 5 + 120 + 17 };
            Assert.AreEqual("05:02:17", data.CalculateTime());
        }

        [TestMethod]
        public void LargeHoursTest()
        {
            var data = new NobleData() { CallLength = 60 * 60 * 120 };
            Assert.AreEqual("120:00:00", data.CalculateTime());
        }
    }
}