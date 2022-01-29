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

namespace NSLDSCONSO
{
    public partial class RecoveryForm : Form
    {
        public RecoveryForm(DataLoadRun unfinishedRun)
        {
            InitializeComponent();
            StartBox.Text = unfinishedRun.StartedOn.ToString();
            UpdateBox.Text = unfinishedRun.LastUpdated.ToString();
            ElapsedBox.Text = (unfinishedRun.LastUpdated - unfinishedRun.StartedOn).ToString(@"hh\:mm\:ss");
            var perc = (int)(((double)unfinishedRun.ActualBorrowerCount / unfinishedRun.BorrowerCount) * 100);
            ProgressBox.Text = unfinishedRun.ActualBorrowerCount + "/" + unfinishedRun.BorrowerCount + " (" + perc + "%)";
            IdBox.Text = unfinishedRun.DataLoadRunId.ToString();
            FilenameBox.Text = unfinishedRun.Filename;
        }

        private void FileCheckTimer_Tick(object sender, EventArgs e)
        {
            RecoveryButton.Enabled = File.Exists(FilenameBox.Text);
        }
    }
}
