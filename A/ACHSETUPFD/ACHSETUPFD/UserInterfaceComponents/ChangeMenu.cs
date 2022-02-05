using System.Windows.Forms;


namespace ACHSETUPFD
{
	partial class ChangeMenu : Form
	{
        private ChangeMenuOptions Options { get; set; }

		public ChangeMenu(ChangeMenuOptions options)
		{
			InitializeComponent();
			Options = options;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (radAddRemove.Checked)
			{
				Options.Selection = ChangeMenuOptions.ChangeOption.AddRemove;
				DialogResult = DialogResult.OK;
			}
			else if (radModify.Checked)
			{
				Options.Selection = ChangeMenuOptions.ChangeOption.Modify;
				DialogResult = DialogResult.OK;
			}
			else
			{
				MessageBox.Show("You must select one of the two options.");
			}
		}
	}
}
