using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace ErrorFinder
{
    public partial class MainForm : Form
    {
        private List<BorrowerLine> Results { get; set; }
        private string BaseExportLocation { get; set; }
        private string ExportLocation { get { return Path.Combine(Path.Combine(BaseExportLocation, Environment.UserName), Util.TimeStamp()); } }
        private bool refreshImmediately;
        private bool generateImmediately;
        private int PossibleNumberOfReports;
        public MainForm(List<BorrowerLine> results, bool refreshImmediately, bool generateImmediately)
        {
            BaseExportLocation = @"X:\Archive\ALIGN\ErrorReports\";
            InitializeComponent();
            Results = results;
            //one per error code, one per batch number, and 1 for a report of all
            PossibleNumberOfReports = 1 + Results.Select(o => o.ErrorCode).Distinct().Count() + Results.Select(o => o.MinorBatchNo).Distinct().Count();
            ErrorSelector.DataSource = new string[] { "" }.Union(Results.Select(o => o.ErrorCode).Distinct().OrderBy(o => o)).ToList();
            BatchSelector.DataSource = new string[] { "" }.Union(Results.Select(o => o.MinorBatchNo).Distinct().OrderBy(o => o)).ToList();
            LoadResults();
            this.refreshImmediately = refreshImmediately;
            this.generateImmediately = generateImmediately;
        }

        private void ErrorCodeText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }

        public void LoadResults()
        {
            if (ErrorSelector.SelectedIndex == 0 && BatchSelector.SelectedIndex == 0)
            {
                ResultsGrid.DataSource = Results;
                RecordsFoundLabel.Text = Results.Count + " Total Records";
                ExportCurrentMenu.Enabled = false;
            }
            else
            {
                var filter = FilteredResults;
                ResultsGrid.DataSource = filter;
                RecordsFoundLabel.Text = filter.Count + " Filtered Records";
                ExportCurrentMenu.Enabled = true;
            }
        }

        private List<BorrowerLine> FilteredResults
        {
            get
            {
                if (ErrorSelector.SelectedIndex == 0 && BatchSelector.SelectedIndex == 0)
                    return Results;
                string errorCode = ErrorSelector.SelectedValue as string;
                string batchNum = BatchSelector.SelectedValue as string;
                var filtered = from result in Results
                               where (errorCode == "" || result.ErrorCode == errorCode)
                                  && (batchNum == "" || result.MinorBatchNo == batchNum)
                               select result;
                return filtered.ToList();
            }
        }

        private void ErrorSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadResults();
        }


        private void BatchSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadResults();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (refreshImmediately)
                RefreshDatabase();
            if (generateImmediately)
                GenerateAllViews();
        }

        private void ExportCurrentMenu_Click(object sender, EventArgs e)
        {
            ExportPopup p = new ExportPopup(ExportLocation, Results, CurrentViewLabel);
            p.ShowDialog();
        }

        private void ExportDataMenu_Click(object sender, EventArgs e)
        {
            ExportPopup p = new ExportPopup(ExportLocation, Results, "borrower_export");
            p.ShowDialog();
        }


        private void ExportAllMenu_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to generate {0} reports?", PossibleNumberOfReports), "Generate Reports?", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                GenerateAllViews();
            }
        }

        private void GenerateAllViews()
        {
            ExportPopup p = new ExportPopup(ExportLocation, Results, null);
            p.ShowDialog();
        }

        private void ChangeExportMenu_Click(object sender, EventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();
            if (ofd.ShowDialog(this.Handle, true) == DialogResult.OK)
            {
                BaseExportLocation = ofd.Folder;
            }
        }

        private void RefreshMenu_Click(object sender, EventArgs e)
        {
            RefreshDatabase();
        }

        private void RefreshDatabase()
        {
            RefreshPopup pop = new RefreshPopup(Results);
            pop.Show();
        }

        private void OpenExcelButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ExportHelper.Export(FilteredResults, Path.Combine(ExportLocation, CurrentViewLabel + "_" + Util.TimeStamp() + ".csv"), true);
            Cursor.Current = Cursors.Default;
        }

        private string CurrentViewLabel
        {
            get
            {
                string label = "borrowers_with_";
                string error = ErrorSelector.SelectedValue as string;
                bool hasError = !string.IsNullOrEmpty(error);
                string batch = BatchSelector.SelectedValue as string;
                bool hasBatch = !string.IsNullOrEmpty(batch);

                if (!hasError && !hasBatch)
                    label = "all_borrowers";
                else
                {
                    if (hasError)
                        label += "error_" + error;
                    if (hasError && hasBatch)
                        label += "_and_";
                    if (hasBatch)
                        label += "batch_" + batch;
                }
                return label;
            }
        }
    }
}
