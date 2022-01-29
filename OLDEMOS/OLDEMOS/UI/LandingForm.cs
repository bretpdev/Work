using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using static Uheaa.Common.Dialog;

namespace OLDEMOS
{
    //HACK: The BorrowerSearchControl can not explicitly set the values of CornerstoneEnabled,UheaaEnabled,OnelinkEnabled 
    //in the designer for this form since it will consequently remove access to cornerstone for future searches
    public partial class LandingForm : BaseForm
    {
        readonly List<QuickBorrower> PreviousBorrowers = new List<QuickBorrower>();

        public LandingForm()
        {
            InitializeComponent();
            BorrowerResults.RegisterSetting<Properties.Settings>(Properties.Settings.Default, typeof(Properties.Settings).GetProperty("SearchResultsLayout"));
            BorrowerSearch.LDA = ModeHelper.LogRun.LDA;
        }

        private void AddPreviousBorrower(QuickBorrower qb)
        {
            PreviousBorrowers.Insert(0, qb);
            RefreshPreviousBorrowers();
        }

        private void RefreshPreviousBorrowers()
        {
            codeInitiated = true;
            PreviousBorrowersGrid.AutoGenerateColumns = false;
            PreviousBorrowersGrid.DataSource = null;
            PreviousBorrowersGrid.DataSource = PreviousBorrowers;
            PreviousBorrowersGrid.ClearSelection();
            codeInitiated = false;
        }

        private void BorrowerSearch_OnSearchResultsRetrieved(BorrowerSearchControl sender, List<QuickBorrower> results)
        {
            codeInitiated = true;
            BorrowerResults.SetResults(results);
            codeInitiated = false;
        }

        bool codeInitiated = false;
        private void BorrowerResults_OnSelectionChanged(object sender, QuickBorrower selected)
        {
            if (codeInitiated)
                return;
            PreviousBorrowersGrid.ClearSelection();
            if (selected != null)
                AccIdBox.Text = selected.SSN;
            else
                AccIdBox.Text = null;
        }

        private void BorrowerResults_OnBorrowerChosen(object sender, QuickBorrower selected)
        {
            AddPreviousBorrower(selected);
            LoadBorrower(selected.SSN, false);
        }

        public void LoadBorrower(string acctId, bool addPrevious)
        {
            try
            {
                Borrower bor = new Borrower(Helper.RI, acctId);
                if (bor != null && bor.AccountNumber.IsPopulated())
                {
                    AccIdBox.Text = "";

                    this.Hide();
                    using Demographics demos = new Demographics(bor);
                    demos.ShowDialog();
                    if (addPrevious && bor != null)
                    {
                        QuickBorrower qb = new QuickBorrower();
                        qb.RegionEnum = RegionSelectionEnum.Uheaa;
                        qb.SSN = bor.Ssn;
                        qb.FirstName = bor.FirstName;
                        qb.MiddleInitial = bor.MiddleInitial;
                        qb.LastName = bor.LastName;
                        AddPreviousBorrower(qb);
                    }
                    if (this.IsHandleCreated)
                    {
                        //Blank out the results when returning to the main menu
                        BorrowerResults.SetResults(new List<QuickBorrower>());
                        BorrowerSearch.Reset(); //This will invoke the BorrowerSearch_SearchCleared
                        this.Show();
                    }
                }
            }
            catch (Exception)
            {
                Error.Ok($"There was no borrower found in the session for: {AccIdBox.Text}.", "Borrower Not Found");
                AccIdBox.Text = "";
            }
        }

        private void AccIdBox_TextChanged(object sender, EventArgs e)
        {
            GoButton.Enabled = AccIdBox.Text.Length >= 9;
            if (AccIdBox.Text.Length == 9)
                SsnAcctLabel.Text = "SSN";
            else if (AccIdBox.Text.Length == 10)
                SsnAcctLabel.Text = "Account #";
            else
                SsnAcctLabel.Text = "SSN or Acct #";
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            LoadBorrower(AccIdBox.Text, true);
        }

        private void PreviousBorrowersGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (PreviousBorrowersGrid.SelectedRows.Count > 0)
            {
                QuickBorrower qb = PreviousBorrowers[PreviousBorrowersGrid.SelectedRows[0].Index];
                PreviousBorrowers.Remove(qb);
                AddPreviousBorrower(qb);
                LoadBorrower(qb.SSN, false);
            }
        }

        private void PreviousBorrowersGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (!codeInitiated)
                PreviousBorrowersGrid_Activity();
        }

        private void PreviousBorrowersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PreviousBorrowersGrid_Activity();
        }

        private void PreviousBorrowersGrid_Activity()
        {
            if (PreviousBorrowersGrid.SelectedRows.Count > 0)
            {
                QuickBorrower qb = PreviousBorrowers[PreviousBorrowersGrid.SelectedRows[0].Index];
                AccIdBox.Text = qb.SSN;
                BorrowerResults.ClearSelection();
            }
        }

        private void PreviousBorrowersGrid_Leave(object sender, EventArgs e)
        {
            PreviousBorrowersGrid.ClearSelection();
        }

        private void BorrowerResults_Enter(object sender, EventArgs e)
        {
            PreviousBorrowersGrid.ClearSelection();
        }

        private void PreviousBorrowersGrid_Enter(object sender, EventArgs e)
        {
            BorrowerResults.ClearSelection();
        }

        private void BorrowerSearch_SearchCleared(object sender, EventArgs e)
        {
            AccIdBox.Text = null;
            BorrowerResults.SetResults(new List<QuickBorrower>());
        }

        private void LandingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ModeHelper.LogRun.LogEnd();
                Helper.RI.CloseSession();
            }
        }
    }
}