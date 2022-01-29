using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PAYOFF
{
    public partial class PayoffInformation : Form
    {
        public Borrower Borrower { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }
        public List<Loan> LoanOptions { get; set; }

        /// <summary>
        /// Basic constructor for testing, don't use
        /// </summary>
        public PayoffInformation(Borrower borrower)
        {
            InitializeComponent();
            Borrower = borrower;
        }

        public PayoffInformation(Borrower borrower, ReflectionInterface ri, ProcessLogRun logRun)
        {
            InitializeComponent();
            dateTimePickerPayoffDate.MinDate = DateTime.Now.Date;
            labelVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LogRun = logRun;
            DA = new DataAccess(LogRun);
            RI = ri;
            Borrower = borrower;
            accountIdentifierTextBox.Text = Borrower.Demos.Ssn;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(ValidateData())
            {
                if(LoadData())
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        public bool ValidateData()
        {
            string message = "";
            if (Borrower.Demos.Ssn.IsNullOrEmpty())
                message += "Account Number\r\n";
            if (dateTimePickerPayoffDate.Text.ToDateNullable() == null)
                message += "Payoff Date\r\n";
            if (!checkBoxPayoffAll.Checked && dataGridViewIgnoreHeader.SelectedRows.Count == 0)
                message += "Payoff All Loans or Loan Selections\r\n";
            if (message.IsPopulated())
                Dialog.Error.Ok(string.Format("The following fields are missing\r\n\r\n{0}\r\nPlease fix the fields and try again.", message));
            else
                return true;
            return false;
        }

        public bool LoadData()
        {
            if (Borrower.DemosLoaded)
            {
                Borrower.PayoffDate = dateTimePickerPayoffDate.Text;

                List<int> seqs = GetSequencesFromSelection(); //Get loans sequences selected

                foreach (int seq in seqs)
                {
                    RI.FastPath("TX3ZITS2O" + accountIdentifierTextBox.Text);
                    if (RI.MessageCode == "50108")
                    {
                        Dialog.Warning.Ok("There are no loans in TS2O for this borrower", "No Loans");
                        return false;
                    }
                    else if (RI.ScreenCode == "TSX2P")
                    {
                        RI.PutText(7, 26, dateTimePickerPayoffDate.Text.ToDate().ToString("MMddyy"));
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
            foreach (DataGridViewRow row in dataGridViewIgnoreHeader.SelectedRows)
                seqs.Add(row.Cells["LoanSequence"].Value.ToString().ToInt());
            return seqs.OrderBy(i => i).ToList();
        }

        private void accountIdentifierTextBox_TextChanged(object sender, EventArgs e)
        {
            // See CausesValidation property
            DisableControls();
            List<Loan> loans = new List<Loan>();
            dataGridViewIgnoreHeader.Controls.Clear();
            dataGridViewIgnoreHeader.DataSource = null;
            if (accountIdentifierTextBox.Text.Length >= 9)
            {
                Borrower.Demos = LoadDemos();
                if (Borrower.Demos == null)
                {
                    DisableControls();
                    Label emptyMessage = new Label();
                    emptyMessage.Text = $"Borrower Not Found";
                    Size size = new Size(325, 35);
                    LoanOptions = new List<Loan>();
                    LoadGridWithMessage(emptyMessage, size);
                }
                else
                {
                    Borrower.DemosLoaded = true;
                    loans = DA.GetLoanInformation(Borrower.Demos.AccountNumber);
                    LoanOptions = loans;
                    if (loans != null)
                        SetGridData(loans);
                }
            }
        }

        /// <summary>
        /// Disables all the controls, except the AccountIdentifier
        /// </summary>
        public void DisableControls()
        {
            dateTimePickerPayoffDate.Enabled = false;
            checkBoxPayoffAll.Enabled = false;
            checkBoxPayoffAll.Checked = false;
            dataGridViewIgnoreHeader.Enabled = false;
            buttonOK.Enabled = false;
            Borrower.DemosLoaded = false;
        }

        /// <summary>
        /// Enables all the controls, except the AccountIdentifier
        /// </summary>
        public void EnableControls()
        {
            dateTimePickerPayoffDate.Enabled = true;
            checkBoxPayoffAll.Enabled = true;
            dataGridViewIgnoreHeader.Enabled = !checkBoxPayoffAll.Checked;
            buttonOK.Enabled = true;
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
            dataGridViewIgnoreHeader.Controls.Add(emptyMessage);
        }

        /// <summary>
        /// Loads the grid view with the borrowers loan information or displays error message
        /// </summary>
        public void SetGridData(List<Loan> loans)
        {
            if (loans.Count > 0)
            {
                EnableControls();
                dataGridViewIgnoreHeader.Controls.Clear();
                dataGridViewIgnoreHeader.DataSource = loans;
                dataGridViewIgnoreHeader.ClearSelection();
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
        /// Get the borrower demographics from database
        /// </summary>
        private SystemBorrowerDemographics LoadDemos()
        {
            SystemBorrowerDemographics demos = DA.GetDemos(accountIdentifierTextBox.Text);
            if (demos != null && !demos.IsValidAddress)
            {
                Dialog.Warning.Ok("Borrower does not have a valid address.", "Invalid Address");
                accountIdentifierTextBox.Text = "";
                demos = null;
            }

            return demos;
        }

        /// <summary>
        /// Gets the Payoff calculations for the borrower.
        /// </summary>
        private bool GetPayoffData(int row, int seq)
        {
            LoanPayoffData data = new LoanPayoffData();
            if (row > 0)
            {
                RI.PutText(row, 2, "X", ReflectionInterface.Key.Enter);
                data = new LoanPayoffData();
                data.SequenceNumber = seq;
                //string loanCode = RI.GetText(row, 43, 6);
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
            Borrower.LoanPayoffRecords.Add(data);
            return true;
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

        private void PayoffInformation_Shown(object sender, EventArgs e)
        {
            dataGridViewIgnoreHeader.ClearSelection();
        }

        private void checkBoxPayoffAll_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewIgnoreHeader.Enabled = !checkBoxPayoffAll.Checked;

            if (checkBoxPayoffAll.Checked)
                dataGridViewIgnoreHeader.SelectAll();
            else
                dataGridViewIgnoreHeader.ClearSelection();
        }
    }
}
