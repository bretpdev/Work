using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Xunit;

namespace UnitTests
{
    public class TestRegimen
    {
        // This tesdt only returns true if there are data in database.
        [Fact]
        private void RecordsExistForLetter()
        {
            string[] eax = new string[] { "dev", "uheaa" };
            LENDERUPDT.DataAccess DA;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
            ProcessLogRun logRun = new ProcessLogRun("LENDERUPDT", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new LENDERUPDT.DataAccess(logRun.ProcessLogId);
            DataAccessHelper.StandardArgsCheck(eax, DA.ScriptId);
            List<LENDERUPDT.LenderUpdates> records = new List<LENDERUPDT.LenderUpdates>();

            records = DA.GetUnprocessedLenderUpdates();

            logRun.LogEnd();
            Assert.True(records.Count > 0);
        }
    }
}
