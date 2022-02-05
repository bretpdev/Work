using System.Windows.Forms;
using System.Collections.Generic;
using Uheaa.Common;

namespace ACHSETUP
{
    public partial class DenialReasonDialog : Form
    {

        //IMPORTANT!!!!
        //Be careful when changing text in the checkboxes on this form.  
        //The calling code uses the text in the checkboxes to fill out a letter.

        private List<string> DenialReasons;

        /// <summary>
        /// DO NOT USE!!!
        /// The parameterless constructor is required by the Windows Forms Designer,
        /// but it won't work with the script.
        /// </summary>
        public DenialReasonDialog()
        {
            InitializeComponent();
        }

        public DenialReasonDialog(ref List<string> denialReasons)
        {
            InitializeComponent();
            DenialReasons = denialReasons;
        }

        private void Incomplete_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkIncomplete.Checked)
                txtDescribe.Enabled = true;
            else
            {
                txtDescribe.Clear();
                txtDescribe.Enabled = false;
            }
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            //_denialReasons = new List<string>();
            foreach (Control cntrl in grpDenialReasons.Controls)
            {
                //if control is a checkbox and it is checked then
                if (cntrl is CheckBox && (cntrl as CheckBox).Checked)
                {
                    if (cntrl.Name == "chkIncomplete")
                    {
                        if (string.IsNullOrEmpty(txtDescribe.Text))
                        {
                            Dialog.Info.Ok("You must indicate why the form is incomplete in the Describe text box.  Please try again", "Additional Info Needed");
                            return;
                        }
                        DenialReasons.Add(txtDescribe.Text);
                        continue;
                    }
                    else
                        DenialReasons.Add(cntrl.Text);
                }
            }
            if (DenialReasons.Count == 0)
            {
                Dialog.Info.Ok("You must select at least one denial reason.  Please try again.", "One Denial Reason Needed");
            }
            else if (DenialReasons.Count > 5)
            {
                Dialog.Info.Ok("You can't select more than 5 denial reasons.  Please try again.", "Too Many Denial Reasons Selected");
                DenialReasons.Clear();
            }
            else
                this.DialogResult = DialogResult.OK;
        }
    }
}