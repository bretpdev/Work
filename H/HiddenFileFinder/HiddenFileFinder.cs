using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HiddenFileFinder
{
    public partial class HiddenFileFinder : Form
    {
        public HiddenFileFinder()
        {
            InitializeComponent();

            txtStatus.Text = "No directory selected.";
        }

        private void FindHiddenFiles(string directoryName)
        {
            if (!Directory.Exists(directoryName)) { return; }

            //Check each file in this directory to see if it's hidden.
            foreach (string fileName in Directory.GetFiles(directoryName))
            {
                //Keep the user informed of what's going on.
                txtStatus.Text = "Scanning file " + fileName;
                Application.DoEvents();

                if ((File.GetAttributes(fileName) & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    lstResults.Items.Add(fileName);
                }
            }

            //Recurse into any subdirectories found in this directory.
            foreach (string subdirectory in Directory.GetDirectories(directoryName))
            {
                FindHiddenFiles(subdirectory);
            }
        }//FindHiddenFiles()

        private void DoScan(string directoryName)
        {
            this.Cursor = Cursors.WaitCursor;
            lstResults.Items.Clear();
            FindHiddenFiles(txtScanRoot.Text);
            txtStatus.Text = "Done!";
            if (lstResults.Items.Count > 0)
            {
                txtStatus.Text += " Double-click a file name to view its parent directory.";
            }
            else
            {
                txtStatus.Text += " There are no results.";
            }
            this.Cursor = Cursors.Default;
        }//StartScan()

        #region Event Handlers
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtScanRoot.Text = dialog.SelectedPath;
                DoScan(txtScanRoot.Text);
            }
        }//btnBrowse_Click()

        private void lstResults_DoubleClick(object sender, EventArgs e)
        {
            if (lstResults.SelectedIndex >= 0)
            {
                //Open the parent directory of the selected file.
                string parentDirectory = lstResults.SelectedItem.ToString().Substring(0, lstResults.SelectedItem.ToString().LastIndexOf('\\'));
                System.Diagnostics.Process.Start("explorer.exe", parentDirectory);
            }
        }//lstResults_DoubleClick()

        private void txtScanRoot_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoScan(txtScanRoot.Text);
                e.Handled = true;
            }
        }//txtScanRoot_KeyUp()
        #endregion //Event Handlers
    }//class
}//namespace
