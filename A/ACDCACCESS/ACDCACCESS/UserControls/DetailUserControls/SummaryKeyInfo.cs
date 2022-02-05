using System.Windows.Forms;

namespace ACDCAccess
{
	partial class SummaryKeyInfo : UserControl
	{
		public SummaryKeyInfo()
		{
			InitializeComponent();
		}

		public SummaryKeyInfo(Key key)
		{
			InitializeComponent();
			keyBindingSource.DataSource = key;
		}
	}
}
