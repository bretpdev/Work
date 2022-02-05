using System;
using System.Windows.Forms;

namespace ACURINTFED
{
	partial class MainForm : Form
	{
		private UserInput _userInput;

		public MainForm(UserInput userInput, string recoveryPoint)
		{
			InitializeComponent();
			_userInput = userInput;
			lblContinueFrom.Text = recoveryPoint;
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			_userInput.Selected = UserInput.Selection.Continue;
			DialogResult = DialogResult.OK;
		}

		private void btnCreateRequest_Click(object sender, EventArgs e)
		{
			_userInput.Selected = UserInput.Selection.CreateRequestFile;
			DialogResult = DialogResult.OK;
		}

		private void btnGetResponse_Click(object sender, EventArgs e)
		{
			_userInput.Selected = UserInput.Selection.GetResponseFile;
			DialogResult = DialogResult.OK;
		}

		private void btnProcessResponse_Click(object sender, EventArgs e)
		{
			_userInput.Selected = UserInput.Selection.ProcessResponseFile;
			DialogResult = DialogResult.OK;
		}

		private void btnSendRequest_Click(object sender, EventArgs e)
		{
			_userInput.Selected = UserInput.Selection.SendRequestFile;
			DialogResult = DialogResult.OK;
		}
	}//class
}//namespace
