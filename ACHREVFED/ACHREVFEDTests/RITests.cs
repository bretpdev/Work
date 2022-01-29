using System;
using System.Linq;
using Xunit;
using ACHREVFED;
using Uheaa.Common.DataAccess;

namespace ACHREVFEDTests
{
    public class RITests
    {
        //[Fact]
        //public void ChecksScreenCodeReturnsFalseIfInvalid()
        //{
        //    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
        //    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        //    MockRI ri = new MockRI();
        //    ri.ScreenCode = "TSX7J";
        //    ACHREVFED.AchReviewFed.InitalizeProcessLogs();
        //    bool result = ACHREVFED.AchReviewFed.ProcessCTS7O(ri, "");
        //    Assert.False(result);
        //}

        //[Fact]
        //public void ChecksScreenCodeReturnsTrueIfValid()
        //{
        //    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
        //    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        //    MockRI ri = new MockRI();
        //    ri.ScreenCode = "TSX7K";
        //    ri.MessageCode = "02526";
        //    ACHREVFED.AchReviewFed.InitalizeProcessLogs();
        //    bool result = ACHREVFED.AchReviewFed.ProcessCTS7O(ri, "");
        //    Assert.True(result);
        //}

        //[Fact]
        //public void ChecksMessageCodeReturnsFalseIfInValid()
        //{
        //    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
        //    DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        //    MockRI ri = new MockRI();
        //    ri.ScreenCode = "TSX7K";
        //    ri.MessageCode = "02525";
        //    ACHREVFED.AchReviewFed.InitalizeProcessLogs();
        //    bool result = ACHREVFED.AchReviewFed.ProcessCTS7O(ri, "");
        //    Assert.False(result);
        //}
    }
}
