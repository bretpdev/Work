using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestRegimin
    {
        [TestMethod]
        public void TestArc()
        {
            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = "0123456789",
                Arc = "XPHN4",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = "",
                IsEndorser = false,
                IsReference = false,
                ScriptId = "PHONE4RMVL",
            };
            ArcAddResults result = arc.AddArc();
        }
    }
}
