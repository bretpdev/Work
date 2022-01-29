using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIACTCMTS;

namespace DIACTCMTS_Tests
{
    [TestClass]
    public class InboundDataTest
    {
        InboundData testData;
        public InboundDataTest()
        {
            testData = new InboundData()
            {
                NobleRowId = 7,
                CallType = 1,
                AccountIdentifier = "1234567890",
                AreaCode = "801",
                PhoneNumber = "8018698631",
                CallCampaign = "CORX",
                DispositionCode = "AM",
                AdditionalDispositionCode = "92",
                AgentId = "1209",
                ActivityDate = DateTime.Now,
                EffectiveTime = "",
                ListId = "8004",
                VoxFileId = "",
                IsInbound = true,
                CallLength = 0
            };
        }

        [TestMethod]
        //[InlineData("134567890")] This will fail
        public void CleanseDataAccountNumberTrim()
        {
            //control
            testData.AccountIdentifier = "1234567890";
            testData.CleanseData();
            Assert.AreEqual("1234567890", testData.AccountIdentifier);

            //hanging space before
            testData.AccountIdentifier = "  1234567890";
            testData.CleanseData();
            Assert.AreEqual("1234567890", testData.AccountIdentifier);

            //hanging space after
            testData.AccountIdentifier = "1234567890  ";
            testData.CleanseData();
            Assert.AreEqual("1234567890", testData.AccountIdentifier);

            //internal space
            testData.AccountIdentifier = "12345  67890";
            testData.CleanseData();
            Assert.AreEqual("1234567890", testData.AccountIdentifier);
        }

        [TestMethod]
        public void CleanseDataAccountNumberNull()
        {
            testData.AccountIdentifier = null;
            testData.CleanseData();
            Assert.IsNull(testData.AccountIdentifier);
        }
    }
}
