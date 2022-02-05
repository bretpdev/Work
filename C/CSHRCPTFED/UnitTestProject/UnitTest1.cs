using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uheaa.Common.ProcessLogger;
using CSHRCPTFED.Infrastructure;
using CSHRCPTFED.ViewModels;
using CSHRCPTFED.Models;
using System;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_CshRcpt()
        {
            ProcessLogRun PLR;
            DataAccess DA;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            PLR = new ProcessLogRun("INCARBWRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            DA = new DataAccess(PLR.LDA);
            CshRcptVM vm = new CshRcptVM();
            vm.Amount = "10.0";
            vm.GetDate = DateTime.Now;
            vm.Account = "0123456789";
            vm.CheckId = "1024";
            vm.Payee = EPayee.Borrower;

            CshRcpt obj = new CshRcpt(vm, DA, PLR);

            Assert.IsTrue(obj.Process("Test") == Errno.ArcAdded);
        }
    }
}
