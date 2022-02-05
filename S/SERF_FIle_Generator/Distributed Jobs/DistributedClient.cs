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

namespace SERF_File_Generator
{
    public partial class DistributedClient : Form
    {
        private DistributedJobInfo Job { get; set; }
        public DistributedClient(DistributedJobInfo info)
        {
            InitializeComponent();
            LoadJob(info);
        }

        private bool running = false;
        private void LoadJob(DistributedJobInfo info)
        {
            Job = info;
            WorkspaceText.Text = info.Workspace.WorkspaceDirectory;
            JobText.Text = Path.GetFileName(Job.JobDirectory);
            Task.Factory.StartNew(() =>
            {
                running = true;
                while (running)
                {
                    string workedFile = DistributedJobHelper.FindAndWorkBatch(Job);
                    if (workedFile != null)
                        AddWorkedFile(workedFile);
                    else
                        running = false;
                }
                if (this.IsHandleCreated)
                    this.Invoke(new Action(() => StopButton.Text = "Processing Complete.  Click to close."));
            });
        }

        private void AddWorkedFile(string workedFile)
        {
            if (this.IsHandleCreated)
                this.Invoke(new Action(() => FinishedList.Items.Insert(0, workedFile)));
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (running)
                running = false;
            this.Close();
        }
    }
}
