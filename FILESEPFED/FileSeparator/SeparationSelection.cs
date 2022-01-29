using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileSeparator
{
    public partial class SeparationSelection : Form
    {
        public int RecordCount { get; set; }
        public List<string> Rows { get; set; }
        public int RowsPerFile { get; set; }
        public int NumberOfNewFiles { get; set; }
        public bool HasHeaderRow { get; set; }
        public bool HasRecordCount { get; set; }
        public string HeaderRow { get; set; }

        public SeparationSelection(string fileName)
        {
            InitializeComponent();
            LoadingProgress progress = new LoadingProgress(fileName);
            Rows = progress.Start();
            progress.Close();
            totalRows.Text = Rows.Count.ToString();
            RecordCount = Rows.Count;
        }


        private void NumberPerFile_TextChanged(object sender, EventArgs e)
        {
            if (NumberPerFile.Text != string.Empty)
            {
                try
                {
                    if (int.Parse(NumberPerFile.Text) > RecordCount)
                    {
                        MessageBox.Show("You must choose a number less than the total number of rows in the file.", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        NumberPerFile.Text = string.Empty;
                    }
                    else
                    {
                        RecordCount = Rows.Count;
                        if (chkheaderRow.Checked) --RecordCount;
                        RowsPerFile = int.Parse(NumberPerFile.Text);
                        NumberOfFiles.Text = Math.Ceiling((double)RecordCount / (double)RowsPerFile).ToString();
                        NumberOfNewFiles = int.Parse(NumberOfFiles.Text);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Please insert a valid number to calculate how many files to create", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    NumberPerFile.Text = string.Empty;
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (NumberOfFiles.Text != string.Empty)
            {
                //Check for header row and assign it if there is one.
                HasHeaderRow = chkheaderRow.Checked;
                if (HasHeaderRow)
                {
                    if (string.IsNullOrEmpty(HeaderRow))
                    {
                        HeaderRow = Rows[0];
                        HasRecordCount = HasHeaderRow ? HeaderRow.ToString().ToUpper().Contains("COUNTER") : false;
                        Rows.RemoveAt(0);
                    }
                    if (HasRecordCount && RowsPerFile < 30)
                    {
                        MessageBox.Show("This file has a borrower record counter. You must choose to have at least 30 records per file", "Too Few Records", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
        }

        private void NumberPerFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && NumberPerFile.Text != string.Empty)
            {
                OK_Click(sender, new EventArgs());
            }
        }
    }
}
