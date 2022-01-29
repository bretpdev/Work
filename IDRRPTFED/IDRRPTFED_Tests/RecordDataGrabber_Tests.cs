using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDRRPTFED;
using System.Linq;

namespace IDRRPTFED_Tests
{
    [TestClass]
    public class RecordDataGrabber_Tests
    {
        Random r = new Random();
        [TestMethod]
        public void AbRecordWithNullAwardId_IsListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var appId = r.Next();
            var hasNullId = new AbRecordData()
            {
                AwardId = null,
                ApplicationId = appId
            };
            grabber.AbData.Add(hasNullId);
            Assert.AreEqual(appId, grabber.NullAwardIds.Single());
        }
        [TestMethod]
        public void AbRecordWithPopulatedAwardId_IsNotListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var hasId = new AbRecordData()
            {
                AwardId = "POPULATED",
                ApplicationId = 1
            };
            grabber.AbData.Add(hasId);
            Assert.AreEqual(0, grabber.NullAwardIds.Count());
        }

        [TestMethod]
        public void BdRecordWithNullAwardId_IsListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var appId = r.Next();
            var hasNullId = new BdRecordData()
            {
                AwardId = null,
                ApplicationId = appId
            };
            grabber.BdData.Add(hasNullId);
            Assert.AreEqual(appId, grabber.NullAwardIds.Single());
        }
        [TestMethod]
        public void BdRecordWithPopulatedAwardId_IsNotListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var hasId = new BdRecordData()
            {
                AwardId = "POPULATED",
                ApplicationId = 1
            };
            grabber.BdData.Add(hasId);
            Assert.AreEqual(0, grabber.NullAwardIds.Count());
        }

        [TestMethod]
        public void BeRecordWithNullAwardId_IsListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var appId = r.Next();
            var hasNullId = new BeRecordData()
            {
                AwardId = null,
                ApplicationId = appId
            };
            grabber.BeData.Add(hasNullId);
            Assert.AreEqual(appId, grabber.NullAwardIds.Single());
        }
        [TestMethod]
        public void BeRecordWithPopulatedAwardId_IsNotListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var hasId = new BeRecordData()
            {
                AwardId = "POPULATED",
                ApplicationId = 1
            };
            grabber.BeData.Add(hasId);
            Assert.AreEqual(0, grabber.NullAwardIds.Count());
        }

        [TestMethod]
        public void BfRecordWithNullAwardId_IsListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var appId = r.Next();
            var hasNullId = new BfRecordData()
            {
                AwardId = null,
                ApplicationId = appId
            };
            grabber.BfData.Add(hasNullId);
            Assert.AreEqual(appId, grabber.NullAwardIds.Single());
        }
        [TestMethod]
        public void BfRecordWithPopulatedAwardId_IsNotListedInNullAwardIds()
        {
            var grabber = new RecordDataGrabber();
            var hasId = new BfRecordData()
            {
                AwardId = "POPULATED",
                ApplicationId = 1
            };
            grabber.BfData.Add(hasId);
            Assert.AreEqual(0, grabber.NullAwardIds.Count());
        }
    }
}
