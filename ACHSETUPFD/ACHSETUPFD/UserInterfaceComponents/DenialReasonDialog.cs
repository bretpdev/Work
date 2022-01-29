using System.Windows.Forms;

using System.Collections.Generic;

namespace ACHSETUPFD
{
	public partial class DenialReasonDialog : Form
	{
        private List<string> DenialReasons { get; set; }

        public DenialReasonDialog(ref List<string> denialReasons)
		{
			InitializeComponent();

            DenialReasons = denialReasons;
		}

		private void chkIncomplete_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkIncomplete.Checked)
			{
				txtDescribe.Enabled = true;
			}
			else
			{
				txtDescribe.Clear();
				txtDescribe.Enabled = false;
			}
		}

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            //_denialReasons = new List<string>();
            foreach (Control cntrl in grpDenialReasons.Controls)
            {
                //if control is a checkbox and it is checked then
                if (cntrl is CheckBox && (cntrl as CheckBox).Checked)
                {
                    if (cntrl.Name == "chkIncomplete")
                    {
                        DenialReasons.Add(txtDescribe.Text);
                    }
                    else
                    {
                        DenialReasons.Add(cntrl.Text);
                    }
                }
            }
            if (DenialReasons.Count == 0)
            {
                MessageBox.Show("You must select at least one denial reason.  Please try again.", "One Denial Reason Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (DenialReasons.Count > 5)
            {
                MessageBox.Show("You can't select more than 5 denial reasons.  Please try again.", "Too Many Denial Reasons Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DenialReasons.Clear();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
	}
}
