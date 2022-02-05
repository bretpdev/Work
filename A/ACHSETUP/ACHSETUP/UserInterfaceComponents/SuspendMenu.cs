using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUP
{
	partial class SuspendMenu : Form
	{
		private SuspendOptions SuspendOptions;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required by the Windows Forms Designer,
		/// but it won't work with the script.
		/// </summary>
		public SuspendMenu()
		{
			InitializeComponent();
		}

		public SuspendMenu(SuspendOptions suspendOptions)
		{
			InitializeComponent();

			SuspendOptions = suspendOptions;
			suspendOptionsBindingSource.DataSource = SuspendOptions;
		}

		private void Ok_Click(object sender, System.EventArgs e)
		{
			if (radBorrowerRequest.Checked)
			{
				SuspendOptions.SelectedRequestor = SuspendOptions.Requestor.Borrower;
				DialogResult = DialogResult.OK;
			}
			else if (radStaffRequest.Checked)
			{
				SuspendOptions.SelectedRequestor = SuspendOptions.Requestor.Staff;
				DialogResult = DialogResult.OK;
			}
			else
				Dialog.Def.Ok("You must provide a number of bills to suspend.");
		}
	}
}