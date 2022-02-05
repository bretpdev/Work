using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EMAILBTCF;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace EMAILBTCF_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckExtraFields_ReturnsFalse()
        {
            EmailCampaign campaign = new EmailCampaign() { EmailCampaignId = 0, SasFile = "test", LetterId = "test", SendingAddress = "test", SubjectLine = "test", Arc = "TEST", CommentText = "test", WorkLastLoaded = DateTime.Now };
            string email = "";
            Assert.IsFalse(new EmailBatchScript().CheckExtraFields(campaign, email));
        }

        [TestMethod]
        public void CheckExtraFields_ReturnsTrue()
        {
            EmailCampaign campaign = new EmailCampaign() { EmailCampaignId = 0, SasFile = "test", LetterId = "test", SendingAddress = "test", SubjectLine = "test", Arc = "TEST", CommentText = "test", WorkLastLoaded = DateTime.Now };
            string email = "[[[MissedMergeField]]]";
            Assert.IsTrue(new EmailBatchScript().CheckExtraFields(campaign, email));
        }

        [TestMethod]
        public void CheckFile_BlankReturnsFalse()
        {
            string file = "T:\\CheckFile_BlankReturnsFalse.txt";
            File.Create(file).Dispose();

            string filePattern = "*BlankReturnsFalse.txt";
            Assert.IsFalse(new EmailBatchScript().CheckFile(file, filePattern));
            File.Delete(file);
        }

        [TestMethod]
        public void CheckFile_PopulatedReturnsTrue()
        {
            string file = "T:\\CheckFile_PopulatedReturnsTrue.txt";
            string filePattern = "*PopulatedReturnsTrue";
            File.WriteAllLines(file, new List<string> { "data is here!" });
            Assert.IsTrue(new EmailBatchScript().CheckFile(file, filePattern));
            File.Delete(file);
        }
    }
}

