using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
//using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.VisualStudio.TestTools.UITest.Extension;
//using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Uheaa.Common.DataAccess;
using BARCODEFED;

namespace BARCODEFED_Tests
{
    [TestClass]
    public class EmailDataTest
    {
        public ProcessLogRun LogRun { get; set; }
        public BARCODEFED.DataAccess DA { get; set; }

        public EmailDataTest()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            LogRun = new ProcessLogRun("BARCODEFED_Tests", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            DA = new BARCODEFED.DataAccess(LogRun);
        }
        
        [TestMethod]
        public void BorrowerExistsInFedRegion()
        {
            Assert.IsTrue(DA.CheckBorrowerRegion("3247732447"));
            Assert.IsTrue(DA.CheckBorrowerRegion("2463447795"));
            Assert.IsTrue(DA.CheckBorrowerRegion("3344587750"));
        }

        [TestMethod]
        public void BorrowerDoesNotExistInFedRegion()
        {
            Assert.IsFalse(DA.CheckBorrowerRegion("4972867810"));
            Assert.IsFalse(DA.CheckBorrowerRegion("3805563320"));
            Assert.IsFalse(DA.CheckBorrowerRegion("5591444542"));
        }

        [TestMethod]
        public void ResendMail_DoNotResend()
        {
            Assert.IsFalse(DA.LetterIsRequired("EASYPAY"));
            Assert.IsFalse(DA.LetterIsRequired("PMTMONFED"));
        }

        [TestMethod]
        public void ResendMail_DoResend()
        {
            Assert.IsTrue(DA.LetterIsRequired("US09B10P"));
        }

    }
}