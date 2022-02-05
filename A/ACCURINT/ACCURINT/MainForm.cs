using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace ACCURINT
{
    partial class MainForm : Form
    {
        private UserInput _userInput;

        public MainForm(UserInput userInput)
        {
            InitializeComponent();
            _userInput = userInput;
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
                FirstProcCheck.Visible = true;
        }

        private void btnCreateRequest_Click(object sender, System.EventArgs e)
        {
            _userInput.Selected = UserInput.Selection.CreateRequestFile;
            DialogResult = DialogResult.OK;
        }

        private void btnGetResponse_Click(object sender, System.EventArgs e)
        {
            _userInput.Selected = UserInput.Selection.GetResponseFile;
            DialogResult = DialogResult.OK;
        }

        private void btnProcessResponse_Click(object sender, System.EventArgs e)
        {
            _userInput.Selected = UserInput.Selection.ProcessResponseFile;
            DialogResult = DialogResult.OK;
        }

        private void btnRunAll_Click(object sender, System.EventArgs e)
        {
            _userInput.Selected = UserInput.Selection.RunAll;
            DialogResult = DialogResult.OK;
        }

        private void btnSendInput_Click(object sender, System.EventArgs e)
        {
            _userInput.Selected = UserInput.Selection.SendInputFile;
            DialogResult = DialogResult.OK;
        }

        private void btnSendRequest_Click(object sender, System.EventArgs e)
        {
            _userInput.Selected = UserInput.Selection.SendRequestFile;
            DialogResult = DialogResult.OK;
        }

        private void FirstProcCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            _userInput.OnlyProcessFirstSelection = FirstProcCheck.Checked;
        }
    }//class
}//namespace
