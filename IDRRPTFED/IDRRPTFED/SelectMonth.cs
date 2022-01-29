using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace IDRRPTFED
{
    public partial class SelectMonth : Form
    {
        public RunHistory SelectedRun { get; internal set; }
        List<RunHistory> dates;

        public SelectMonth()
        {
            InitializeComponent();

            //Load the run history
            dates = DataAccess.GetLastRunDate().OrderByDescending(o => o.RunDate).ToList();
            PreviousDates.AutoGenerateColumns = false;
            PreviousDates.DataSource = dates;
            ToggleSelection();
            SelectCurrentRun();
        }

        public void SelectCurrentRun()
        {
            SelectedRun = new RunHistory();
            DateTime startDate = DateTime.Now.AddYears(-1);
            if (dates.Any())
                startDate = dates.Max(p => p.EndDate).AddMinutes(1);
            SelectedRun.StartDate = startDate;
            SelectedRun.RunDate = SelectedRun.EndDate = DateTime.Now;
            RunTypeTextBox.Text = "Current Run";
            SyncDisplay();
        }

        public void SelectPreviousRun()
        {
            RunButton.Enabled = (PreviousDates.SelectedRows.Count > 0);
            if (PreviousDates.SelectedRows.Count == 0)
                return;
            int rowId = PreviousDates.SelectedRows[0].Index;
            SelectedRun = dates[rowId];
            RunTypeTextBox.Text = "Previous Run";
            SyncDisplay();
        }

        public void SyncDisplay()
        {
            string formatString = "MM/dd/yy hh:mm tt";
            if (SelectedRun != null)
            {
                StartDateTextBox.Text = SelectedRun.StartDate.ToString(formatString);
                EndDateTextBox.Text = SelectedRun.EndDate.ToString(formatString);
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (SelectedRun != null)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("You must choose a month to process first.", "Select Month", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PreviousDates_SelectionChanged(object sender, EventArgs e)
        {
            if (currentMode == ToggleMode.PreviousRun)
                SelectPreviousRun();
        }

        private enum ToggleMode
        {
            CurrentRun,
            PreviousRun
        }

        private ToggleMode currentMode = ToggleMode.PreviousRun;
        private void ToggleSelectionButton_Click(object sender, EventArgs e)
        {
            ToggleSelection();
        }

        private void ToggleSelection()
        {
            if (currentMode == ToggleMode.CurrentRun)
            {
                currentMode = ToggleMode.PreviousRun;
                ToggleSelectionButton.Text = "Revert to Current Run";
                PreviousDates.Visible = true;
                this.Height = 555;
                SelectPreviousRun();
            }
            else
            {
                currentMode = ToggleMode.CurrentRun;
                ToggleSelectionButton.Text = "Select Previous Run";
                this.Height = 245;
                RunButton.Enabled = true;
                PreviousDates.Visible = false;
                SelectCurrentRun();
            }
            RunButton.Focus();
        }

        private void StartDateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
