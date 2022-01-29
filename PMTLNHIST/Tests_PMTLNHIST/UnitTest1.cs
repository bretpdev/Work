using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMTLNHIST;
using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace Tests_PMTLNHIST
{
    [TestClass]
    public class UnitTest1
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }

        public UnitTest1()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            LogRun = new ProcessLogRun("PMTLNHIST", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(LogRun.LDA);
    
        }

        [TestMethod]
        public void BorrowerDataFound()
        {
            ReflectionInterface ri = new ReflectionInterface();
            while (!ri.CheckForText(16, 2, "LOGON"))
                Thread.Sleep(1000);
            BatchProcessingLoginHelper.Login(LogRun, ri, "PMTLNHIST", "BatchUheaa");
            LoanPaymentHistory paymentHistory = new LoanPaymentHistory(ri);
            string ssn = paymentHistory.GetSsn(LogRun.LDA.ExecuteSingle<string>("pmtlnhist.GetCurrentAccount", Ols).Result);
            BorrowerData borrowerData = paymentHistory.GetBorrowerData(ssn);
            Assert.IsNotNull(borrowerData);
            ri.CloseSession();
        }
    }
}
