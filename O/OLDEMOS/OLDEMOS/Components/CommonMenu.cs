using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.Dialog;

namespace OLDEMOS
{
    public class CommonMenu : MenuStrip
    {
        public enum ContrastModeEnum
        {
            Normal,
            HighContrast,
            Professional
        }

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
            BuildContrastMenu();
            BuildIncidentReportingMenu();

            Sync();
        }

        private void BuildInfoMenu()
        {
            var infoItem = new ToolStripMenuItem("Info");
            infoItem.Alignment = ToolStripItemAlignment.Right;
            infoItem.Size = new Size(150, 50);
            this.Items.Add(infoItem);

            var versionItem = new ToolStripLabel($"[version: {Assembly.GetExecutingAssembly().GetName().Version}]");
            if (TestHelper.IsTesting && TestHelper.NewVersionAvailable)
            {
                versionItem.ForeColor = Color.Red;
                versionItem.Text += "!";
                versionItem.Click += (o, ea) =>
                {
                    if (Warning.YesNo("Are you sure you want to automatically update your test copy to version " + TestHelper.CurrentTestVersion + "?  This will log you out of your current session."))
                        TestHelper.Update();
                };
            }
            infoItem.DropDownItems.Add(versionItem);
            var info = WarehouseInfo.RetrieveWarehouseInfo();
            ToolStripLabel whsLbl = new ToolStripLabel("Last UHEAA warehouse update: " + (info.UdwRefresh.HasValue ? info.UdwRefresh.Value.ToShortTimeString() : "Unknown"));
            whsLbl.Width = 200;
            infoItem.DropDownItems.Add(whsLbl);
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

        private void Sync()
        {
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
                bf.ApplyContrast();
        }

        private void LaunchIncidentReporting(string ticketType)
        {
            string args = "--ticketType " + ticketType + " --region UHEAA";
            if (DataAccessHelper.TestMode) args += " --dev";
            Proc.Start("IncidentReporting", args);
        }
    }
}