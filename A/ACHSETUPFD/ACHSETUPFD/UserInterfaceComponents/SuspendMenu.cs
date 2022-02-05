using System.Windows.Forms;


namespace ACHSETUPFD
{
	partial class SuspendMenu : Form
	{
        private SuspendOptions SuspendOptions { get; set; }

		public SuspendMenu(SuspendOptions suspendOptions)
		{
			InitializeComponent();

			SuspendOptions = suspendOptions;
			suspendOptionsBindingSource.DataSource = SuspendOptions;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
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
			{
				MessageBox.Show("You must provide a number of bills to suspend.");
			}
		}
	}
}
