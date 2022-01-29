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
using System.Collections.Specialized;
using Uheaa.Common;

namespace ImagingTransferFileBuilder
{
    public partial class GeneratorControl : UserControl
    {
        OpenFolderDialog ofd = new OpenFolderDialog();
        public GeneratorControl()
        {
            InitializeComponent();
            ResultsLocationText.DataBindings.Add(new Binding("Text", Settings.Default, "ResultsLocation"));
            MasterExcelLocationText.DataBindings.Add(new Binding("Text", Settings.Default, "ExcelLocation"));
            BindLocations();
        }

        private void BindLocations()
        {
            LoadLocationsList.DataSource = null;
            LoadLocationsList.DataSource = Properties.Settings.Default.LoadLocations;
        }
        private void ResultsLocationBrowse_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this.Handle, true) == DialogResult.OK)
            {
                Settings.Default.ResultsLocation = ofd.Folder;
            }
        }

        private void LoadLocationsAddButton_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this.Handle, false) == DialogResult.OK)
            {
                if (Settings.Default.LoadLocations == null)
                    Settings.Default.LoadLocations = new StringCollection();
                Settings.Default.LoadLocations.Add(ofd.Folder);
                BindLocations();
            }
        }

        private void LoadLocationsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLocationsRemoveButton.Enabled = (LoadLocationsList.SelectedIndex >= 0);
        }

        private void LoadLocationsRemoveButton_Click(object sender, EventArgs e)
        {
            if (Settings.Default.LoadLocations == null)
                Settings.Default.LoadLocations = new StringCollection();
            Settings.Default.LoadLocations.RemoveAt(LoadLocationsList.SelectedIndex);
            BindLocations();
            
            LoadLocationsList.Focus();
        }

        private void MasterExcelLocationsBrowse_Click(object sender, EventArgs e)
        {
            if (OpenExcelDialog.ShowDialog() == DialogResult.OK)
            {
                if (!FileSystemHelper.IsSecureLocation(OpenExcelDialog.FileName))
                {
                    MessageBox.Show("Not a secured CornerStone location: " + OpenExcelDialog.FileName);
                    return;
                }
                Settings.Default.ExcelLocation = OpenExcelDialog.FileName;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (!FileSystemHelper.IsSecureLocation(MasterExcelLocationText.Text))
            {
                MessageBox.Show("Not a secure CornerStone location: " + MasterExcelLocationText.Text);
                return;
            }
            if (!FileSystemHelper.IsSecureLocation(ResultsLocationText.Text))
            {
                MessageBox.Show("Not a secure CornerStone location: " + ResultsLocationText.Text);
                return;
            }
            Results.Clear();
            this.GenerateButton.Enabled = SetupGroup.Enabled = false;
            if (FileSystemHelper.CheckDirectory(ResultsLocationText.Text) && FileSystemHelper.CheckFile(MasterExcelLocationText.Text))
            {
                List<string> loadLocations = new List<string>();
                bool allCorrect = true;
                foreach (string s in LoadLocationsList.Items)
                {
                    if (!FileSystemHelper.CheckDirectory(s))
                    {
                        allCorrect = false;
                        break;
                    }
                    loadLocations.Add(s);
                }
                if (allCorrect)
                {
                    try
                    {
                        Generator.Generate(ResultsLocationText.Text, MasterExcelLocationText.Text, loadLocations, SheetNameText.Text, ReplaceCheck.Checked);
                    }
                    catch (NoSheetFound)
                    {
                        Results.LogError(string.Format("No Sheet with name '{0}' was found in the excel document.", SheetNameText.Text));
                        Progress.Finish();
                    }
                }
            }
            this.GenerateButton.Enabled = SetupGroup.Enabled = true;
        }

        private void LoadLocationsList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                LoadLocationsRemoveButton.PerformClick();
            }
        }

    }
}
