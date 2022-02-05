using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;
using Uheaa.Common.Scripts;
using System.Threading;
using System.DirectoryServices;
using Uheaa.Common;
using MauiDUDE; //new c# MauiDUDE
using Uheaa.Common.ProcessLogger;
using System.Reflection;

namespace MD
{
    //HACK: The BorrowerSearchControl can not explicitly set the values of CornerstoneEnabled,UheaaEnabled,OnelinkEnabled 
    //in the designer for this form since it will consequently remove access to cornerstone for future searches
    public partial class LandingForm : BaseForm
    {
        List<QuickBorrower> previousBorrowers = new List<QuickBorrower>();
        public LandingForm()
        {
            InitializeComponent();
            BorrowerResults.RegisterSetting<Properties.Settings>(Properties.Settings.Default, typeof(Properties.Settings).GetProperty("SearchResultsLayout"));
            BorrowerSearch.LDA = new Uheaa.Common.ProcessLogger.LogDataAccess(DataAccessHelper.CurrentMode, ModeHelper.ProcessLogId, true, true);
        }

        private void LoadBorrowerInSession(string accId)
        {
            Hlpr.RI.Stup(DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            if (Hlpr.RI.CheckForText(3, 7, "This computer is"))
                Hlpr.RI.Hit(Uheaa.Common.Scripts.ReflectionInterface.Key.F10);
            SystemBorrowerDemographics demos;
            try
            {
                demos = Hlpr.RI.GetDemographicsFromTx1j(accId);
            }
            catch (DemographicException)
            {
                Hlpr.RI.Stup(DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
                if (Hlpr.RI.CheckForText(3, 7, "This computer is"))
                    Hlpr.RI.Hit(Uheaa.Common.Scripts.ReflectionInterface.Key.F10);
                try
                {
                    demos = Hlpr.RI.GetDemographicsFromTx1j(accId);
                }
                catch (DemographicException)
                {
                    return;
                }
            }

            Hlpr.RI.FastPath("tx3z/its24" + demos.Ssn);
        }

        private void AddPreviousBorrower(QuickBorrower qb)
        {
            previousBorrowers.Insert(0, qb);
            RefreshPreviousBorrowers();
        }
        private void RefreshPreviousBorrowers()
        {
            codeInitiated = true;
            PreviousBorrowersGrid.AutoGenerateColumns = false;
            PreviousBorrowersGrid.DataSource = null;
            PreviousBorrowersGrid.DataSource = previousBorrowers;
            PreviousBorrowersGrid.ClearSelection();
            codeInitiated = false;
        }

        private void BorrowerSearch_OnSearchResultsRetrieved(Uheaa.Common.WinForms.BorrowerSearchControl sender, List<QuickBorrower> results)
        {
            codeInitiated = true;
            BorrowerResults.SetResults(results);
            codeInitiated = false;
        }

        bool codeInitiated = false;
        private void BorrowerResults_OnSelectionChanged(object sender, QuickBorrower selected)
        {
            if (codeInitiated) return;
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

        public void LoadBorrower(string accountIdentifier, bool addPrevious)
        {
            //set data access defaulting to log in the uheaa region
            MauiDUDE.DataAccess.DA = new MauiDUDE.DataAccess(ModeHelper.GetProcessLogRun(DataAccessHelper.Region.Uheaa));

            string ssn = accountIdentifier.Length == 9 ? accountIdentifier : null;

            if (ssn == null)
            {
                try
                {
                    ssn = ssn = MauiDUDE.DataAccess.DA.GetSSNFromAccountNumber(accountIdentifier, DataAccessHelper.Region.Uheaa);
                }
                catch (Exception)
                {
                }
            }
            if (ssn != null)
            {
                var helper = new ManualAlertHelper();
                var alerts = helper.GetAlerts(ssn);
                if (alerts.Any())
                {
                    MessageBox.Show(string.Join(Environment.NewLine + Environment.NewLine, alerts.Select(o => o.Alert).ToArray()));
                    if (alerts.Any(o => o.AbortAfterMessageDisplay))
                    {
                        AccIdBox.Text = ""; //clear search box
                        return;
                    }
                }
            }
            LoadBorrowerInSession(accountIdentifier);
            AccIdBox.Text = "";
            MauiDUDE.FeedbackLinker.FeatureRequestAction = (form) =>
                Hlpr.UI.CreateAndShowDialog<FeedbackForm>((f) => f.Initialize(form, FeedbackTypeEnum.Feature), form);
            MauiDUDE.FeedbackLinker.BugReportAction = (form) =>
                Hlpr.UI.CreateAndShowDialog<FeedbackForm>((f) => f.Initialize(form, FeedbackTypeEnum.Bug), form);
            MauiDUDE.FaqLinker.ShowFaq = (form) => FaqViewerForm.ShowViewer();
            MauiDUDE.FaqLinker.ShowTraining = (form) =>
            {
                new Thread(() =>
                {
                    Application.Run(new TrainingModulesForm());
                }).Start();
            };

            MauiDUDE.SessionInteractionComponents.ProcessLogId = ModeHelper.ProcessLogId;
            MauiDUDE.SessionInteractionComponents.InstantiateVariables(Hlpr.RI, ModeHelper.GetProcessLogRun(DataAccessHelper.Region.Uheaa));
            MauiDUDE.SessionInteractionComponents.KillReflection = () => Hlpr.RH.Kill();
            var flow = new HomePageAndDemographicsOnlyFlow(
                CommonMenu.DemographicsOnly, Hlpr.Login.LoginMode == LoginMode.Calls,
                new ReflectionInterface(Hlpr.RI.ReflectionSession));
            flow.SSN = accountIdentifier;
            this.Hide();
            MauiDUDE.MainMenuFlowCoordinator.Coordinate(flow); //this runs the majority of the lower level code
            if (addPrevious)
            {
                QuickBorrower qb = new QuickBorrower();
                Borrower b = null;
                qb.RegionEnum = RegionSelectionEnum.Uheaa;
                b = flow._uheaaBorrower;

                if (b != null)
                {
                    qb.SSN = b.SSN;
                    qb.FirstName = b.FirstName;
                    qb.MiddleInitial = b.MI;
                    qb.LastName = b.LastName;
                    AddPreviousBorrower(qb);
                }
            }
            if (this.IsHandleCreated)
            {
                //Blank out the results when returning to the main menu
                BorrowerResults.SetResults(new List<QuickBorrower>());
                BorrowerSearch.Reset(); //This will invoke the BorrowerSearch_SearchCleared
                this.Show();
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
                QuickBorrower qb = previousBorrowers[PreviousBorrowersGrid.SelectedRows[0].Index];
                previousBorrowers.Remove(qb);
                AddPreviousBorrower(qb);
                LoadBorrower(qb.SSN, false);
            }
        }

        private void FaqButton_Click(object sender, EventArgs e)
        {
            FaqAdminHomeForm.ShowAdmin();
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
                QuickBorrower qb = previousBorrowers[PreviousBorrowersGrid.SelectedRows[0].Index];
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
    }
}
