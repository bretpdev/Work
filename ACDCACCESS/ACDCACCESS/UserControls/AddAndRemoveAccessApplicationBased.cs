using System.Collections.Generic;

namespace ACDCAccess
{
	partial class AddAndRemoveAccessApplicationBased : BaseMainTabUserControl
	{
		private readonly DataAccess _dataAccess;

		public AddAndRemoveAccessApplicationBased()
			: base()
		{
			InitializeComponent();
		}

		public AddAndRemoveAccessApplicationBased(bool testMode)
			: base(testMode)
		{
			InitializeComponent();
			_dataAccess = new DataAccess(testMode);
			IEnumerable<string> applications = _dataAccess.GetApplications();
			foreach (string application in applications)
			{
				pnlData.Controls.Add(new ApplicationRoot(testMode, application));
			}
		}
	}
}
