using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;

namespace UnitTests
{
    [TestClass]
    public class TestRegimen
    {
        private List<string> banaList = new List<string>(new string[] { "814817", "818334", "824421", "826079", "831495", "832733", "801871", "802176", "805317", "806746", "807674", "811735" });

        private List<string> uheaaList = new List<string>(new string[] { "834529", "829306", "826717", "830248", "828476", "834437", "834493", "829769", "834396", "999775", "83449301", "82847601" });

        private LENDERLTRS.Lenders lenderLists = new LENDERLTRS.Lenders();

        DataAccessHelper.Mode theMode;


        public TestRegimen()
        {
            theMode = DataAccessHelper.Mode.Dev;
        }

        [TestMethod]
        public void banaListIntact()
        {
            int total = 12;

            foreach (string bana in banaList)
                if (lenderLists.InBana.ContainsKey(bana))
                    --total;

            Assert.IsTrue(total == 0);
        }

        [TestMethod]
        public void uheaaListIntact()
        {
            int total = 12;

            foreach (string uheaa in uheaaList)
                if (lenderLists.InUheaa.ContainsKey(uheaa))
                    --total;

            Assert.IsTrue(total == 0);
        }

        [TestMethod]
        public void arcAccess()
        {
            int eax = 0;
            ArcData arc = null;
            string[] testArgs = { "dev" };
            DataAccessHelper.StandardArgsCheck(testArgs, "LENDERLTRS");

            ProcessLogData pld = new ProcessLogData();
            ProcessLogRun plr = new ProcessLogRun(pld.ProcessLogId, "LENDERLTRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev, true);
            ArcAddResults result = new ArcAddResults();

            try
            {
                arc = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    ArcTypeSelected = ArcData.ArcType.Atd22ByBalance ,
                    AccountNumber = "0123456789",
                    Arc = "LCNCM",
                    ScriptId = "UNITTEST",
                    Comment = "Test from LENDERLTRS"
                };
            }
            catch (Exception e)
            {
                ;
            }

            try
            {
                result = arc.AddArc();
            }
            catch(Exception ex)
            {
                ;
            }
            plr.LogEnd();

            Assert.IsTrue(result.ArcAdded);
        }
    }
}

