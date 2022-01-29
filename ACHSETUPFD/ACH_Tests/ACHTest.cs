using ACHSETUPFD;
using System;
using System.Reflection;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Xunit;
using ACHSETUPFD.DataClasses;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ACH_Tests
{
    public class ACHTest
    {
        [Fact]
        public void CheckSessionRegion_ShouldBeCornerstoneRegion_ReturnTrue()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("ACHSETUPFD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, "ACHSETUPFD", "BatchCornerStone");
            CompassAchSetupFed setup = new CompassAchSetupFed(ri);

            Assert.True(setup.CheckRegion());

            ri.CloseSession();
        }

        [Fact]
        public void TestGetEndorsers()
        {
            const string scriptId = "ACHSETUPFED.tests";
            string AccountNumber = "9817509120"; //Use an account # with endorsers from test region
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccess DA = new DataAccess(LogRun);
            var ret = DA.GetEndorsers(DataAccessHelper.ExecuteSingle<string>("dbo.spGetSSNFromAcctNumber", DataAccessHelper.Database.Cdw, SqlParams.Single("AccountNumber", AccountNumber)));
            Debug.Assert(ret != null && ret.Count > 0);
        }

        [Fact]
        public void TestInfoDialogEndorserValidation()
        {
            //Shortcut to addinfo dialogue
            const string scriptId = "ACHSETUPFED.tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccess DA = new DataAccess(LogRun);
            ACHRecord _achRecToAdd = new ACHRecord();
            _achRecToAdd.IsEndorser = ACHRecord.EndorserStatus.NA;
            AddInfoDialog aid = new AddInfoDialog(_achRecToAdd, new List<EndorserRecord> { new EndorserRecord() }, DA);
            if (aid.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Debug.Assert(_achRecToAdd.IsEndorser == ACHRecord.EndorserStatus.Yes || _achRecToAdd.IsEndorser == ACHRecord.EndorserStatus.No);
        }

        [Fact]
        public void TestSelectEndorserValidation()
        {
            //Shortcut to selectendorser dialogue
            const string scriptId = "ACHSETUPFED.tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccess DA = new DataAccess(LogRun);
            List<EndorserRecord> ers = new List<EndorserRecord> { new EndorserRecord { DM_PRS_1 = "John", DM_PRS_LST = "Doe"}, new EndorserRecord { DM_PRS_1 = "John", DM_PRS_LST = "Foo" } };
            SelectEndorser aid = new SelectEndorser(ref ers);
            if (aid.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Debug.Assert(aid.recs.Count == 1);
        }

        [Fact]
        public void TestSelectApprovalLetter()
        {
            //Set up system borrower demographics with distinct data
            SystemBorrowerDemographics demos = new SystemBorrowerDemographics();
            demos.AccountNumber = "1111111111";
            demos.Address1 = "Adr1";
            demos.Address2 = "Adr2";
            demos.City = "ct";
            demos.Country = "ctry";
            demos.FirstName = "fn";
            demos.LastName = "ln";
            demos.Ssn = "222222222";
            demos.State = "st";
            demos.ZipCode = "zp";

            //create letterData from code
            string letterData = LetterDataFormatter.GenerateApprovedLetterData(demos, "bfn", "apbd", "aa", "333333333", "acc_par");
            //create hard coded expected result
            string expectedLetterData = Uheaa.Common.DocumentProcessing.DocumentProcessing.ACSKeyLine("333333333", Uheaa.Common.DocumentProcessing.DocumentProcessing.LetterRecipient.Borrower, Uheaa.Common.DocumentProcessing.DocumentProcessing.ACSKeyLineAddressType.Legal)
                + ",acc_par,fn ln,Adr1,Adr2,ct,st,zp,ctry,bfn,apbd,aa";

            //assert code and example return the same result for thest data
            Assert.True(letterData.CompareTo(expectedLetterData) == 0);
        }

        [Fact]
        public void TestSelectDenialLetter()
        {
            //Set up system borrower demographics with distinct data
            SystemBorrowerDemographics demos = new SystemBorrowerDemographics();
            demos.AccountNumber = "1111111111";
            demos.Address1 = "Adr1";
            demos.Address2 = "Adr2";
            demos.City = "ct";
            demos.Country = "ctry";
            demos.FirstName = "fn";
            demos.LastName = "ln";
            demos.Ssn = "222222222";
            demos.State = "st";
            demos.ZipCode = "zp";

            List<string> denials = new List<string>() { "den1", "den2", "den3", "den4", "den5", };
            //create letterData from code
            string letterData = LetterDataFormatter.GenerateDeniedLetterData(demos, denials);
            //create hard coded expected result
            string expectedLetterData = Uheaa.Common.DocumentProcessing.DocumentProcessing.ACSKeyLine("222222222", Uheaa.Common.DocumentProcessing.DocumentProcessing.LetterRecipient.Borrower, Uheaa.Common.DocumentProcessing.DocumentProcessing.ACSKeyLineAddressType.Legal)
                + ",1111111111,fn ln,Adr1,Adr2,ct,st,zp,ctry,\"- den1\",\"- den2\",\"- den3\",\"- den4\",\"- den5\"";

            //assert code and example return the same result for thest data
            Assert.True(letterData.CompareTo(expectedLetterData) == 0);
        }

    }

}