using System.Collections.Generic;
using System.Windows.Forms;

namespace BARCODEFED
{
    partial class ForwardingDialog : Form
	{
		private readonly AddressHygiene _daRules;

		public ForwardingDialog(IList<string> states, ForwardingInfo forwardingInfo, string lastVerified)
		{
			InitializeComponent();
			
			_daRules = new AddressHygiene();
			cmbStates.DataSource = states;
			forwardingInfoBindingSource.DataSource = forwardingInfo;
            AddressVerifiedlbl.Text = lastVerified;
		}

		private void btnRules_Click(object sender, System.EventArgs e)
		{
			if (_daRules.Visible)
			{ _daRules.BringToFront(); }
			else
			{ _daRules.Show(); }
		}
	}
}