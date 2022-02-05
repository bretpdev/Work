using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCHUPDATES;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace SCHUPDATES_TESTS
{
    [TestClass]
    public class FileDataTests
    {
        ProcessLogRun LogRun { get; set; }

        public FileDataTests()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun("SCHUPDTEST", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
        }

        [TestMethod]
        public void LoadData_DataMatches()
        {
            List<string> data = new List<string>()
            {
                "00367500",
                "STFFRD",
                "000706",
                "B",
                "E",
                DateTime.Now.Date.ToString("MMddyyyy"),
                "00367510",
                "",
                ""
            };
            FileData fData = new FileData(data, LogRun);

            Assert.AreSame(fData.SchoolId, data[0]);
            Assert.AreSame(fData.LoanProgram, data[1]);
            Assert.AreSame(fData.Guarantor, data[2]);
            Assert.AreSame(fData.TX10Approval, data[3]);
            Assert.AreSame(fData.TX13Approval, data[4]);
            Assert.AreEqual(fData.ApprovalDate, data[5].ToDate());
            Assert.AreEqual(fData.MergedSchool, data[7]);
        }

        [TestMethod]
        public void LoadData_DataNotLoaded()
        {
            List<string> data = new List<string>()
            {
                "00367500",
                "STFFRD",
                "000706",
                "B",
                "E",
                "17/74/1845",
                "00365710"
            };
            FileData fData = new FileData(data, LogRun);

            Assert.AreNotSame(fData.SchoolId, data[0]);
            Assert.AreNotSame(fData.LoanProgram, data[1]);
            Assert.AreNotSame(fData.Guarantor, data[2]);
            Assert.AreNotSame(fData.TX10Approval, data[3]);
            Assert.AreNotSame(fData.TX13Approval, data[4]);
            Assert.AreNotEqual(fData.ApprovalDate, data[5].ToDateNullable().HasValue ? data[5].ToDateNullable().Value : new object());
            Assert.AreNotSame(fData.MergedSchool, data[6]);
        }
    }
}