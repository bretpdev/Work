using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImagingTransferFileBuilder.Properties;
using Uheaa.Common;

namespace ImagingTransferFileBuilder
{
    public partial class ImageScraperControl : UserControl
    {
        OpenFolderDialog ofd = new OpenFolderDialog();
        public ImageScraperControl()
        {
            InitializeComponent();

            ResultsLocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "ResultsLocation"));
            MasterExcelLocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "ExcelLocation"));
        }

        private void ResultsLocationBrowse_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this.Handle, true) == DialogResult.OK)
            {
                Settings.Default.ResultsLocation = ofd.Folder;
            }
        }

        private void MasterExcelLocationsBrowse_Click(object sender, EventArgs e)
        {
            if (OpenExcelDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.ExcelLocation = OpenExcelDialog.FileName;
            }
        }


        private void ScrapeButton_Click(object sender, EventArgs e)
        {
            if (FileSystemHelper.CheckFile(MasterExcelLocationText.Text))
            {
                try
                {
                    ImageScraper.Scrape(MasterExcelLocationText.Text, SheetNameText.Text, ResultsLocationText.Text, UsernameText.Text, PasswordText.Text, LoanProgramTypeText.Text, DocDateCheck.Checked ? (DateTime?)DateAfterPicker.Value : null);
                }
                catch (InvalidCredentialsException)
                {
                    MessageBox.Show("Invalid Credentials");
                }
                catch (NoSheetFound)
                {
                    MessageBox.Show(string.Format("No sheet with name {0} was found", SheetNameText.Text));
                    Progress.Failure();
                }
            }
        }

        private void DocDateCheck_CheckedChanged(object sender, EventArgs e)
        {
            DateAfterPicker.Enabled = DocDateCheck.Checked;
        }

    }
}
