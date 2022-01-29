using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using MANMAIL;

namespace MANMAIL_Tests
{
    [TestClass]
    public class EmailDataTest
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }

        public EmailDataTest()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            LogRun = new ProcessLogRun("MANMAIL_Tests", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            DA = new DataAccess(LogRun);
        }

        [TestMethod]
        public void IsCaliforniaCompassBorrower()
        {
            //Assert.IsTrue(DA.GetCAEmailData("8663145539", true).AccountNumber.IsPopulated());
            //Assert.IsTrue(DA.GetCAEmailData("7766859234", true).AccountNumber.IsPopulated());
            //Assert.IsTrue(DA.GetCAEmailData("1020892133", true).AccountNumber.IsPopulated());
        }

        [TestMethod]
        public void IsNotCaliforniaCompassBorrower()
        {
            //Assert.IsNull(DA.GetCAEmailData("0000286130", true));
            //Assert.IsNull(DA.GetCAEmailData("5067799107", true));
            //Assert.IsNull(DA.GetCAEmailData("7072636727", true));
        }

        [TestMethod]
        public void IsCaliforniaOnelinkBorrower()
        {
            //Assert.IsTrue(DA.GetCAEmailData("0699136630", false).AccountNumber.IsPopulated());
            //Assert.IsTrue(DA.GetCAEmailData("1627952882", false).AccountNumber.IsPopulated());
            //Assert.IsTrue(DA.GetCAEmailData("2158668285", false).AccountNumber.IsPopulated());
        }

        [TestMethod]
        public void IsNotCaliforniaOnelinkBorrower()
        {
            //Assert.IsNull(DA.GetCAEmailData("2225391062", false));
            //Assert.IsNull(DA.GetCAEmailData("7894586083", false));
            //Assert.IsNull(DA.GetCAEmailData("5149108886", false));
        }
    }
}