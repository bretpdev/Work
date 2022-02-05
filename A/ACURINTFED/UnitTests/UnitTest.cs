using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Xunit;

namespace UnitTests
{
    public class UnitTest
    {
    
        
    
        //DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        [Fact]
        public void CheckIfFileExists()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            Assert.False(ACURINTFED.RequestFile.Exists());

        }
    }
}
