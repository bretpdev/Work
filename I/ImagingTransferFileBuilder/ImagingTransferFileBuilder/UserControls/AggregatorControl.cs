using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImagingTransferFileBuilder.Properties;
using Uheaa.Common;

namespace ImagingTransferFileBuilder
{
    public partial class AggregatorControl : UserControl
    {
        OpenFolderDialog ofd = new OpenFolderDialog();

        public AggregatorControl()
        {
            InitializeComponent();
            ResultsLocationText.DataBindings.Add(new Binding("Text", Settings.Default, "ResultsLocation"));
        }

        private void ResultsLocationBrowse_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this.Handle, true) == DialogResult.OK)
            {
                Settings.Default.ResultsLocation = ofd.Folder;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            SettingsGroup.Enabled = GenerateButton.Enabled = false;

            if (FileSystemHelper.CheckDirectory(ResultsLocationText.Text))
            {
                try
                {
                    Aggregator.Aggregate(ResultsLocationText.Text, DealIDText.Text, SaleDatePicker.Value.ToString("MM/dd/yyyy"), LoanProgramTypeText.Text);
                }
                catch (ZipAlreadyExistsException)
                {
                    MessageBox.Show("Test");
                }
            }
            SettingsGroup.Enabled = GenerateButton.Enabled = true;
        }
    }
}
