using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MD
{
    public class CommonMenu : MenuStrip
    {
        private ToolStripMenuItem DemographicsOnlyMenu;
        public static bool DemographicsOnly { get; private set; }

        public static ContrastModeEnum ContrastMode
        {
            get { return (ContrastModeEnum)Properties.Settings.Default.ContrastMode; }
            set
            {
                Properties.Settings.Default.ContrastMode = (int)value;
                Properties.Settings.Default.Save();
            }
        }
        private ToolStripMenuItem ContrastModeMenu;
        private ToolStripMenuItem ContrastNormalMenu;
        private ToolStripMenuItem ContrastHighMenu;
        private ToolStripMenuItem ContrastProfessionalMenu;
        public CommonMenu()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                Init();
        }

        private ToolStripMenuItem NewItem(string text = null) { return (ToolStripMenuItem)this.Items.Add(text); }
        private void Init()
        {
            BuildInfoMenu();
            BuildDemographicsOnlyMenu();
            BuildContrastMenu();
            BuildIncidentReportingMenu();
            BuildTrainingMenu();
            BuildFeedbackMenu();

            Sync();
        }

        private void BuildInfoMenu()
        {
            var infoItem = new ToolStripMenuItem("Info");
            infoItem.Alignment = ToolStripItemAlignment.Right;
            this.Items.Add(infoItem);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var versionItem = new ToolStripLabel("[v{0}.{1}.{2}]".FormatWith(version.Major, version.Minor, version.Build));
            if (TestHelper.IsTesting && TestHelper.NewVersionAvailable)
            {
                versionItem.ForeColor = Color.Red;
                versionItem.Text += "!";
                versionItem.Click += (o, ea) =>
                {
                    if (Dialog.Warning.YesNo("Are you sure you want to automatically update your test copy to version " + TestHelper.CurrentTestVersion + "?  This will log you out of your current session."))
                        TestHelper.Update();
                };
            }
            infoItem.DropDownItems.Add(versionItem);
            var info = WarehouseInfo.RetrieveWarehouseInfo();
            infoItem.DropDownItems.Add(new ToolStripLabel("Last UHEAA warehouse update: " + (info.UdwRefresh.HasValue ? info.UdwRefresh.Value.ToShortTimeString() : "Unknown")));
            infoItem.DropDownItems.Add(new ToolStripLabel("Last CornerStone warehouse update: " + (info.CdwRefresh.HasValue ? info.CdwRefresh.Value.ToShortTimeString() : "Unknown")));
        }

        private void BuildDemographicsOnlyMenu()
        {
            DemographicsOnlyMenu = NewItem();
            DemographicsOnlyMenu.Alignment = ToolStripItemAlignment.Right;
            DemographicsOnlyMenu.Click += (o, ea) =>
            {
                DemographicsOnly = !DemographicsOnly;
                Sync();
            };
            if (!Hlpr.Login.IsLoggedIn)
                DemographicsOnlyMenu.Visible = false;
            else if (Hlpr.Login.LoginMode == LoginMode.Calls)
                DemographicsOnlyMenu.Visible = false;
        }

        private void BuildContrastMenu()
        {
            ContrastModeMenu = NewItem("Contrast");
            ContrastModeMenu.Alignment = ToolStripItemAlignment.Right;
            ContrastNormalMenu = (ToolStripMenuItem)ContrastModeMenu.DropDownItems.Add("Default");
            ContrastNormalMenu.Click += (o, ea) =>
            {
                ContrastMode = ContrastModeEnum.Normal;
                Sync();
                ApplyContrast();
            };
            ContrastHighMenu = (ToolStripMenuItem)ContrastModeMenu.DropDownItems.Add("High");
            ContrastHighMenu.Click += (o, ea) =>
            {
                ContrastMode = ContrastModeEnum.HighContrast;
                Sync();
                ApplyContrast();
            };
            ContrastProfessionalMenu = (ToolStripMenuItem)ContrastModeMenu.DropDownItems.Add("Professional");
            ContrastProfessionalMenu.Click += (o, ea) =>
            {
                ContrastMode = ContrastModeEnum.Professional;
                Sync();
                ApplyContrast();
            };
        }

        private void BuildIncidentReportingMenu()
        {
            var incidentReportingMenu = NewItem("Incident Reporting");
            incidentReportingMenu.DropDownItems.Add("Physical Threat").Click += (o, ea) => LaunchIncidentReporting("Threat");
            incidentReportingMenu.DropDownItems.Add("Security Incident").Click += (o, ea) => LaunchIncidentReporting("Incident");
        }

        private void BuildTrainingMenu()
        {
            var training = NewItem("Training");
            training.DropDownItems.Add("FAQ").Click += (o, ea) => FaqViewerForm.ShowViewer();
            training.DropDownItems.Add("Training Modules").Click += (o, ea) =>
            {
                new Thread(() =>
                {
                    Application.Run(new TrainingModulesForm());
                }).Start();
            };
            if (AccessHelper.UserIsFaqAdmin)
            {
                training.DropDownItems.Add("-");
                training.DropDownItems.Add("Manage FAQ").Click += (o, ea) => FaqAdminHomeForm.ShowAdmin();
            }
        }

        private void BuildFeedbackMenu()
        {
            var feedback = NewItem("Feedback");

            feedback.DropDownItems.Add("Bug Report").Click += (o, ea) => 
                Hlpr.UI.CreateAndShowDialog<FeedbackForm>((f) => f.Initialize((Form)this.Parent, FeedbackTypeEnum.Bug));
            feedback.DropDownItems.Add("Feature Request").Click += (o, ea) =>
                Hlpr.UI.CreateAndShowDialog<FeedbackForm>((f) => f.Initialize((Form)this.Parent, FeedbackTypeEnum.Feature));

            if (AccessHelper.UserIsFaqAdmin)
            {
                feedback.DropDownItems.Add("-");
                feedback.DropDownItems.Add("Manage Feedback Requests").Click += (o, ea) => Hlpr.UI.CreateAndShowDialog<FeedbackViewer>();
            }
        }

        private bool LoggedIn
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                    return LoginHelper.Instance.IsLoggedIn;
                return false;
            }
        }

        private void Sync()
        {
            DemographicsOnlyMenu.Text = "Demographics Only: ";
            if (DemographicsOnly)
                DemographicsOnlyMenu.Text += "ON";
            else
                DemographicsOnlyMenu.Text += "OFF";

            ContrastNormalMenu.Checked = ContrastHighMenu.Checked = ContrastProfessionalMenu.Checked = false;
            switch (ContrastMode)
            {
                case ContrastModeEnum.Normal:
                    ContrastNormalMenu.Checked = true;
                    break;
                case ContrastModeEnum.HighContrast:
                    ContrastHighMenu.Checked = true;
                    break;
                case ContrastModeEnum.Professional:
                    ContrastProfessionalMenu.Checked = true;
                    break;
                default:
                    throw new Exception("Fatal Contrast Mode Enum exception.");
            }
        }

        private void ApplyContrast()
        {
            BaseForm bf = this.Parent as BaseForm;
            if (bf != null)
            {
                bf.ApplyContrast();
            }
        }

        private void LaunchIncidentReporting(string ticketType)
        {
            string args = "--ticketType " + ticketType + " --region UHEAA";
            if (DataAccessHelper.TestMode) args += " --dev";
            Proc.Start("IncidentReporting", args);
        }
    }
}
