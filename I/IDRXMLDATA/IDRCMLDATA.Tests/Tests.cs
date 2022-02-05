using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uheaa.Common.DataAccess;
using IDRXMLDATA;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace IDRCMLDATA.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Test_NumberOfThreads1()
        {
            int xmlNodes = 3;
            int maxNumberOfThreads = 5;

            if (xmlNodes < maxNumberOfThreads)
                Assert.AreEqual(xmlNodes, new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads));
            else if (xmlNodes == maxNumberOfThreads)
                Assert.AreEqual(new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads), maxNumberOfThreads);
            else if (xmlNodes > maxNumberOfThreads)
                Assert.AreEqual(new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads), maxNumberOfThreads);
            else
                throw new Exception("Un-expected condition in Test_NumberOfThreads");
        }

        [TestMethod]
        public void Test_NumberOfThreads2()
        {
            int xmlNodes = 5;
            int maxNumberOfThreads = 5;

            if (xmlNodes < maxNumberOfThreads)
                Assert.AreEqual(xmlNodes, new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads));
            else if (xmlNodes == maxNumberOfThreads)
                Assert.AreEqual(new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads), maxNumberOfThreads);
            else if (xmlNodes > maxNumberOfThreads)
                Assert.AreEqual(new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads), maxNumberOfThreads);
            else
                throw new Exception("Un-expected condition in Test_NumberOfThreads");
        }

        [TestMethod]
        public void Test_NumberOfThreads3()
        {
            int xmlNodes = 6;
            int maxNumberOfThreads = 5;

            if (xmlNodes < maxNumberOfThreads)
                Assert.AreEqual(xmlNodes, new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads));
            else if (xmlNodes == maxNumberOfThreads)
                Assert.AreEqual(new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads), maxNumberOfThreads);
            else if (xmlNodes > maxNumberOfThreads)
                Assert.AreEqual(new IDRXMLDATA.IDRXMLDATA(DataAccessHelper.Mode.Dev).GetNumberOfThreads(xmlNodes, maxNumberOfThreads), maxNumberOfThreads);
            else
                throw new Exception("Un-expected condition in Test_NumberOfThreads");
        }

        [TestMethod]
        public void Test_FfelConsol()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccess da = new DataAccess(new ProcessLogRun("IDRXMLDATA", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode, false, true));
            var borrower = new AppBorrowerType();
            var loan = new UnderlyingLoansType() { AwardType = AwardTypeType.FFELConsolidation };

            borrower.RepaymentApplication = new RepaymentApplicationType[1];
            borrower.RepaymentApplication[0] = new RepaymentApplicationType() { ApplicationID = "Test" };
            string val = da.DetermineLoanType(borrower, loan, 1);
            Assert.IsTrue(val == "CNSLDN");
        }
    }
}
