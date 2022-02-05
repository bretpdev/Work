using MassAssignBatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MSSASGNDFT_Test
{
    [TestClass]
    public class UnitTests
    {
        public MassAssignBatchProcess Process { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public UnitTests()
        {
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                LogRun = new ProcessLogRun("MSSASGNDFT", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
                DataAccess da = new DataAccess(LogRun);
                Process = new MassAssignBatchProcess(LogRun, new List<MassAssignRangeAssignment.SqlUser>(), da);
        }

        [TestMethod]
        public void OpenedSession()
        {
            Assert.IsTrue(Process.RI != null);
            LogRun.LogEnd();
            Process.RI.CloseSession();
        }

        [TestMethod]
        public void Userexists()
        {
            Assert.IsTrue(Process.CheckUserInSession("UT00451"));
            LogRun.LogEnd();
            Process.RI.CloseSession();
        }

        [TestMethod]
        public void UserDoesNotExist()
        {
            Assert.IsFalse(Process.CheckUserInSession("UT00000"));
            LogRun.LogEnd();
            Process.RI.CloseSession();
        }
    }
}