using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace IDRUSERPRO
{
    public partial class AccountEntry : Form
    {
        private DataAccess DA { get; set; }
        public string AccountNumber { get; set; }
        public string SSN { get; set; }
        public bool MisroutedApp { get; set; }
        public bool NewApp { get; set; }
        public bool NoBalance { get; set; }
        public SystemBorrowerDemographics Demo { get; set; }
        public List<Ts26Loans> Loans { get; set; }
        public IdentifiedApplication SelectedApp { get; set; }
        private ReflectionInterface RI { get; set; }
        private List<IdentifiedApplication> Apps { get; set; }
        public bool FirstTimeApp { get; set; }

        private enum BorrowerSearchResults
        {
            BorrowerFound,
            EndorserFound,
            NoResult,
            MisroutedApp
        }

        public AccountEntry(ReflectionInterface ri, DataAccess da)
        {
            InitializeComponent();
            RI = ri;
            Apps = new List<IdentifiedApplication>();
            DA = da;
        }

        /// <summary>
        /// Checks to see if an inputed account number is an endorser
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns>True if the account number is an endorser</returns>
        private bool CheckEndorser(string accountNumber)
        {
            RI.FastPath("TX3Z/ITX1J*");
            RI.PutText(5, 16, "", true);
            RI.PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);
            return RI.CheckForText(1, 71, "TXX1R-02");
        }

        private bool GetLoans()
        {
            var results = new Ts26Results(RI);
            results.LoadLoanDataFromTs26(AccountNumber, MisroutedApp);

            if (results.Status == Ts26Status.ZeroBalance)
                if (MessageBox.Show("The borrower does not have a loan with a balance.  Do you want to continue?", "Zero Balance", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    NoBalance = true;
                else
                    return false;

            if (results.Status == Ts26Status.BorrowerNotFound)
            {
                MessageBox.Show("The borrower was not found on the system.  Please review and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Loans = results.Loans.ToList();
            if (Loans == null)
                return false;

            return true;
        }

        private BorrowerSearchResults CheckAccountIdentifier()
        {
            accountIdentifierText.Text = accountIdentifierText.Text.Trim();
            if (accountIdentifierText.Text.Length >= 9)
            {
                if (accountIdentifierText.Text.Length == 10)
                    AccountNumber = accountIdentifierText.Text;
                else
                {
                    SSN = accountIdentifierText.Text;
                    AccountNumber = string.Empty;
                }

                if (cbMisroutedApp.Checked)
                    return BorrowerSearchResults.MisroutedApp;

                if (!LoadDemos()) //If demos not loaded, return
                    return BorrowerSearchResults.NoResult;

                if (CheckEndorser(Demo.AccountNumber))
                {
                    MessageBox.Show("Account number/SSN belongs to an Endorser. Review is needed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return BorrowerSearchResults.EndorserFound;
                }
                return BorrowerSearchResults.BorrowerFound;
            }
            return BorrowerSearchResults.NoResult;
        }

        private bool LoadDemos()
        {
            try
            {
                //Get the account number with the ssn entered by the user.
                if (AccountNumber.IsNullOrEmpty())
                {
                    Demo = RI.GetDemographicsFromTx1j(SSN);
                    AccountNumber = Demo.AccountNumber;
                }
                else
                {
                    Demo = RI.GetDemographicsFromTx1j(AccountNumber);
                    SSN = Demo.Ssn;
                }
            }
            catch (DemographicException)
            {
                return false;
            }
            return true;
        }

        private void cbMisroutedApp_CheckedChanged(object sender, EventArgs e)
        {
            SyncDisplay();
            CheckAccountIdentifier();
        }

        private void SyncDisplay()
        {
            if (cbMisroutedApp.Checked)
            {
                lblAcctNum.Text = "SSN";
                accountIdentifierText.MaxLength = 9;
            }
            else
            {
                accountIdentifierText.MaxLength = 10;
                if (accountIdentifierText.Text.Length == 10)
                {
                    lblAcctNum.Text = "Account Number";
                    cbMisroutedApp.Checked = false;
                    cbMisroutedApp.Enabled = false;
                }
                else
                {
                    lblAcctNum.Text = "Account Identifier";
                    cbMisroutedApp.Enabled = true;
                }
            }
        }

        private void NewApplication_Click(object sender, EventArgs e)
        {
            NewApp = true;
            if (!GetLoans())
                return;
            this.DialogResult = DialogResult.OK;
        }

        private void ExistingApplications_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Apps.Count <= ExistingApplications.CurrentRow.Index)
                return;
            SelectedApp = Apps[ExistingApplications.CurrentRow.Index];
            if (!GetLoans())
                return;

            DialogResult = DialogResult.OK;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            SyncDisplay();
            MisroutedApp = cbMisroutedApp.Checked;
            BorrowerSearchResults accountResult = CheckAccountIdentifier();
            if (accountResult == BorrowerSearchResults.NoResult)
            {
                List<BorrowerNotFound> notFound = new List<BorrowerNotFound>();
                Dialog.Error.Ok("Account Number/SSN not found", "Borrower Not Found");
                notFound.Add(new BorrowerNotFound() { Borrower_Not_Found = "Account Number/SSN not found" });
                ExistingApplications.DataSource = notFound;
                return;
            }
            else if (accountResult != BorrowerSearchResults.EndorserFound)
            {
                ExistingApplications.DataSource = null; //Clear the results first
                Apps = DA.GetIdentifiedApplication(SSN);
                if (!Apps.Any())
                    FirstTimeApp = true;
                ExistingApplications.DataSource = Apps;
                ExistingApplications.Columns["Account_Identifier"].Visible = false;
                NewApplication.Enabled = true;
            }
        }

        private void accountIdentifierText_TextChanged(object sender, EventArgs e)
        {
            ExistingApplications.DataSource = null;
            ExistingApplications.Refresh();
            SyncDisplay();
        }
    }
}