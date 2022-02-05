using System.Threading;
using System.Collections.Generic;
using Q;

namespace ACDCFlows
{
	public class LibraryStarter
	{
		private readonly DataAccessBase.ConfigurationMode _mode;
		private readonly int _sqlUserId;
		private readonly Thread _uiThread;
		private readonly IEnumerable<string> _userRoles;

		public LibraryStarter(int mode, int sqlUserId, IEnumerable<string> userRoles)
		{
			_mode = (DataAccessBase.ConfigurationMode)mode;
			_sqlUserId = sqlUserId;
			_userRoles = userRoles;
			_uiThread = new Thread(new ThreadStart(UIDelegate));
			_uiThread.SetApartmentState(ApartmentState.STA);
			_uiThread.Start();
		}

		private void UIDelegate()
		{
			FlowsUI flows = new FlowsUI(_mode, _sqlUserId, _userRoles);
			flows.ShowDialog();
		}
	}
}
