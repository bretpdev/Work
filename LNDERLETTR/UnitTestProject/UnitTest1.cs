using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uheaa.Common.DataAccess;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        DataAccessHelper.Mode Mode = DataAccessHelper.Mode.Dev;

        [TestMethod]
        public void TestTemplateAvailable()
        {
            string template = @"X:\PADD\BorrowerServices\Test\US06BLCNTM.docx";
            Assert.IsTrue(File.Exists(template));
        }

        [TestMethod]
        public void TestArcAdd()
        {
            List<int> Sequences = new List<int>  { 1, 2, 3 };
            string[] args = { "dev" };
            DataAccessHelper.StandardArgsCheck(args, "LNDERLETTR");
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = "0123456789",
                Arc = "LCNCM",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan, 
                Comment = "This is a TEST Arc.",
                IsEndorser = false,
                IsReference = false,
                ScriptId = "LNDERLETTR",
                LoanSequences = Sequences
            };
            ArcAddResults result = arc.AddArc();
            Assert.IsTrue(result.ArcAdded);
        }
    }
}
