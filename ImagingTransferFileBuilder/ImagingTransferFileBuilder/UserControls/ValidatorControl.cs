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
    public partial class ValidatorControl : UserControl
    {
        public ValidatorControl()
        {
            InitializeComponent();
            MasterExcelLocationText.DataBindings.Add(new Binding("Text", Settings.Default, "ExcelLocation"));
            ZipLocationText.DataBindings.Add(new Binding("Text", Settings.Default, "ZipLocation"));
        }

        private void ZipLocationBrowse_Click(object sender, EventArgs e)
        {
            if (OpenZipDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.ZipLocation = OpenZipDialog.FileName;
            }
        }

        private void MasterExcelLocationBrowse_Click(object sender, EventArgs e)
        {
            if (OpenExcelDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.ExcelLocation = OpenExcelDialog.FileName;
            }
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            if (FileSystemHelper.CheckFile(ZipLocationText.Text) && FileSystemHelper.CheckFile(MasterExcelLocationText.Text))
            {
                try
                {
                    Validator.Validate(ZipLocationText.Text, MasterExcelLocationText.Text, SheetNameText.Text);
                }
                catch (NoIndexFileException)
                {
                    MessageBox.Show("No index file (*.IDX) found.  Please create an index file before continuing.");
                    Progress.Failure();
                }
                catch (MultipleIndexFilesException)
                {
                    MessageBox.Show("Multiple index files (*.IDX) found.  Please ensure there is only one index file before continuing.");
                    Progress.Failure();
                }
                catch (NoSheetFound)
                {
                    MessageBox.Show("Couldn't find a sheet called '" + SheetNameText.Text + "' in the Excel document.");
                    Progress.Failure();
                }
            }
        }
    }
}
