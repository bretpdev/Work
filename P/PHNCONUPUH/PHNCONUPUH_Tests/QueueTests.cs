using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Xunit;

namespace PHNCONUPUH_Tests
{
    public class QueueTests
    {
        [Fact]
        public void CheckForQueues_QueueDataAvailable()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            
            List<PHNCONUPUH.QueueInformation> queues = PHNCONUPUH.DataAccess.GetQueueInfo();
            Assert.True(queues.Count > 0);
        }

        [Fact]
        public void CheckForEndorser_ShouldDetermineEndorserAcctNo()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ReflectionInterface ri = new ReflectionInterface();
            MessageBox.Show("Log in then hit Insert");
            ri.PauseForInsert();

            ri.FastPath("TX3ZITX6X");
            ri.PutText(6, 37, "PU");
            ri.PutText(8, 37, "01", ReflectionInterface.Key.Enter);
            string acctNo = ri.GetText(9, 61, 10);
            string ssn = "";

            if (acctNo.IsNullOrEmpty() || !acctNo.IsNumeric())
                ssn = ri.GetText(8, 6, 9);
            else
                ssn = acctNo;

            Assert.True(ssn.IsNumeric());

            ri.CloseSession();
        }
    }
}