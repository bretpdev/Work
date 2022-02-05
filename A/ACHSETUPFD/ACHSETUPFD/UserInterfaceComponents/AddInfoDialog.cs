using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUPFD
{
    public partial class AddInfoDialog : Form
    {
        private ACHRecord AchRec { get; set; }
        private DataAccess DA { get; set; }

        public AddInfoDialog(ACHRecord achRec, List<DataClasses.EndorserRecord> endorsers, DataAccess da)
        {
            InitializeComponent();

            AchRec = achRec;
            aCHRecordBindingSource.DataSource = achRec;
            AchRec.IsEndorser = ACHRecord.EndorserStatus.No;
            if(endorsers.Count == 0)
            {
                isEndorserYes.Enabled = false;
                isEndorserNo.Enabled = false;
                label2.Enabled = false;
                AchRec.IsEndorser = ACHRecord.EndorserStatus.NA;
            }
            DA = da;
        }

        /// <summary>
        /// Ok Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (IsValidUserEntries())
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Changes between checking and savings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkChecking_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (chkSavings.Checked == chkChecking.Checked)
                chkSavings.Checked = !chkChecking.Checked;
            else
                chkChecking.Checked = !chkSavings.Checked;
        }

        /// <summary>
        /// Changes between yes and no for Form Signed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkYes_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (chkNo.Checked == chkYes.Checked)
                chkNo.Checked = !chkYes.Checked;
            else
                chkYes.Checked = !chkNo.Checked;
        }

        /// <summary>
        /// Checks all loans sequences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_CheckedChanged(object sender, System.EventArgs e)
        {
            txtLoans.ReadOnly = chkAll.Checked;
            txtLoans.Clear();
        }

        /// <summary>
        /// Checks all fields to make sure data is provided.
        /// </summary>
        /// <returns></returns>
        private bool IsValidUserEntries()
        {
            //ABA Routing Number
            if (!CheckABARoutingNumber())
                return false;

            //Bank Account Number
            if (!CheckBankAccountNumber())
                return false;

            //Account type
            if (!CheckAccountType())
                return false;

            //EFT Source will always be paper.
            AchRec.EFT = ACHRecord.EFTSource.Paper;

            //additional amount
            if (!CheckAdditionalAmount())
                return false;

            //Form signed
            if (!CheckFormSigned())
                return false;

            //Loans
            if (!CheckLoans())
                return false;

            //Endorsers
            if(!CheckEndorsers())
                return false; 

            return true;

        }

        /// <summary>
        /// Validates the ABA Routing number
        /// </summary>
        /// <returns></returns>
        private bool CheckABARoutingNumber()
        {
            if (AchRec.ABARoutingNumber.Length != 9)
            {
                MessageBox.Show("A 9 digit ABA routing number must be provided.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!AchRec.ABARoutingNumber.IsNumeric())
            {
                MessageBox.Show("A numeric ABA routing number must be provided.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (AchRec.ABARoutingNumber != txtABARoutingNumberVerification.Text)
            {
                MessageBox.Show("The ABA routing number and the verification ABA routing number do not match.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (DA.IsInvalidValue("ABA_Routing", AchRec.ABARoutingNumber))
            {
                MessageBox.Show("The ABA routing number is not valid.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates the Bank Account number
        /// </summary>
        /// <returns></returns>
        private bool CheckBankAccountNumber()
        {
            if (AchRec.BankAccountNumber.Length == 0)
            {
                MessageBox.Show("A bank account number must be provided.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (AchRec.BankAccountNumber != txtBankAcctNumberVerification.Text)
            {
                MessageBox.Show("The bank account number and the verification bank account number do not match.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates the Check Account type
        /// </summary>
        /// <returns></returns>
        private bool CheckAccountType()
        {
            if (!chkSavings.Checked && !chkChecking.Checked)
            {
                MessageBox.Show("An account type must be marked.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (chkSavings.Checked)
            {
                AchRec.AccountType = ACHRecord.BankAccountType.Savings;
            }
            else if (chkChecking.Checked)
            {
                AchRec.AccountType = ACHRecord.BankAccountType.Checking;
            }
            return true;
        }

        /// <summary>
        /// Validates the Additional Amount
        /// </summary>
        /// <returns></returns>
        private bool CheckAdditionalAmount()
        {
            if (AchRec.AdditionalWithdrawalAmount.Length > 0)
            {
                if (!AchRec.AdditionalWithdrawalAmount.IsNumeric())
                {
                    //not numeric
                    MessageBox.Show("Any additional withdrawal amount provided must be numeric.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    //numeric
                    AchRec.AdditionalWithdrawalAmount = string.Format("{0:#####0.00}", double.Parse(AchRec.AdditionalWithdrawalAmount));
                }
            }
            return true;
        }

        /// <summary>
        /// Validates that the form was signed
        /// </summary>
        /// <returns></returns>
        private bool CheckFormSigned()
        {
            if (!chkYes.Checked && !chkNo.Checked)
            {
                MessageBox.Show("Please indicate whether the user signed the form or not.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!chkYes.Checked)
            {
                MessageBox.Show("The borrower is not eligible for ACH without a signed agreement.  Please obtain a signed agreement and run the script again.", "Form Must Be signed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates the loans were selected
        /// </summary>
        /// <returns></returns>
        private bool CheckLoans()
        {
            if (!chkAll.Checked)
            {
                if (txtLoans.Text.Length == 0)
                {
                    MessageBox.Show("You must either select 'All' loans or provide the loans manually.  Please try again.", "Loan Data Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    if (AchRec.ParseLoanSequences(txtLoans.Text) == false)
                    {
                        MessageBox.Show("An invalid character was found in your list of loans.  You must enter multiple loan numbers separated by commas (01,02,04), or in ranges indicated by a hyphen (01-10), or using a combination of both (01-03,05).  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckEndorsers()
        {
            if (AchRec.IsEndorser != ACHRecord.EndorserStatus.NA)
            {
                if (!isEndorserYes.Checked && !isEndorserNo.Checked)
                {
                    MessageBox.Show("Please indicate whether the user  is an endorser or not.  Please try again.", "Invalid Data Provided", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (isEndorserYes.Checked)
                {
                    AchRec.IsEndorser = ACHRecord.EndorserStatus.Yes;
                }
                else
                {
                    AchRec.IsEndorser = ACHRecord.EndorserStatus.No;
                }
            }
            return true;
        }

        private void isEndorserYes_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (isEndorserNo.Checked == isEndorserYes.Checked)
                isEndorserNo.Checked = !isEndorserYes.Checked;
            else
                isEndorserYes.Checked = !isEndorserNo.Checked;
        }
    }
}
