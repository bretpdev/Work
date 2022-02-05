using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PYOFFLTRFD
{
    public partial class PayoffInformation : Form
    {
        public BorrowerData Borrower { get; set; }
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }

        /// <summary>
        /// This constructor is for unit testing, DO NOT USE
        /// </summary>
        public PayoffInformation(BorrowerData data)
        {
            Borrower = data;
            InitializeComponent();
        }

        public PayoffInformation(BorrowerData data, ReflectionInterface ri, ProcessLogRun logRun)
        {
            InitializeComponent();
            PayoffDate.MinDate = DateTime.Now.Date;
            VersionNumber.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LogRun = logRun;
            DA = new DataAccess(logRun);
            RI = ri;
            Borrower = data;
            AccountIdentifier.Text = Borrower.Demos.Ssn;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
                if (LoadData())
                    this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Validates all the data provided by the user
        /// </summary>
        /// <returns>True if everything required is supplied.</returns>
        public bool ValidateData()
        {
            string message = "";
            if (Borrower.Demos.Ssn.IsNullOrEmpty())
                message += "Account Number\r\n";
            if (PayoffDate.Text.ToDateNullable() == null)
                message += "Payoff Date\r\n";
            if (!AllLoans.Checked && PayoffDataGrid.SelectedRows.Count == 0)
                message += "Payoff All Loans or Loan Selections\r\n";
            if (message.IsPopulated())
                Dialog.Error.Ok(string.Format("The following fields are missing\r\n\r\n{0}\r\nPlease fix the fields and try again.", message));
            else
                return true;
            return false;
        }

        /// <summary>
        /// Assigns the SSN or Account Number to the PayoffData object
        /// </summary>
        private void AccountIdentifier_TextChanged(object sender, EventArgs e)
        {
            // See CausesValidation property
            DisableControls();
            List<LoanSelection> data = new List<LoanSelection>();
            PayoffDataGrid.Controls.Clear();
            PayoffDataGrid.DataSource = null;
            if (AccountIdentifier.Text.Length >= 9)
            {
                Borrower.Demos = LoadDemos();
                if (Borrower.Demos == null )
                {
                    DisableControls();
                    Label emptyMessage = new Label();
                    emptyMessage.Text = $"Borrower Not Found";
                    Size size = new Size(325, 35);
                    LoadGridWithMessage(emptyMessage, size);
                }
                else
                {
                    Borrower.DemosLoaded = true;
                    data = DA.GetLoanSelectionData(Borrower.Demos.AccountNumber);
                    if (data != null)
                        SetGridData(data);
                }
            }
        }

        /// <summary>
        /// Loads the grid view with the borrowers loan information or displays error message
        /// </summary>
        public void SetGridData(List<LoanSelection> data)
        {
            if (data.Count > 0)
            {
                EnableControls();
                PayoffDataGrid.Controls.Clear();
                PayoffDataGrid.DataSource = data;
                PayoffDataGrid.ClearSelection();
            }
            else
            {
                DisableControls();
                Label emptyMessage = new Label();
                emptyMessage.Text = "All loans are PIF";
                LoadGridWithMessage(emptyMessage, new Size(250, 35));
            }
        }

        /// <summary>
        /// Adds the message to the grid
        /// </summary>
        private void LoadGridWithMessage(Label emptyMessage, Size size)
        {
            emptyMessage.ForeColor = Color.Black;
            emptyMessage.Size = size;
            emptyMessage.Font = new Font(new FontFamily("Microsoft Sans Serif"), 20, FontStyle.Bold);
            emptyMessage.Location = new Point(100, 100);
            emptyMessage.Anchor = AnchorStyles.Top;
            PayoffDataGrid.Controls.Add(emptyMessage);
        }

        /// <summary>
        /// Changes the grid to disabled if all loans are selected
        /// </summary>
        private void AllLoans_CheckedChanged(object sender, EventArgs e)
        {
            PayoffDataGrid.Enabled = !AllLoans.Checked;

            if (AllLoans.Checked)
                PayoffDataGrid.SelectAll();
            else
                PayoffDataGrid.ClearSelection();
        }

        /// <summary>
        /// Enables all the controls, except the AccountIdentifier
        /// </summary>
        public void EnableControls()
        {
            PayoffDate.Enabled = true;
            AllLoans.Enabled = true;
            PayoffDataGrid.Enabled = !AllLoans.Checked;
            OK.Enabled = true;
        }

        /// <summary>
        /// Disables all the controls, except the AccountIdentifier
        /// </summary>
        public void DisableControls()
        {
            PayoffDate.Enabled = false;
            AllLoans.Enabled = false;
            AllLoans.Checked = false;
            PayoffDataGrid.Enabled = false;
            OK.Enabled = false;
            Borrower.DemosLoaded = false;
        }

        /// <summary>
        /// Get the borrower demographics from TX1J
        /// </summary>
        private SystemBorrowerDemographics LoadDemos()
        {
            SystemBorrowerDemographics demos = DA.GetDemos(AccountIdentifier.Text);
            if (demos != null && !demos.IsValidAddress)
            {
                Dialog.Warning.Ok("Borrower does not have a valid address.", "Invalid Address");
                AccountIdentifier.Text = "";
                demos = null;
            }

            return demos;
        }

        /// <summary>
        /// Gets the data from the system
        /// </summary>
        /// <returns>True if all the data was gathered</returns>
        public bool LoadData()
        {
            if (Borrower.DemosLoaded)
            {
                Borrower.PayoffDate = PayoffDate.Text;

                List<int> seqs = GetSequencesFromSelection(); //Get loans sequences selected

                foreach (int seq in seqs)
                {
                    RI.FastPath("TX3ZITS2O" + AccountIdentifier.Text);
                    if (RI.MessageCode == "50108")
                    {
                        Dialog.Warning.Ok("There are no loans in TS2O for this borrower", "No Loans");
                        return false;
                    }
                    else if (RI.ScreenCode == "TSX2P")
                    {
                        RI.PutText(7, 26, PayoffDate.Text.ToDate().ToString("MMddyy"));
                        RI.PutText(9, 54, "Y", ReflectionInterface.Key.Enter);
                        if (!GetPayoffData(FindSequence(seq), seq))
                            return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Gets all the loan sequence numbers that were selected in the form.
        /// </summary>
        /// <returns></returns>
        private List<int> GetSequencesFromSelection()
        {
            List<int> seqs = new List<int>();
            foreach (DataGridViewRow row in PayoffDataGrid.SelectedRows)
                seqs.Add(row.Cells["LoanSequence"].Value.ToString().ToInt());
            return seqs.OrderBy(i => i).ToList();
        }

        /// <summary>
        /// Finds the row that the loan sequence number is on.
        /// </summary>
        /// <param name="loanSeq">Loan Sequence Number</param>
        /// <returns>The row found</returns>
        private int FindSequence(int loanSeq)
        {
            while (RI.MessageCode != "90007")
            {
                for (int i = 13; i <= 22; i++)
                {
                    int? seq = RI.GetText(i, 16, 4).ToIntNullable();
                    if (seq != null && seq.Value == loanSeq)
                        return i;
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }

            return 0;
        }

        /// <summary>
        /// Gets the Payoff calculations for the borrower.
        /// </summary>
        private bool GetPayoffData(int row, int seq)
        {
            PayoffData data = new PayoffData();
            if (row > 0)
            {
                RI.PutText(row, 2, "X", ReflectionInterface.Key.Enter);
                data = new PayoffData();
                data.SequenceNumber = seq;
                string loanCode = RI.GetText(row, 43, 6);
                data.LoanProgram = RI.GetText(row, 43, 6);
                data.DateDisbursed = RI.GetText(row, 27, 8);
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.ScreenCode != "TSX2Q")
                    RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.ScreenCode != "TSX2Q")
                {
                    string message = string.Format("Unable to get to Payoff Calculation Totals screen in TS2O for borrower {0}", Borrower.Demos.AccountNumber);
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Info.Ok(message, "Unable to reach TS2O");
                    this.DialogResult = DialogResult.Cancel;
                    return false;
                }
                else
                {
                    data.PayoffAmount = RI.GetText(12, 27, 12).Replace(",", "").ToDecimal();
                    data.CurrentPrincipal = RI.GetText(14, 27, 12).Replace(",", "").ToDecimal();
                    data.PayoffInterest = RI.GetText(15, 27, 12).Replace(",", "").ToDecimal();
                    data.DailyInterest = RI.GetText(17, 27, 12).Replace(",", "").ToDecimal();
                    data.LateFees = RI.GetText(19, 27, 12).Replace(",", "").ToDecimal();
                }
            }
            Borrower.PData.Add(data);
            return true;
        }

        private void PayoffInformation_Shown(object sender, EventArgs e)
        {
            PayoffDataGrid.ClearSelection();
        }
    }
}