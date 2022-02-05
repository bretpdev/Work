using System.Windows.Forms;
using Uheaa.Common;

namespace ACHSETUP
{
    public partial class AddInfoDialog : Form
    {
        private ACHRecord AchRec;

        /// <summary>
        /// DO NOT USE!!!
        /// The parameterless constructor is required by the Windows Forms Designer,
        /// but it won't work with the script.
        /// </summary>
        public AddInfoDialog()
        {
            InitializeComponent();
        }

        public AddInfoDialog(ACHRecord achRec)
        {
            InitializeComponent();

            AchRec = achRec;
            aCHRecordBindingSource.DataSource = achRec;
        }

        private void Ok_Click(object sender, System.EventArgs e)
        {
            if (IsValidUserEntries())
                DialogResult = DialogResult.OK;
        }

        private void Checking_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (chkSavings.Checked == chkChecking.Checked)
                chkSavings.Checked = !chkChecking.Checked;
        }

        private void Savings_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (chkChecking.Checked == chkSavings.Checked)
                chkChecking.Checked = !chkSavings.Checked;
        }

        private void Yes_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (chkNo.Checked == chkYes.Checked)
                chkNo.Checked = !chkYes.Checked;
        }

        private void No_CheckedChanged(object sender, System.EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (chkYes.Checked == chkNo.Checked)
                chkYes.Checked = !chkNo.Checked;
        }

        private void All_CheckedChanged(object sender, System.EventArgs e)
        {
            txtLoans.ReadOnly = chkAll.Checked;
            txtLoans.Clear();
        }

        private bool IsValidUserEntries()
        {
            //ABA Routing Number
            if (AchRec.ABARoutingNumber.Length != 9)
            {
                Dialog.Warning.Ok("A 9 digit ABA routing number must be provided.  Please try again.", "Invalid Data Provided");
                return false;
            }
            if (!AchRec.ABARoutingNumber.IsNumeric())
            {
                Dialog.Warning.Ok("A numeric ABA routing number must be provided.  Please try again.", "Invalid Data Provided");
                return false;
            }
            if (AchRec.ABARoutingNumber != txtABARoutingNumberVerification.Text)
            {
                Dialog.Warning.Ok("The ABA routing number and the verification ABA routing number do not match.  Please try again.", "Invalid Data Provided");
                return false;
            }

            //Bank Account Number
            if (AchRec.BankAccountNumber.Length == 0)
            {
                Dialog.Warning.Ok("A bank account number must be provided.  Please try again.", "Invalid Data Provided");
                return false;
            }
            if (AchRec.BankAccountNumber != txtBankAcctNumberVerification.Text)
            {
                Dialog.Warning.Ok("The bank account number and the verification bank account number do not match.  Please try again.", "Invalid Data Provided");
                return false;
            }
            if (AchRec.BankAccountNumber.Length > 17)
            {
                Dialog.Warning.Ok("A bank account number can not be longer than 17 digits.  Please try again.", "Invalid Data Provided");
                return false;
            }

            //Account type
            if (!chkSavings.Checked && !chkChecking.Checked)
            {
                Dialog.Warning.Ok("An account type must be marked.  Please try again.", "Invalid Data Provided");
                return false;
            }
            else if (chkSavings.Checked)
                AchRec.AccountType = ACHRecord.BankAccountType.Savings;
            else if (chkChecking.Checked)
                AchRec.AccountType = ACHRecord.BankAccountType.Checking;

            //additional amount
            if (AchRec.AdditionalWithdrawalAmount.Length > 0)
            {
                if (!AchRec.AdditionalWithdrawalAmount.IsNumeric())
                {
                    //not numeric
                    Dialog.Warning.Ok("Any additional withdrawal amount provided must be numeric.  Please try again.", "Invalid Data Provided");
                    return false;
                }
                else //numeric
                    AchRec.AdditionalWithdrawalAmount = string.Format("{0:#####0.00}", double.Parse(AchRec.AdditionalWithdrawalAmount));
            }

            //Form signed
            if (!chkYes.Checked && !chkNo.Checked)
            {
                Dialog.Warning.Ok("Please indicate whether the user signed the form or not.  Please try again.", "Invalid Data Provided");
                return false;
            }
            if (!chkYes.Checked)
            {
                Dialog.Warning.Ok("The borrower is not eligible for ACH without a signed agreement.  Please obtain a signed agreement and run the script again.", "Form Must Be signed");
                return false;
            }

            //Loans
            if (!chkAll.Checked)
            {
                if (txtLoans.Text.Length == 0)
                {
                    Dialog.Warning.Ok("If the all loans option isn't selected you must provide the loans you want.  Please try again.", "Invalid Data Provided");
                    return false;
                }
                else
                {
                    if (AchRec.ParseLoanSequences(txtLoans.Text) == false)
                    {
                        Dialog.Warning.Ok("An invalid character was found in your list of loans.  You must enter multiple loan numbers separated by commas (01,02,04), or in ranges indicated by a hyphen (01-10), or using a combination of both (01-03,05).  Please try again.", "Invalid Data Provided");
                        return false;
                    }
                }
            }
            return true;
        }

    }
}