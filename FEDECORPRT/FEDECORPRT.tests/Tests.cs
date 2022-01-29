using System;
using System.Linq;
using Xunit;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace FEDECORPRT.tests
{
    public class Tests
    {

        [Fact]
        public void InternalNameTest()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun PL = new ProcessLogRun("FEDCORPRT.test", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            var DA = new DataAccess(PL.ProcessLogId);

            BatchPrinting bp = new BatchPrinting(1, DA, PL);
            Dictionary<string, string> internalNames = bp.GetInternalHeaderNamesHelper();
            Assert.True(internalNames != null && internalNames.Count != 0);
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
            string letterData = "0000000000,Joe,K,Average,987654321,1,2,ct1,st1,54321,fs1,fc1,Joe Average";
            string modifiedLetterData = bp.DoLetterDataReplace(letterData, cb, sd, internalNames);

            List<string> strs = modifiedLetterData.SplitAndRemoveQuotes(",");
            Assert.True(strs[0] == "0000000000");
            Assert.True(strs[1] == cb.FirstName);
            Assert.True(strs[2] == cb.MiddleName);
            Assert.True(strs[3] == cb.LastName);
            Assert.True(strs[4] != "987654321");
            Assert.True(strs[5] == cb.Address1);
            Assert.True(strs[6] == cb.Address2);
            Assert.True(strs[7] == cb.City);
            Assert.True(strs[8] == cb.State);
            Assert.True(strs[9] == cb.Zip);
            Assert.True(strs[10] == cb.ForeignState);
            Assert.True(strs[11] == cb.ForeignCountry);
            Assert.True(strs[12] == cb.FirstName + " " + cb.LastName);
        }
    }
}
