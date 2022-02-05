using System;
using System.Linq;
using Xunit;
using ACHREVFED;
using Uheaa.Common.DataAccess;
using System.IO;

namespace ACHREVFEDTests
{
    public class DialerFileTests
    {
        //[Fact]
        //public void WriteToDialerFile()
        //{
        //    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
        //    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        //    AchReviewFed.DialerFile = null;
        //    ACHData data = new ACHData()
        //    {
        //        AccountNumber = "0123456789",
        //        AltConsent = "N",
        //        AltPhone = "8014488096",
        //        AmountDue = 100,
        //        DaysDelq = 0,
        //        DC_DOM_ST = "UT",
        //        DF_ZIP_CDE = "84047",
        //        DM_CT = "SLC",
        //        DX_STR_ADR_1 = "123 test st"
        //    };

        //    AchReviewFed.AddToDialerFile(data);

        //    Assert.Equal(data.ToString(), File.ReadAllLines(AchReviewFed.DialerFile)[0]);
        //}

        //[Fact]
        //public void VerifyAppend()
        //{
        //    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
        //    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        //    AchReviewFed.DialerFile = null;
        //    ACHData data = new ACHData()
        //    {
        //        AccountNumber = "0123456789",
        //        AltConsent = "N",
        //        Ssn = "123450000",
        //        AltPhone = "8014488096",
        //        AmountDue = 100,
        //        DaysDelq = 0,
        //        DC_DOM_ST = "UT",
        //        DF_ZIP_CDE = "84047",
        //        DM_CT = "SLC",
        //        DX_STR_ADR_1 = "123 test st"
        //    };

        //    ACHData data1 = new ACHData()
        //    {
        //        AccountNumber = "0123456789",
        //        Ssn = "123456780",
        //        AltConsent = "N",
        //        AltPhone = "8014488096",
        //        AmountDue = 100,
        //        DaysDelq = 0,
        //        DC_DOM_ST = "UT",
        //        DF_ZIP_CDE = "84047",
        //        DM_CT = "SLC",
        //        DX_STR_ADR_1 = "123 test st"
        //    };

        //    AchReviewFed.AddToDialerFile(data);
        //    AchReviewFed.AddToDialerFile(data1);

        //    Assert.Equal(2, File.ReadAllLines(AchReviewFed.DialerFile).Length);
            
        //}
    }
}
