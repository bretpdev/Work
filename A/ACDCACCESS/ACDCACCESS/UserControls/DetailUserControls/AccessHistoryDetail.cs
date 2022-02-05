using System.Drawing;
using System.Windows.Forms;

namespace ACDCAccess
{
	partial class AccessHistoryDetail : UserControl
	{
		public AccessHistoryDetail()
		{
			InitializeComponent();
		}

		public AccessHistoryDetail(FullAccessRecord far)
		{
			InitializeComponent();
			fullAccessRecordBindingSource.DataSource = far;
			lblKey.MaximumSize = new Size(lblKey.Width, 0);
			lblAddedBy.MaximumSize = new Size(lblAddedBy.Width, 0);
			lblBusinessUnit.MaximumSize = new Size(lblBusinessUnit.Width, 0);
			lblRemovedBy.MaximumSize = new Size(lblRemovedBy.Width, 0);
			lblSystem.MaximumSize = new Size(lblSystem.Width, 0);
			lblDateAdded.MaximumSize = new Size(lblDateAdded.Width, 0);
			lblDateRemoved.MaximumSize = new Size(lblDateRemoved.Width, 0);
		}
	}
}
