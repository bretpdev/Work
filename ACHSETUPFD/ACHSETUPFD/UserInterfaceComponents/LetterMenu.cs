using System.Windows.Forms;


namespace ACHSETUPFD
{
	partial class LetterMenu : Form
	{
		private LetterSelection Selection { get; set; }

		public LetterMenu(LetterSelection selection)
		{
			InitializeComponent();
			Selection = selection;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
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
			{
				MessageBox.Show("You must select at least one option.");
			}
		}
	}
}
