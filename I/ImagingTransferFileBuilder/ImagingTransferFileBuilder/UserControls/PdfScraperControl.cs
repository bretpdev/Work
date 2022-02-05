using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Uheaa.Common;

namespace ImagingTransferFileBuilder
{
    public partial class PdfScraperControl : UserControl
    {
        OpenFolderDialog ofd = new OpenFolderDialog();
        public PdfScraperControl()
        {
            InitializeComponent();
            ResultsLocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "ResultsLocation"));
            MasterExcelLocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "ExcelLocation"));
            PdfLocationText.DataBindings.Add(new Binding("Text", Properties.Settings.Default, "PdfLocation"));
        }

        private void ResultsLocationBrowse_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this.Handle, true) == DialogResult.OK)
            {
                Properties.Settings.Default.ResultsLocation = ofd.Folder;
            }
        }

        private void PdfLocationBrowse_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this.Handle, false) == DialogResult.OK)
            {
                Properties.Settings.Default.PdfLocation = ofd.Folder;
            }
        }

        private void MasterExcelLocationsBrowse_Click(object sender, EventArgs e)
        {
            if (OpenExcelDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.ExcelLocation = OpenExcelDialog.FileName;
            }
        }

        private void ScrapeButton_Click(object sender, EventArgs e)
        {
            if (FileSystemHelper.CheckDirectory(ResultsLocationText.Text) && FileSystemHelper.CheckDirectory(PdfLocationText.Text) && FileSystemHelper.CheckFile(MasterExcelLocationText.Text))
            {
                try
                {
                    PdfScraper.Scrape(PdfLocationText.Text, ResultsLocationText.Text, MasterExcelLocationText.Text, SheetNameText.Text, DocDatePicker.Text, SaleDatePicker.Text, DealIDText.Text, LoanProgramTypeText.Text);
                }
                catch (NoSheetFound)
                {
                    MessageBox.Show(string.Format("No Sheet with name \"{0}\" found.", SheetNameText.Text));
                }
            }
        }
    }
}
