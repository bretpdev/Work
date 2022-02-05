using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UHECORPRT.tests
{
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// Pull back every Arc with ScriptDataId = 3 Test data has valid Activity Type and Activity Contact
        /// for an entry with that ScriptDataId
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            bool nonNullFound = false;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            var PL = new ProcessLogRun(Program.ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var DA = new DataAccess(PL);
            List<ArcInformation> recs = ArcInformation.Populate(DA, 3);
            foreach(ArcInformation rec in recs)
            {
                if(rec.ActivityContact != null && rec.ActivityType != null)
                {
                    nonNullFound = true;
                    break;
                }
            }
            Debug.Assert(nonNullFound);
        }

        /// <summary>
        /// Test that the information repalcement for coborrower letters functions as intended
        /// </summary>
        [TestMethod]
        public void InternalNameTest()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun PL = new ProcessLogRun("FEDCORPRT.test", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var DA = new DataAccess(PL);

            BatchPrinting bp = new BatchPrinting(1, DA);
            Dictionary<string, string> internalNames = bp.GetInternalHeaderNamesHelper();
            Assert.IsTrue(internalNames != null && internalNames.Count != 0);
            CoBorrowerInformation cb = new CoBorrowerInformation();
            ScriptData sd = new ScriptData();
            cb.AccountNumber = "11111111111";
            cb.Address1 = "adr1";
            cb.Address2 = "adr2";
            cb.City = "ct";
            cb.CoBorrowerSSN = "123456789";
            cb.Email = "email";
            cb.FirstName = "John";
            cb.ForeignCountry = "fc";
            cb.ForeignState = "fs";
            cb.LastName = "Doe";
            cb.MiddleName = "T";
            cb.OnEcorr = "Y";
            cb.State = "ST";
            cb.ValidAddress = "Y";
            cb.ValidEmail = "Y";
            cb.Zip = "12345";
            sd.FileHeader = "DF_SPE_ACC_ID,FIRST NAME,MIDDLEINITIAL,DM_PRS_LST,ACSKeyLine,ADDRESS1,STREET 2,DM_CT,State,ZIP,Foreign State,Country,NAME";
            sd.FileHeaderConst = sd.FileHeader;
            string letterData = "0000000000,Joe,K,Average,987654321,1,2,ct1,st1,54321,fs1,fc1,Joe Average";
            string modifiedLetterData = bp.DoLetterDataReplace(letterData, cb, sd, internalNames);

            List<string> strs = modifiedLetterData.SplitAndRemoveQuotes(",");
            Assert.IsTrue(strs[0] == "0000000000");
            Assert.IsTrue(strs[1] == cb.FirstName);
            Assert.IsTrue(strs[2] == cb.MiddleName);
            Assert.IsTrue(strs[3] == cb.LastName);
            Assert.IsTrue(strs[4] != "987654321");
            Assert.IsTrue(strs[5] == cb.Address1);
            Assert.IsTrue(strs[6] == cb.Address2);
            Assert.IsTrue(strs[7] == cb.City);
            Assert.IsTrue(strs[8] == cb.State);
            Assert.IsTrue(strs[9] == cb.Zip);
            Assert.IsTrue(strs[10] == cb.ForeignState);
            Assert.IsTrue(strs[11] == cb.ForeignCountry);
            Assert.IsTrue(strs[12] == cb.FirstName + " " + cb.LastName);
        }
    }
}
