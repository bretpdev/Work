using System.Threading;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System;
using System.Reflection;

namespace ACDCAccess
{
    public class LibraryStarter
    {
        private readonly int _sqlUserId;
        private readonly Thread _uiThread;
        private readonly List<string> _userRoles;
        public ProcessLogRun LogRun { get; set; }

        public LibraryStarter(int mode, int sqlUserId, List<string> userRoles)
        {
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            LogRun = new ProcessLogRun("ACDCAccess", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.None, DataAccessHelper.CurrentMode);
            _sqlUserId = sqlUserId;
			_userRoles = userRoles;
			_uiThread = new Thread(new ThreadStart(UiDelegate));
			_uiThread.SetApartmentState(ApartmentState.STA);
			_uiThread.Start();
		}

        private void UiDelegate()
        {
            AccessUI frm  = new AccessUI(_sqlUserId, _userRoles, LogRun);
            frm.ShowDialog();
        }
    }
}