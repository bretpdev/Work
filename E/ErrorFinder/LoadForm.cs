using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ErrorFinder
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
            RefreshCheck.Enabled = SqlHelper.CanAccessOpsdev;
        }

        OpenFileDialog ofd = new OpenFileDialog();
        Processor p;
        private void LoadForm_Shown(object sender, EventArgs e)
        {
            p = Processor.Process(ofd.FileName, this, ProgressUpdate, ProcessResults);
            RefreshCheck.Text = "Immediately refresh opsdev.EA27_BANA.dbo.Borrower_Errors";
        }

        private void ProgressUpdate(int progress, int max)
        {
            LoadProgress.Maximum = max;
            LoadProgress.Value = progress;
        }

        private void ProcessResults(List<BorrowerLine> results)
        {
            MainForm mf = new MainForm(results, RefreshCheck.Checked, GenerateCheck.Checked);
            mf.Show();
            this.Hide();
        }

        private void LoadForm_Load(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void LoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (p.Thread != null)
                p.Thread.Abort();
        }
    }
}
