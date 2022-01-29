using BTCHLTRSFD;
using System;
using System.Collections.Generic;
using Xunit;
using System.IO;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;

namespace BTCHLTRSFD.Tests
{
    public class BatchLettersFedTest
    {
        [Theory]
        [InlineData("T:\\UNWS14.NWS14R7.IDONTHAVEDATA")]
        public void IsSasFileEmptyTrue(string file)
        {
            //Spawns up a session only so that BatchLettersFed Constructor doesnt crash
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper LoginHelper = BatchProcessingHelper.GetNextAvailableId("BTCHLTRSFD", "BatchCornerstone");
            ri.Login(LoginHelper.UserName, LoginHelper.Password);           

            using (FileStream fs = File.Create(file)) //Create a dummy file with the filename of the InlineData
            {
                Byte[] bytes = new UTF8Encoding(true).GetBytes("ARC, ACCT NO, FIRST NAME, MIDDLE INITIAL, LAST NAME, STREET 1, STREET 2, CITY, STATE, ZIP, FOREIGN COUNTRY, FOREIGN STATE, ACS KEYLINE, ARC DATE\r\n");
                fs.Write(bytes, 0, bytes.Length);
            }
            Assert.True(new BTCHLTRSFD.BatchLettersFed(ri).IsSasFileEmpty(file)); //Check to make sure said file doesnt have any data records using actual method
            ri.LogOut();
            ri.CloseSession();
        }
    }
}
