using System;
using System.Collections.Generic;
using MD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uheaa.Common.DataAccess;

namespace MauiDUDETests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void GetScriptsAndServicesCS()
        {
            //perform initialization
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            MauiDUDE.DataAccess.DA = new MauiDUDE.DataAccess(ModeHelper.GetProcessLogRun(DataAccessHelper.Region.CornerStone));

            string homePage = "CornerStone Customer Services";
            List<string> parentMenus = new List<string>()
            { 
                "",
                "Activity History",
                "30 Days",
                "60 Days",
                "90 Days",
                "All",
                "Scripts",
                "Create Future Dated Queue FED",
                "Check by Phone (FED)",
                "Payoff Loan Letter (FED)"
            };

            //Test
            foreach(string parent in parentMenus)
            {
                var dt = MauiDUDE.DataAccess.DA.GetScriptAndServicesMenuOptions(homePage, parent);
                if(dt == null)
                {
                    Console.WriteLine($"Null Result: Home Page {homePage}, Parent Menu {parent}");
                }
                Assert.IsTrue(dt != null);
            }
        }

        [TestMethod]
        public void GetScriptsAndServicesUH()
        {
            //perform initialization
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            MauiDUDE.DataAccess.DA = new MauiDUDE.DataAccess(ModeHelper.GetProcessLogRun(DataAccessHelper.Region.Uheaa));

            string homePage = "CornerStone Customer Services";
            List<string> parentMenus = new List<string>()
            {
                "",
                "Activity History",
                "30 Days",
                "60 Days",
                "90 Days",
                "All",
                "Scripts",
                "Re-Queue Task",
                "Waive Late Fees",
                "Employer Add",
                "Incarcerated Borrower",
                "Reference Add",
                "Auxiliary Services Letters",
                "Services",
                "Check By Phone",
                "Re-Print Bill",
                "Due Date Change Request"
            };

            //Test
            foreach (string parent in parentMenus)
            {
                var dt = MauiDUDE.DataAccess.DA.GetScriptAndServicesMenuOptions(homePage, parent);
                if (dt == null)
                {
                    Console.WriteLine($"Null Result: Home Page {homePage}, Parent Menu {parent}");
                }
                Assert.IsTrue(dt != null);
            }
        }

        [TestMethod]
        public void AddCallCategorizationRecord()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            MauiDUDE.DataAccess.DA = new MauiDUDE.DataAccess(ModeHelper.GetProcessLogRun(DataAccessHelper.Region.Uheaa));
            string guid = Guid.NewGuid().ToString().Substring(0, 30);//table is a varchar(30)
            var record = new MauiDUDE.CallCategorizationEntry() { Category = "Other", Reason = "", LetterID = "", Region = "Uheaa", UserID = "MDTests", Comments = guid };
            MauiDUDE.DataAccess.DA.AddCallCategorizationRecord(record);

            int count = DataAccessHelper.ExecuteSingle<int>("MDTestCallCategorizationRecord", DataAccessHelper.Database.MauiDude, SqlParams.Single("CMT", guid));
            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        public void GetCorrectDB()
        {
            //perform initialization
            MauiDUDE.DataAccess.DA = new MauiDUDE.DataAccess(ModeHelper.GetProcessLogRun(DataAccessHelper.Region.Uheaa));

            DataAccessHelper.Database uheaaWarehouse = DataAccessHelper.Database.Udw;
            DataAccessHelper.Database cornerstoneWarehouse = DataAccessHelper.Database.Cdw;

            //test
            DataAccessHelper.Database uheaaDataWarehouseResult = MauiDUDE.DataAccess.DA.GetWarehouseDbFromRegion(DataAccessHelper.Region.Uheaa);
            DataAccessHelper.Database cornerstoneDataWarehouseResult = MauiDUDE.DataAccess.DA.GetWarehouseDbFromRegion(DataAccessHelper.Region.CornerStone);

            Assert.IsTrue(uheaaWarehouse == uheaaDataWarehouseResult);
            Assert.IsTrue(cornerstoneWarehouse == cornerstoneDataWarehouseResult);
        }

        [TestMethod]
        public void GetsDBRegionException()
        {
            //perform initialization
            MauiDUDE.DataAccess.DA = new MauiDUDE.DataAccess(ModeHelper.GetProcessLogRun(DataAccessHelper.Region.Uheaa));
            bool exceptionReceived = false;

            //Test
            try
            {
                var db = MauiDUDE.DataAccess.DA.GetWarehouseDbFromRegion(DataAccessHelper.Region.None);
            }
            catch(MauiDUDE.RegionNotValidException e)
            {
                exceptionReceived = true;
            }

            Assert.IsTrue(exceptionReceived);
        }
    }
}
