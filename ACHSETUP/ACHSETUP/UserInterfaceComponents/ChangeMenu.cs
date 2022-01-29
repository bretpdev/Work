using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUP
{
	partial class ChangeMenu : Form
	{
		private ChangeMenuOptions Options;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required by the Windows Forms Designer,
		/// but it won't work with the script.
		/// </summary>
		public ChangeMenu()
		{
			InitializeComponent();
		}

		public ChangeMenu(ChangeMenuOptions options)
		{
			InitializeComponent();
			Options = options;
		}

		private void Ok_Click(object sender, System.EventArgs e)
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
				Dialog.Def.Ok("You must select one of the two options.");
		}
	}
}