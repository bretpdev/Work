using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace IMGHISTFED
{
    public partial class ActivityForm : Form
    {
        private int Max;
        public bool WillImage { get; set; }
        public string Folder { get { return string.Format("{0}{1}\\", EnterpriseFileSystem.TempFolder, ScriptId); } }
        public string ImagingFolder { get { return string.Format("{0}", EnterpriseFileSystem.GetPath("Imaging")); } }
        private string ScriptId;
        ActivityHistoryReport Report;
        public Thread reportThread;
        public bool IsClosed = false;
        public int CurrentCount = 0;
        public bool CheckProgress = true;
        public bool DidCancel = false;

        public ActivityForm(string scriptId, ActivityHistoryReport report)
        {
            InitializeComponent();
            ScriptId = scriptId;
            Report = report;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber.Text = string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        /// <summary>
        /// Sets the progressbar maximum value
        /// </summary>
        /// <param name="max">The maximum number to process</param>
        public void SetProgressValues(int max)
        {
            this.Invoke(() =>
            {
                progressBar.Maximum = max;
                Max = max;
                counter.Text = string.Format("0 of {0} processed", max);
            });
        }

        /// <summary>
        /// Increments the progressbar
        /// </summary>
        /// <param name="val">The amount to increase the progress bar. Default for 1 or send in the amount already processed if in recovery</param>
        public void Increase(int val = 1)
        {
            if (IsHandleCreated)
            {
                this.Invoke(() =>
                {
                    progressBar.Increment(val);
                    counter.Text = string.Format("{0} of {1} processed", progressBar.Value, Max);
                });
            }
            CurrentCount = progressBar.Value;
        }

        private void Tdrive_Click(object sender, EventArgs e)
        {
            WillImage = false;
            Tdrive.Enabled = false;
            Imaging.Enabled = false;
            reportThread = new Thread(() =>
            {
                Report.SetMaxValue();
                Report.Process();
                if (!IsClosed)
                    CloseForm(false);
            });
            reportThread.Start();
        }

        private void Imaging_Click(object sender, EventArgs e)
        {
            WillImage = true;
            Tdrive.Enabled = false;
            Imaging.Enabled = false;
            reportThread = new Thread(() =>
            {
                Report.SetMaxValue();
                Report.Process();
                if (!IsClosed)
                    CloseForm(false);
            });
            reportThread.Start();
        }

        /// <summary>
        /// Invokes the close method to close the form.
        /// </summary>
        public void CloseForm(bool errorsFound)
        {
            this.Invoke(() =>
            {
                Report.ShouldDelete = true;
                this.Close();
                Report.IsRunning = false;
                if (!DidCancel)
                    Report.EndScript(errorsFound);
                reportThread.Abort();
            });
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            IsClosed = true;
            Report.IsRunning = false;
            this.Close();
        }

        private void ActivityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckProgress && progressBar.Value < progressBar.Maximum)
            {
                DidCancel = true;
                string message = "Yes:\tRemove all image files, control files and recovery\r\nNo:\tKeep files and end in recovery";
                DialogResult result = MessageBox.Show(message, "Cancel and Remove Files", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Report.ShouldDelete = false;
                    if (Directory.Exists(Folder))
                        Directory.Delete(Folder, true);
                    Report.EndScript(false, "Process Canceled");
                }
                else if (result == DialogResult.No)
                {
                    Report.CancelScript();
                }
            }
        }

        public void StopThread()
        {
            this.Invoke(() =>
            {
                if (reportThread != null && reportThread.IsAlive)
                    reportThread.Abort();
            });
        }

        /// <summary>
        /// Simplifies Invoke() calls everywhere else in this class.
        /// </summary>
        private void Invoke(Action a)
        {
            base.Invoke(a);
        }
    }
}