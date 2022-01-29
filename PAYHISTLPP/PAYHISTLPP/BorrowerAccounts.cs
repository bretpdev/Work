using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;
using static Uheaa.Common.Dialog;

namespace PAYHISTLPP
{
    public partial class BorrowerAccounts : Form
    {
        private DataAccess DA { get; set; }
        public Process Proc { get; set; } = new Process();
        private UserAccess User { get; set; }

        public BorrowerAccounts(DataAccess da)
        {
            InitializeComponent();
            DA = da;

            Version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            LoadUsers();
            LoadLenders();
        }

        private void LoadUsers()
        {
            List<UserAccess> users = DA.GetUsers();
            Users.DataSource = users;
            Users.DisplayMember = "UserName";
            Users.ValueMember = "UserAccessId";
        }

        public void LoadLenders()
        {
            List<string> lenders = DA.GetLenders();
            lenders.Insert(0, "");
            Lender.Items.AddRange(lenders.ToArray());
        }

        private void Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process.Enabled = false;
            User = (UserAccess)Users.SelectedItem ?? null;
            if (Users.SelectedIndex > 0 && Lender.SelectedIndex > 0)
                Unlockfields();
        }

        private void Lender_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process.Enabled = false;
            Tilp.Checked = Lender.Text == "971357";
            Manual.Enabled = true;
            if (Lender.SelectedIndex > 0)
                Manual.Enabled = false;
            if (Users.SelectedIndex > 0 && Lender.SelectedIndex > 0)
                Unlockfields();
        }

        public void Unlockfields()
        {
            NumberToProcess.Enabled = true;
            NumberToProcess.Text = "2500";
            Proc.InRecovery = false;
            Manual.Enabled = false;
            bool? recover = ShouldRun();
            if (recover == null)
            {
                User = null;
                Users.SelectedIndex = 0;
                return;
            }
            Process.Enabled = true;
            if (Proc.InRecovery) //Do not allow manual entry if in recovery
            {
                Proc.Accounts = DA.GetAccounts(Proc.RunId, User.UserAccessId);
                Count.Text = Proc.Accounts.Count.ToString();
                NumberToProcess.Text = Proc.Accounts.Count.ToString();
                NumberToProcess.Enabled = false;
                Tilp.Checked = Proc.Tilp;
            }
            else
                Manual.Enabled = true;
        }

        private bool? ShouldRun()
        {
            List<Process> unprocessedRun = DA.GetAllRecoveryIds(User.UserAccessId, Lender.Text);
            unprocessedRun = CheckUnprocessedAccounts(unprocessedRun);
            if (unprocessedRun.Count > 0)
            {
                if (unprocessedRun.Count > 1)
                {
                    bool? recover = Question.YesNoCancel($"There are more than 1 previous jobs that have not completed running for {User.UserName} and is in recovery. What would you like to do?\r\n\r\nYes recover:\r\nThis will recover the last job in recovery and delete all previous runs. All the borrowers in the previous runs will be restaged to be run again.\r\n\r\nNo do not recover:\r\nThis will delete all previous runs and restage all the borrowers to be run again.\r\n\r\nCancel.");
                    if (recover == null)
                        return null;
                    else if (recover.HasValue && recover.Value)
                    { //Remove the last run so it is not deleted for recovery
                        Proc = unprocessedRun[unprocessedRun.Count - 1];
                        unprocessedRun.RemoveAt(unprocessedRun.Count - 1);
                        Proc.InRecovery = true;
                        Proc.RunId = unprocessedRun[unprocessedRun.Count - 1].RunId;
                        Proc.Tilp = unprocessedRun[unprocessedRun.Count - 1].Tilp;
                    }
                    else if (!recover.HasValue && !recover.Value)
                    {
                        //Delete the unprocessed accounts and the run job
                        foreach (Process run in unprocessedRun)
                        {
                            DA.DeleteUnproceesedAccounts(run.RunId);
                            DA.DeleteRun(run.RunId);
                        }
                    }
                    return true;
                }
                else
                {
                    bool? recover = Info.YesNoCancel($"It appears the script did not complete the last time {User.UserName} ran it. Would you like to recover?\r\n\r\nYes recover:\r\nThis will start over with all the borrowers that are staged in the current process.\r\n\r\nNo:\r\nThis will delete the last run job and remove the borrowers that were in that job so they can be processed again.\r\n\r\nCancel.");
                    if (recover == null)
                        return null;
                    if (recover.HasValue && recover.Value)
                    {
                        Proc = unprocessedRun[0];
                        Proc.InRecovery = true;
                        Proc.RunId = unprocessedRun[0].RunId;
                        Proc.Tilp = unprocessedRun[0].Tilp;
                    }
                    else
                    {
                        DA.DeleteUnproceesedAccounts(unprocessedRun[0].RunId);
                        DA.DeleteRun(unprocessedRun[0].RunId);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks to see if the current run has accounts ready to process. If not, it deletes the run
        /// </summary>
        private List<Process> CheckUnprocessedAccounts(List<Process> unprocessedRun)
        {
            List<Process> accountsToCheck = new List<Process>();
            foreach (Process process in unprocessedRun)
            {
                if (DA.GetUnprocessedCount(process.RunId) > 0)
                    accountsToCheck.Add(process);
                else
                    DA.DeleteRun(process.RunId);
            }
            return accountsToCheck;
        }

        private void AccountIdentifier_TextChanged(object sender, EventArgs e)
        {
            Add.Enabled = false;
            if (AccountIdentifier.Text.IsPopulated())
                Add.Enabled = true;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (AccountIdentifier.Text.IsPopulated() && ValidateData())
            {
                if (!CheckActiveAccount().HasValue)
                    return;
                SsnList.Items.AddRange(AccountsToProcess.ToArray());
                if (AccountsToProcess.Count > 0)
                {
                    Process.Enabled = true;
                    Count.Text = $"{AccountsToProcess.Count}";
                }
                else
                {
                    Info.Ok("There are no accounts to process. Please check your accounts and try again.");
                    AccountIdentifier.Text = "";
                    Process.Enabled = false;
                }
            }
            AccountIdentifier.Text = "";
            this.Refresh();
        }

        private bool ValidateData()
        {
            if (AccountIdentifier.Text.Length.IsIn(9, 10) && NumericTextBox.ValidateInput(AccountIdentifier.Text))
                return true;
            else if (AccountIdentifier.Text.Length >= 9)
            {
                List<string> accounts = AccountIdentifier.Text.SplitAndRemoveQuotes(",");
                for (int i = 0; i < accounts.Count; i++)
                {
                    if (!NumericTextBox.ValidateInput(accounts[i]))
                    {
                        Error.Ok($"There appears to be a non numeric character in SSN: {accounts[i]} in position: {i + 1}");
                        return false;
                    }
                    if (accounts[i].Length < 9)
                    {
                        Error.Ok($"There appears to be an account that is less than 9 characters long. Account: '{accounts[i]}' in position: {i + 1}");
                        return false;
                    }
                    if (accounts[i].Length > 10)
                    {
                        Error.Ok($"There appears to be an account that is greater than 10 characters long. Account: '{accounts[i]}' in position: {i + 1}");
                        return false;
                    }
                }
            }
            return true;
        }

        private List<string> AccountsToProcess { get; set; } = new List<string>();
        private bool? CheckActiveAccount()
        {
            List<string> processedAccounts = new List<string>();
            List<string> allAccounts = AccountIdentifier.Text.SplitAndRemoveQuotes(",");
            List<string> suggestedAccounts = new List<string>();
            foreach (string acct in allAccounts)
            {
                if (acct.Length == 9)
                    suggestedAccounts.Add(acct);
                else if (acct.Length == 10)
                    suggestedAccounts.Add(DA.GetSsn(acct));
            }
            foreach (string acct in suggestedAccounts)
            {
                if (DA.CheckProcessedAccount(acct))
                    processedAccounts.Add(acct);
            }
            if (processedAccounts.Count > 0)
            {
                bool? result = Info.YesNoCancel($"There {(processedAccounts.Count > 1 ? "are" : "is")} {processedAccounts.Count} account(s) in the list that have been previously processed.\r\n\r\nYes to process them again.\r\n\r\nNo to remove them and process the rest.\r\n\r\nCancel to edit the accounts and try again.");
                if (result == null)
                    return null;
                else if (result.Value)
                    AccountsToProcess = suggestedAccounts;
                else
                    AccountsToProcess = suggestedAccounts.Except(processedAccounts).ToList();
            }
            else
                AccountsToProcess = suggestedAccounts;
            return true;
        }

        private void Process_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Validating form and accounts");
            if (!ValidateForm())
                return;
            if (IsManual && AccountsToProcess.Count == 0)
            {
                Error.Ok("No accounts have been added to the list to process.");
                return;
            }
            if (!IsManual && NumberToProcess.Text.IsNullOrEmpty())
            {
                Info.Ok("Please insert the number of records to process.");
                return;
            }
            if (Proc.RunId == 0)
                Proc.RunId = DA.StartRun(User, Tilp.Checked);
            if (IsManual && AccountsToProcess.Count > 0)
            {
                foreach (string ssn in AccountsToProcess)
                    DA.InsertSingleRecord(Proc.RunId, User.UserAccessId, ssn, Lender.Text);
            }
            else
            {
                int? count = NumberToProcess.Text.ToIntNullable();
                Count.Text = count.Value.ToString();
                if (!Proc.InRecovery && !IsManual && Lender.Text.IsPopulated() && (count.HasValue && count > 0))
                    DA.LoadAccounts(Proc.RunId, User.UserAccessId, Lender.Text, count.Value, Tilp.Checked);
            }
            if (Proc.Accounts.Count == 0)
                Proc.Accounts = DA.GetAccounts(Proc.RunId, User.UserAccessId);
            if (Proc.Accounts.Count == 0)
            {
                Info.Ok($"There were 0 borrowers available for Lender {Lender.Text}. It is possible that all the borrowers have been processed or the wrong lender was selected.");
                return;
            }
            Proc.LenderCode = Lender.Text;
            if (Proc.Accounts.Count > 0)
            {
                Console.WriteLine($"Loaded {Proc.Accounts.Count} to process");
                Proc.Tilp = Tilp.Checked;
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateForm()
        {
            string errorMessage = "Please provide the missing fields\r\n";
            bool hadError = false;
            if (Users.Text.IsNullOrEmpty())
            {
                errorMessage += "\r\nRun By";
                hadError = true;
            }
            if (Lender.Text.IsNullOrEmpty())
            {
                errorMessage += "\r\nLender";
                hadError = true;
            }
            int? count = NumberToProcess.Text.ToIntNullable();
            if (!IsManual && (!count.HasValue || count.Value <= 0))
            {
                errorMessage += "\r\nAmount to Process";
                hadError = true;
            }
            if (IsManual && SsnList.Items.Count == 0)
            {
                errorMessage += "\r\nManual account to procses";
                hadError = true;
            }

            if (hadError)
            {
                Error.Ok(errorMessage);
                return false;
            }
            return true;
        }

        private void SsnList_SelectedValueChanged(object sender, EventArgs e)
        {
            Count.Text = SsnList.Items.Count.ToString();
        }

        private bool IsManual = false;
        private void Manual_Click(object sender, EventArgs e)
        {
            AccountIdentifier.Enabled = false;
            SsnList.Enabled = false;
            SsnList.Items.Clear();
            Add.Enabled = false;
            AddLabel.Enabled = false;
            Upload.Enabled = false;
            NumberToProcess.Enabled = true;
            IsManual = !IsManual;
            AccountIdentifier.Text = "";
            Count.Text = "0";
            if (IsManual)
            {
                AccountIdentifier.Enabled = true;
                SsnList.Enabled = true;
                Add.Enabled = true;
                AddLabel.Enabled = true;
                Upload.Enabled = true;
                Manual.Text = "Auto";
                NumberToProcess.Text = "";
                NumberToProcess.Enabled = false;
            }
            else
                Manual.Text = "Manual";
            this.Refresh();
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog dialog = new OpenFileDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var fs = dialog.OpenFile();
                    using StreamReader reader = new StreamReader(fs);
                    AccountIdentifier.Text = reader.ReadToEnd();
                }
                else if (result == DialogResult.Cancel)
                    return;
            }
            catch (Exception ex)
            {
                Error.Ok($"Failed to read file, or file was empty. File should only contain comma separated ssns. Excpetion: {ex.Message}");
            }
        }
    }
}