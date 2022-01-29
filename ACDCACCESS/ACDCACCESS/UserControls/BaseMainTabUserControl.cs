using System.Windows.Forms;

namespace ACDCAccess
{
	partial class BaseMainTabUserControl : UserControl
	{

		protected bool _testMode;

		public BaseMainTabUserControl()
		{
			InitializeComponent();
		}

		public BaseMainTabUserControl(bool testMode)
		{
			InitializeComponent();
			_testMode = testMode;
		}
	}
}
