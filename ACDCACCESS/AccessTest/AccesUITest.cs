using System;
using Xunit;
using ACDCAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace AccessTest
{
    public class AccesUITest
    {
        [Fact]
        public void GetListOfRoles_ShouldReturnData()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun("ACDCAccess", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.Mode.Dev);
            ACDCAccess.DataAccess da = new DataAccess(logRun);

            Assert.NotEmpty(da.GetRoles());
        }
    }
}