using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    public partial class Dashboard : Form
    {
        static Dashboard dashboard;
        public static Dashboard Instance { get { if (dashboard == null) dashboard = new Dashboard(); return dashboard; } }
        const string dateFormat = "MM/dd/yyyy hh:mm tt";
        int tallHeight = 0;
        int shortHeight = 409;
        public ProcessLogRun PLR { get; set; }
        List<RunHistoryDetail> history = null;

        private Dashboard()
        {
            InitializeComponent();
            tallHeight = this.Height;
            StartDate.Value = DateTime.Now.AddDays(-7);
            SyncDisplay();
            PLR = Program.PLR;
        }

        
        public void LoadRunHistory()
        {
            history = RunHistoryDetail.GetReport(StartDate.Enabled ? StartDate.Value : (DateTime?)null, EndDate.Enabled ? EndDate.Value : (DateTime?)null, IncludeEmptyButton.SelectedValue);
            RunHistoryList.DataSource = history.Select(o =>
                $"{(o.EndedOn ?? o.StartedOn).ToString(dateFormat)} ({o.TotalFiles} files, {o.InvalidFiles} invalid, {o.RunBy})").ToArray();
            var last = RunHistory.GetLastRunHistory();
            LastRunText.Text = last == null ? "Never" : (last.EndedOn ?? last.StartedOn).ToString(dateFormat);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void ShowToolTip(string title, string message, ToolTipIcon icon)
        {
            NotifyIcon ni = new NotifyIcon();
            ni.BalloonTipIcon = icon;
            ni.Icon = Properties.Resources.key;
            ni.Visible = true;
            EventHandler close = (o, ea) =>
            {
                ni.Dispose();
            };
            ni.BalloonTipClosed += close;
            ni.BalloonTipClicked += close;
            ni.MouseClick += (o, ea) => close(null, null);
            ni.BalloonTipTitle = title;
            ni.BalloonTipText = message;
            ni.ShowBalloonTip(5000);
        }

        private void RunHistoryList_DoubleClick(object sender, EventArgs e)
        {
            LoadDetails();
        }

        private void ProjectSnapshotMenu_Click(object sender, EventArgs e)
        {
            ProjectSnapshot ps = new ProjectSnapshot();
            ps.Show();
        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            Builders.SettingsForm.Build().ShowDialog();
        }

        private void ProjectsMenu_Click(object sender, EventArgs e)
        {
            if (PathType.GetAll().Any())
                new ProjectsForm().ShowDialog();
            else
                MessageBox.Show("Please add at least one Path Type before editing projects.");
        }

        private void AdvancedLabel_MouseEnter(object sender, EventArgs e)
        {
            AdvancedLabel.ForeColor = Color.Blue;
        }

        private void AdvancedLabel_MouseLeave(object sender, EventArgs e)
        {
            AdvancedLabel.ForeColor = Color.Black;
        }

        public void SyncDisplay()
        {
            bool advancedMode = AdvancedPanel.Visible;
            this.Height = advancedMode ? tallHeight : shortHeight;
            StartDate.Enabled = StartCheck.Checked;
            EndDate.Enabled = EndCheck.Checked;

            if (Settings.IsValid)
                FlagSettingsAsValid();
            else
                FlagSettingsAsInvalid();
            SyncAdvancedLabel();
            LoadRunHistory();
        }

        private void SyncAdvancedLabel()
        {
            string label = "";
            if (!EndCheck.Checked && !StartCheck.Checked)
                label = "Entire History";
            else if (EndCheck.Checked && !StartCheck.Checked)
                label = "Everything before " + EndDate.Value.ToString(dateFormat);
            else if (!EndCheck.Checked && StartCheck.Checked)
                label = "Everything after " + StartDate.Value.ToString(dateFormat);
            else
                label = StartDate.Value.ToString(dateFormat) + " to " + EndDate.Value.ToString(dateFormat);

            string append = (AdvancedPanel.Visible) ? "(-)" : "(+)";
            AdvancedLabel.Text = label + " " + append;
        }

        private void StartCheck_CheckedChanged(object sender, EventArgs e)
        {
            SyncDisplay();
        }

        private void EndCheck_CheckedChanged(object sender, EventArgs e)
        {
            SyncDisplay();
        }

        private void AdvancedLabel_Click(object sender, EventArgs e)
        {
            AdvancedPanel.Visible = !AdvancedPanel.Visible;
            SyncDisplay();
        }

        private void StartDate_ValueChanged(object sender, EventArgs e)
        {
            SyncDisplay();
        }

        private void EndDate_ValueChanged(object sender, EventArgs e)
        {
            SyncDisplay();
        }

        private void IncludeEmptyButton_Cycle(object sender)
        {
            SyncDisplay();
        }

        private void PathTypesMenu_Click(object sender, EventArgs e)
        {
            new PathTypesForm().ShowDialog();
        }

        public void FlagSettingsAsInvalid()
        {
            SettingsMenu.ForeColor = Color.Red;
        }

        public void FlagSettingsAsValid()
        {
            SettingsMenu.ForeColor = Color.Black;
        }

        private void DetailsButton_Click(object sender, EventArgs e)
        {
            LoadDetails();
        }

        private void LoadDetails()
        {
            if (RunHistoryList.SelectedIndex >= 0)
            {
                RunHistoryForm form = new RunHistoryForm(history[RunHistoryList.SelectedIndex]);
                form.Show();
            }
        }

        private void RunHistoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DetailsButton.Enabled = RunHistoryList.SelectedIndex != -1;
        }

        private void RunNowSubItem_Click(object sender, EventArgs e)
        {
            Run(true);
        }

        public void Run(bool openFolders = false)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            RunButton.Enabled = false;
            LastRunText.Text = "Now running...";
            Thread processThread = new Thread(() =>
            {
                try
                {
                    Coordinator c = new Coordinator();
                    var runHistory = c.Run(PLR);
                    List<ActivityLogDetail> details = ActivityLogDetail.GetActivityLogsByRunHistory(runHistory.RunHistoryId);
                    this.Invoke(new Action(() =>
                    {
                        int valid = details.Count(o => o.Success == "Yes");
                        int invalid = details.Count(o => o.Success == "No");
                        ShowToolTip("SFTP Run Completed",
                            "Start Time: {0:hh:mm:ss tt}\nEnd Time: {1:hh:mm:ss tt}\nValid files: {2}\nInvalid files: {3}"
                            .FormatWith(runHistory.StartedOn, DateTime.Now, valid, invalid),
                            invalid > 0 ? ToolTipIcon.Error : ToolTipIcon.Info
                        );
                    }));
                    if (openFolders)
                    {
                        HashSet<string> directories = new HashSet<string>();
                        foreach (var log in ActivityLogDetail.GetActivityLogsByRunHistory(runHistory.RunHistoryId))
                        {
                            if (File.Exists(log.SourcePath))  //some files are FTP, we can't open those locations automatically
                                directories.Add(new FileInfo(log.SourcePath).DirectoryName);
                            if (File.Exists(log.SourcePath))
                                directories.Add(new FileInfo(log.DestinationPath).DirectoryName);
                        }
                        foreach (string directory in directories)
                            if (Directory.Exists(directory)) //sometimes directories are ftp
                                Process.Start(directory);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                this.Invoke(new Action(() =>
                {
                    while (watch.ElapsedMilliseconds < 2000) ; //wait two seconds.
                    RunButton.Enabled = true;
                    LoadRunHistory();
                }));
            });
            processThread.Start();
        }
    }
}
