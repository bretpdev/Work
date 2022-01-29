using Xunit;
using INCIDENTRP;
using SubSystemShared;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System;
using System.Reflection;

namespace INCIDENTRP_Tests
{
    public class SubSystemTest
    {
        [Theory]
        [InlineData("ROLE - Application Development - Programmer")]
        public void HasAccess_ShouldHaveAccess(string role)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun("INCIDENTRP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            SubSystem ss = new SubSystem(new SqlUser(), role, logRun);

            Assert.True(ss.HasAccess);
        }

        [Theory]
        [InlineData("ROLE - IT - Systems")]
        public void HasAccess_ShouldNotHaveAccess(string role)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun("INCIDENTRP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            SubSystem ss = new SubSystem(new SqlUser(), role, logRun);

            Assert.True(!ss.HasAccess);
        }

        [Theory]
        [InlineData("2011 - 11 - 11 11:48:57.170")]
        public void GetEarliestTicketDate_ShouldBeFirstTicketDate(DateTime databaseDate)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun("INCIDENTRP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            SubSystem ss = new SubSystem(new SqlUser(), null, logRun);

            Assert.Equal(databaseDate, ss.GetEarliestTicketCreateDate());
        }

        [Theory]
        [InlineData("2012 - 11 - 11 11:48:57.170")]
        public void GetEarliestTicketDate_ShouldNotBeFirstTicketDate(DateTime databaseDate)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            ProcessLogRun logRun = new ProcessLogRun("INCIDENTRP", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.Mode.Dev);
            SubSystem ss = new SubSystem(new SqlUser(), null, logRun);

            Assert.NotEqual(databaseDate, ss.GetEarliestTicketCreateDate());
        }
    }
}