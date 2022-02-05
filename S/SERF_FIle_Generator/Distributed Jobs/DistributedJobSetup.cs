using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace SERF_File_Generator
{
    public partial class DistributedJobSetup : Form
    {
        public DistributedJobSetup()
        {
            InitializeComponent();
            WorkspaceLocationText.Text += "\\";
        }

        public DistributedJobInfo JobInfo { get; private set; }
        public bool IsNewJob { get; private set; }
        private void NewJobButton_Click(object sender, EventArgs e)
        {
            using (var input = new InputBox<TextBox>("Enter a name for this Job"))
            {
                if (input.ShowDialog() == DialogResult.OK)
                {
                    string jobName = input.InputControl.Text;
                    WorkspaceInfo info = new WorkspaceInfo(WorkspaceLocationText.Text);
                    var job = DistributedJobHelper.InitializeJobScaffold(info, jobName);
                    JobInfo = job;
                    IsNewJob = true;
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            var selected = JobsList.SelectedItem as string;
            WorkspaceInfo workspace = new WorkspaceInfo(WorkspaceLocationText.Text);
            var job = new DistributedJobInfo(workspace, selected);
            JobInfo = job;
            IsNewJob = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        ToolTip tt = new ToolTip();
        private void WorkspaceLocationText_TextChanged(object sender, EventArgs e)
        {
            WorkspaceInfo info = new WorkspaceInfo(WorkspaceLocationText.Text);
            if (info.ValidWorkspace)
            {
                WorkspaceLocationText.BackColor = Color.LightGreen;
                tt.SetToolTip(WorkspaceLocationText, "This is a valid workspace location.  If no jobs are listed, no jobs currently exist.");
                LoadJobs(info);
                NewJobButton.Enabled = true;
            }
            else
            {
                WorkspaceLocationText.BackColor = Color.Pink;
                tt.SetToolTip(WorkspaceLocationText, "Invalid Workspace.  Workspace must contain a folder called 'In Progress', a folder called 'Complete', and no other folders.");
                ClearJobs();
                NewJobButton.Enabled = false;
            }
        }

        private void LoadJobs(WorkspaceInfo info)
        {
            JobsList.DataSource = Directory.GetDirectories(info.InProgressDirectory).Select(o => Path.GetFileName(o)).ToList();
        }

        private void ClearJobs()
        {
            JobsList.DataSource = null;
        }

        private void JobsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            JoinButton.Enabled = JobsList.SelectedIndex != -1;
        }
    }
}
