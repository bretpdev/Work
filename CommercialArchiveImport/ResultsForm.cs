using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommercialArchiveImport
{
    public partial class ResultsForm : Form
    {
        EojResults results;
        public ResultsForm(EojResults results)
        {
            this.results = results;
            InitializeComponent();
        }

        private void ResultsForm_Load(object sender, EventArgs e)
        {
            if (!results.HasRemainingZip)
                RemainingImagesButton.Visible = false;
        }

        private void EojButton_Click(object sender, EventArgs e)
        {
            Process.Start(results.EojFile);
        }

        private void EojFolderButton_Click(object sender, EventArgs e)
        {
            Process.Start(results.EojFolder);
        }

        private void CopyResultsButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to copy all the files from {0} to {1}?", results.ResultsFolder, results.LoadFolder), "Import Results", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<string> files = new List<string>();
                foreach (string file in Directory.GetFiles(results.ResultsFolder).Where(o => !o.EndsWith("Thumbs.db")))
                {
                    string name = Path.GetFileName(file);
                    if (File.Exists(Path.Combine(results.LoadFolder, name)))
                    {
                        MessageBox.Show(string.Format("Cannot complete transfer, {0} already exists in {1}", name, results.LoadFolder));
                        return;
                    }
                    files.Add(file);
                }
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(results.LoadFolder, name));
                }
                MessageBox.Show("Transfer Complete");
            }
        }

        private void ImagingServerButton_Click(object sender, EventArgs e)
        {
            Process.Start(results.LoadFolder);
        }

        private void ViewResultsButton_Click(object sender, EventArgs e)
        {
            Process.Start(results.ResultsFolder);
        }

        private void RemainingImagesButton_Click(object sender, EventArgs e)
        {
            Process.Start(results.RemainingZip);
        }
    }
}
