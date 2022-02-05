using System;
using System.Linq;
using System.IO;
using Xunit;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace ECORXMLGENTest
{
    public class XMLGeneratorTest
    {
        [Fact]
        public void ShouldDeleteDir()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            ProcessLogRun plr = new ProcessLogRun("ECORXMLGENTest", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            string dir = Path.Combine(EnterpriseFileSystem.TempFolder, Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            new ECORXMLGEN.XMLGenerator(plr).DeleteDir(dir);

            Assert.False(Directory.Exists(dir));
        }
    }
}
