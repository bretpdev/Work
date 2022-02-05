using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace INVOSAMPFD
{
    public partial class StatusForm : Form
    {
        ReflectionInterface ri;
        private CsvRecord[] parsedRecords;
        private ProcessLogData pld;
        public StatusForm(ReflectionInterface ri, ProcessLogData pld)
        {
            InitializeComponent();
            this.ri = ri;
            this.pld = pld;
            Cancel(); //set labels to default values
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var results = CsvHelper.ParseTo<CsvRecord>(File.ReadAllLines(dialog.FileName));
                    if (results.HasErrors)
                        Dialog.Warning.Ok(results.GenerateErrorMessage());
                    else
                    {
                        parsedRecords = results.ValidLines.Select(o => o.ParsedEntity).ToArray();
                        FileLocationBox.Text = dialog.FileName;
                        GenerateButton.Enabled = true;
                    }
                }
            }
        }

        Thread mainThread = null;
        enum RunState
        {
            NotRunning,
            Running,
            PendingCancel
        }
        RunState state = RunState.NotRunning;
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (state == RunState.PendingCancel || state == RunState.Running)
            {
                state = RunState.PendingCancel;
                return;
            }
            mainThread = new Thread(Process);
            mainThread.Start();
        }

        private void SetStatus(string status)
        {
            Invoke(() => this.StatusLabel.Text = status);
        }

        private void SetSubStatus(string status, params object[] formatParameters)
        {
            Invoke(() => this.SubStatusLabel.Text = string.Format(status, formatParameters));
        }

        private void SetProgress(int value, int max)
        {
            Invoke(() =>
            {
                ProgressBar.Maximum = max;
                ProgressBar.Value = value;
            });
        }

        private void Invoke(Action a)
        {
            if (this.InvokeRequired)
                base.Invoke(a);
            else
                a();
        }

        private void Cancel()
        {
            Invoke(() =>
            {
                StatusLabel.Text = "Waiting for File Selection";
                GenerateButton.Text = "Generate Report";
                ProgressBar.Value = 0;
                SubStatusLabel.Text = "";
                state = RunState.NotRunning;
                BrowseButton.Enabled = true;
            });
        }

        private void Process()
        {
            Invoke(() => GenerateButton.Text = "Cancel");
            Invoke(() => BrowseButton.Enabled = false);
            state = RunState.Running;
            SetStatus("Gathering Report Data");
            List<Report> reports = new List<Report>();
            var borrowerGroups = parsedRecords.GroupBy(o => o.BorrowerSsn);
            int borrowerCount = 1;
            int totalBorrowers = borrowerGroups.Count();
            foreach (var borrowerRecords in borrowerGroups)
            {
                if (state != RunState.Running)
                    break;
                SetSubStatus("Processing Borrower {0} of {1}", borrowerCount, totalBorrowers);
                SetProgress(borrowerCount, totalBorrowers);
                RecordProcessor rp = new RecordProcessor(ri, borrowerRecords.Key, borrowerRecords.ToArray(), pld);
                var report = rp.Process();
                if (report != null)
                    reports.Add(report);
                borrowerCount++;
            }
            if (state == RunState.Running)
            {
                SetStatus("Compiling Report");

                string location = @"Q:\FSA Monthly Reports\";
                if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
                    location = @"T:\";
                string filename = string.Format("FSA Monthly Invoice Sampling {0}.docx", DateTime.Now.ToString("MMyyyy"));

                var generator = new ReportGenerator();
                generator.AggregateAndGenerate(Path.Combine(location, filename), reports,
                i =>
                {
                    SetSubStatus("Processing Borrower {0} of {1}", i, totalBorrowers);
                    SetProgress(i, totalBorrowers);
                    return state == RunState.Running;
                },
                () =>
                {
                    SetStatus("Saving Report");
                    SetSubStatus("");
                });
            }
            if (state != RunState.Running)
                Cancel();
            else
            {
                Dialog.Info.Ok("Processing Complete");
                Invoke(() => this.Close());
            }
        }
    }
}
