using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Xunit;


namespace UnitTest
{
    public class TestRegimen
    {
        DMCSNTFYFE.DataAccess DA;
        ProcessLogRun logRun;
        string borrowerData = "0123456789,ENILYEK,Test,Regemin,\"3927 LPP Lane\",\"\",SaltCity,Ut,01234,U.S.A.";
        private List<string> FilePatterns { get; set; }

        [Theory]
        [InlineData("UNWS62.NWS62R2.*")]
        // Any filename can be tested.
        public void TestSproc(string testString)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DMCSNTFYFE.BorrowerData bor = populate();
            string[] eax = new string[] { "dev", "uheaa" };
            Assert.False(CheckFile.Check(EnterpriseFileSystem.FtpFolder, false, testString).IsPopulated());
        }

        public DMCSNTFYFE.BorrowerData populate()
        {
            return new DMCSNTFYFE.BorrowerData
            {
                AccountNumber =  "0123456789",
                KeyLine = "ENILYEK",
                FirstName = "Test",
                LastName = "Regemin",
                Address1 = "3927 LPP Lane",
                Address2 = "",
                City = "SaltCity",
                State = "Ut",
                Zip = "01234",
                Country = "U.S.A."
            };
        }
    }
}
