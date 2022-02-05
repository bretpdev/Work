using System;
using System.Windows.Forms;

namespace CSLSLTRFED
{
    public partial class AddressInfo : Form
    {
        private bool HasBorrEcor = false;
        private bool HasCoBorrEcorr = false;

        public AddressInfo(bool showEndorser, bool hasBorrEcorr, bool hasEndEcorr)
        {
            InitializeComponent();
            EnableGroups(showEndorser, hasBorrEcorr, hasEndEcorr);

            rdoBwrsCurrentAddr.Checked = false;
        }

        /// <summary>
        /// Enable each groupbox according to the borrower and endorser ecorr status
        /// </summary>
        /// <param name="showEndorser"></param>
        /// <param name="hasBorrEcorr"></param>
        /// <param name="hasEndorserEcorr"></param>
        private void EnableGroups(bool showEndorser, bool hasBorrEcorr, bool hasEndorserEcorr)
        {
            HasBorrEcor = hasBorrEcorr;
			HasCoBorrEcorr = hasEndorserEcorr;
			if (showEndorser)
				grpCoBorrower.Enabled = true;
			else
				grpCoBorrower.Enabled = false;

			if (hasBorrEcorr && !hasEndorserEcorr && showEndorser)
            {
                ecorrMessage.Text = "The borrower and endorser have different correspondence preferences. The borrower will receive E-correspondence and the co-borrower will receive a printed letter.";
                ECorrGroup.Enabled = true;
                grpBorrower.Enabled = false;
            }
			else if (!hasBorrEcorr && hasEndorserEcorr)
            {
				ecorrMessage.Text = "The borrower and endorser have different correspondence preferences. The co-borrower will receive E-correspondence and the borrower will receive a printed letter.";
                ECorrGroup.Enabled = true;
                grpBorrower.Enabled = true;
				grpCoBorrower.Enabled = false;
            }
			else if(!hasBorrEcorr && !hasEndorserEcorr)
			{
				ECorrGroup.Enabled = false;
				ecorrMessage.Text = "All applicable recipients will receive a printed letter. Please confirm that the addresses are correct.";
				grpBorrower.Enabled = true;
			}
			else if ((hasBorrEcorr && hasEndorserEcorr) || (hasBorrEcorr && !showEndorser))
            {
                ECorrGroup.Enabled = true;
                grpBorrower.Enabled = false;
				grpCoBorrower.Enabled = false;
                btnContinue.Enabled = true;
                btnClear.Enabled = true;
            }
        }

        private void rdbNotBwrCurrAddr_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (MessageBox.Show("You will be taken out of the script in order to update the borrower’s address. Is this what you would like?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DialogResult = DialogResult.Abort;
                    }
                    if(rdbNotBwrCurrAddr.Checked)
                    {
                        btnClear.Enabled = false;
                        btnContinue.Enabled = false;
                    }
                }
            }
        }

        private void rdbNotCoBwrsCurrAddr_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (MessageBox.Show("You will be taken out of the script in order to update the borrower’s address. Is this what you would like?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        DialogResult = DialogResult.Abort;
                    else if ((grpBorrower.Enabled && rdoBwrsCurrentAddr.Checked) || !grpBorrower.Enabled)
                    {
                        btnClear.Enabled = true;
                        btnContinue.Enabled = true;
                    }
                }
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("All of the form will be cleared, continue?", "Continue", MessageBoxButtons.YesNo) == DialogResult.Yes)
                DialogResult = DialogResult.Cancel;
        }

        private void rdoBwrsCurrentAddr_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if ((rdoBwrsCurrentAddr.Checked && !grpCoBorrower.Enabled) || (rdoBwrsCurrentAddr.Checked && rdbCoBwrsCurAddr.Checked))
                    {
                        btnClear.Enabled = true;
                        btnContinue.Enabled = true;
                    }
                    else
                    {
                        btnClear.Enabled = false;
                        btnContinue.Enabled = false;
                    }
                }
            }
        }

        private void rdbCoBwrsCurAddr_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb != null)
            {
                if (rb.Checked)
                {
                    if (HasBorrEcor && HasCoBorrEcorr)
                    {
                        btnClear.Enabled = true;
                        btnContinue.Enabled = true;
                    }
                    if ((!HasBorrEcor && rdoBwrsCurrentAddr.Checked) && (!HasCoBorrEcorr && rdbCoBwrsCurAddr.Checked))
                    {
                        btnClear.Enabled = true;
                        btnContinue.Enabled = true;
                    }
                    else if (HasBorrEcor && (!HasCoBorrEcorr && rdbCoBwrsCurAddr.Checked))
                    {
                        btnClear.Enabled = true;
                        btnContinue.Enabled = true;
                    }
                    else if ((!HasBorrEcor && rdoBwrsCurrentAddr.Checked) && HasCoBorrEcorr)
                    {
                        btnClear.Enabled = true;
                        btnContinue.Enabled = true;
                    }
                    else
                    {
                        btnClear.Enabled = false;
                        btnContinue.Enabled = false;
                    }
                }
            }
        }
    }
}