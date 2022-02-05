using System;
using System.Linq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using SCRAINTUPD;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace SCRAINTUPD.Tests
{
    public class RedisclosureTests
    {
        public ProcessLogRun LogRun { get; set; }
        public ScraRecord Dummy { get; set; }
        public MockRI RI { get; set; }
        public ScraProcess Processor { get; set; }

        public RedisclosureTests()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun("SCRAINTUPD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            Dummy = new ScraRecord();
            RI = new MockRI();
            Processor = new ScraProcess(LogRun, DataAccessHelper.CurrentRegion);
            Processor.RI.CloseSession();
        }

        [Theory]
        [InlineData("01005")]
        public void ExtendSingleValidCode(string messageCode)
        {
            RI.MessageCode = messageCode;
            Processor.RI = RI;
            bool returnCode = Processor.ExtendSingle(Dummy);
            Assert.True(returnCode);
        }

        [Theory]
        [InlineData("00000"), InlineData("21311")]
        public void ExtendSingleInvalidCode(string messageCode)
        {
            RI.MessageCode = messageCode;
            Processor.RI = RI;
            bool returnCode = Processor.ExtendSingle(Dummy);
            Assert.False(returnCode);
        }
    }
}
