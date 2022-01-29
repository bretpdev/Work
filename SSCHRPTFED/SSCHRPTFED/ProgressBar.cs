using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common.Scripts;


namespace SSCHRPTFED
{
    public partial class ProgressBar : Form
    {
        private int NumberOfFilesToProcess { get; set; }
        private int NumberOfFilesProcessed { get; set; }
        private List<SchoolInfo> SData { get; set; }
        private WorkReports Reports { get; set; }
        private string ProgressCount
        {
            get
            {
                return string.Format("Running Send School Reports FED.  Processing file  {0} of {1} files", NumberOfFilesProcessed, NumberOfFilesToProcess);
            }
        }

        public ProgressBar(WorkReports reports, List<SchoolInfo> sData)
        {
            InitializeComponent();
            SData = sData;
            Progress.Maximum = NumberOfFilesToProcess = SData.Count;
            Reports = reports;
            NumberOfFilesProcessed = 0;
            this.Count.Text = ProgressCount;
        }



        private void Cancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel the script?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                backgroundWorker1.CancelAsync();
                backgroundWorker1.Dispose();
                DialogResult = DialogResult.Cancel;
            }
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            foreach(SchoolInfo item in SData)
            {
                Reports.Process(item);
                backgroundWorker1.ReportProgress(1);
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Increment(1);
            NumberOfFilesProcessed++;
            this.Count.Text = ProgressCount;
            this.Refresh();
        }

         
    }
}
