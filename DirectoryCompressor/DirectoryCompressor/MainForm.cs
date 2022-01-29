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
using Uheaa.Common;

namespace DirectoryCompressor
{
    public partial class MainForm : Form
    {
        public MainForm(Args args)
        {
            InitializeComponent();

            this.TargetFolderBox.Text = args.Location;
            this.DestinationFolderBox.Text = args.Destination;
        }

        Compressor compressor;
        private void CompressButton_Click(object sender, EventArgs e)
        {
            if (compressor != null)
                compressor.IsRunning = false;
            var log = new TextBoxLog(ActivityLog, FoundFilesLabel);
            compressor = new Compressor(log, TargetFolderBox.Text, DestinationFolderBox.Text);
            compressor.ProcessAsync();
            ButtonCheck();
            label2.Focus();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (compressor != null)
                compressor.IsRunning = false;
        }

        private void RunTimer_Tick(object sender, EventArgs e)
        {
            ButtonCheck();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFolderDialog();
            if (ofd.ShowDialog(this.Handle, false) == DialogResult.OK)
            {
                TargetFolderBox.Text = ofd.Folder;
                if (string.IsNullOrEmpty(DestinationFolderBox.Text))
                    DestinationFolderBox.Text = ofd.Folder;
            }
        }

        private void BrowseDestinationButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFolderDialog();
            if (ofd.ShowDialog(this.Handle, false) == DialogResult.OK)
                DestinationFolderBox.Text = ofd.Folder;
        }

        private void ButtonCheck()
        {
            if (compressor != null)
            {
                CancelButton.Enabled = compressor.IsRunning;
                CompressButton.Enabled = BrowseButton.Enabled = TargetFolderBox.Enabled = DestinationFolderBox.Enabled = !compressor.IsRunning;
                if (compressor.FinishedRunning)
                    compressor = null;
            }
            else
            {
                CompressButton.Enabled = Directory.Exists(TargetFolderBox.Text) && Directory.Exists(DestinationFolderBox.Text);
            }
        }


    }
}
