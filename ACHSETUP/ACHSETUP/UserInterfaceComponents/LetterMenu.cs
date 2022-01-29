using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUP
{
	partial class LetterMenu : Form
	{
		private LetterSelection Selection;

		/// <summary>
		/// DO NOT USE!!!
		/// The parameterless constructor is required by the Windows Forms Designer,
		/// but it won't work with the script.
		/// </summary>
		public LetterMenu()
		{
			InitializeComponent();
		}

		public LetterMenu(LetterSelection selection)
		{
			InitializeComponent();
			Selection = selection;
		}

		private void Ok_Click(object sender, System.EventArgs e)
		{
			if (radApproval.Checked)
			{
				Selection.Selected = LetterSelection.Letter.Approved;
				DialogResult = DialogResult.OK;
			}
			else if (radDenial.Checked)
			{
				Selection.Selected = LetterSelection.Letter.Denied;
				DialogResult = DialogResult.OK;
			}
			else if (radBoth.Checked)
			{
				Selection.Selected = (LetterSelection.Letter.Approved | LetterSelection.Letter.Denied);
				DialogResult = DialogResult.OK;
			}
			else if (radNone.Checked)
			{
				Selection.Selected = LetterSelection.Letter.None;
				DialogResult = DialogResult.OK;
			}
			else
				Dialog.Def.Ok("You must select at least one option.");
		}
	}
}