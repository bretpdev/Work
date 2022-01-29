using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace GitCloner
{
    public partial class MainForm : Form
    {
        Properties.Settings settings = Properties.Settings.Default;
        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        bool loadingSettings = false;
        private void LoadSettings()
        {
            loadingSettings = true;
            ApiKeyBox.Text = settings.ApiKey;
            CloneLocationBox.Text = settings.CloneLocation;
            CleanBox.Checked = settings.CleanFirst;
            BranchesBox.Checked = settings.IncludeBranches;
            loadingSettings = false;
        }

        private void Setting_Changed(object sender, EventArgs e)
        {
            if (loadingSettings)
                return;
            settings.ApiKey = ApiKeyBox.Text;
            settings.CloneLocation = CloneLocationBox.Text;
            settings.CleanFirst = CleanBox.Checked;
            settings.IncludeBranches = BranchesBox.Checked;

            settings.Save();
        }
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFolderDialog();
            if (ofd.ShowDialog(this.Handle, false) == DialogResult.OK)
                CloneLocationBox.Text = ofd.Folder;
        }

        private void CloneButton_Click(object sender, EventArgs e)
        {
            var log = new ProgressLog(ProgressBox, this);
            var cloner = new Cloner(log, settings.ApiKey, settings.CloneLocation, settings.CleanFirst, settings.IncludeBranches);
            SetEnabled(false);
            Task.Run(new Action(() =>
            {
                cloner.Clone();
                SetEnabled(true);
            }));
        }

        private void SetEnabled(bool enabled)
        {
            this.BeginInvoke(new Action(() =>
            {
                ApiKeyBox.Enabled = CloneLocationBox.Enabled = CleanBox.Enabled = BranchesBox.Enabled = CloneButton.Enabled = enabled;
            }));
        }

        private void ManageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.uheaa.org/settings/tokens");
        }
    }
}
