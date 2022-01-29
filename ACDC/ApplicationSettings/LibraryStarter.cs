using System;
using System.Reflection;
using System.Threading;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ApplicationSettings
{
    public class LibraryStarter
    {
        private DataAccessHelper.Mode Mode { get; set; }
        private ProcessLogRun LogRun { get; set; }
        private int SqlUserId { get; set; }

        public LibraryStarter(int mode, int sqlUserId)
        {
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            LogRun = new ProcessLogRun("ACDC", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DatabaseAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly());
            SqlUserId = sqlUserId;
            Thread thread = new Thread(new ThreadStart(UiDelegate));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void UiDelegate()
        {
            ApplicationMaintenance am = new ApplicationMaintenance(LogRun, SqlUserId);
            am.ShowDialog();
        }
    }
}