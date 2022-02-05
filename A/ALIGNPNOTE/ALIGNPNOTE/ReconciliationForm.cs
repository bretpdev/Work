using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace ALIGNPNOTE
{
    public partial class ReconciliationForm : Form
    {
        private PNoteReconciliation Recon { get; set; }
        private bool DidCancel { get; set; }
        private bool ProcessMore { get; set; }

        public ReconciliationForm(List<BorrowerData> data, PNoteReconciliation recon)
        {
            InitializeComponent();
            SetVersion();
            Recon = recon;
            if (data.Count > 0)
                LoadForm(data);
            else
            {
                List<NoLoans> loans = new List<NoLoans>();
                loans.Add(new NoLoans() { NoLoansFound = "There were no align loans for this borrower. Click next to work the next task." });
                BorrowerView.DataSource = loans;
                Ok.Text = "Next";
            }
        }

        /// <summary>
        /// Sets the version
        /// </summary>
        private void SetVersion()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber.Text = string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        /// <summary>
        /// Loads the data into the controls on the form
        /// </summary>
        /// <param name="data"></param>
        private void LoadForm(List<BorrowerData> data)
        {
            SSN.Text = data.Select(p => p.SSN).First();
            data = data.OrderBy(p => p.Loan_Sequence).ToList();
            BorrowerView.DataSource = data;
            BorrowerView.Columns["SSN"].Visible = false;
        }

        /// <summary>
        /// Separates the selected from unselected and calls the methods to add comments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ok_Click(object sender, EventArgs e)
        {
            AddComments();
        }

        /// <summary>
        /// Determines the comment text and calls the comment method.
        /// </summary>
        private void AddComments()
        {
            List<BorrowerData> selectedData = new List<BorrowerData>();
            List<BorrowerData> unselectedData = new List<BorrowerData>();
            if (Ok.Text != "Next")
            {
                for (int i = 0; i < BorrowerView.Rows.Count; i++)
                {
                    DataGridViewRow row = BorrowerView.Rows[i];
                    if (Convert.ToBoolean(row.Cells["PNote_Found"].Value))
                    {
                        selectedData.Add(Recon.SelectedData(row));
                    }
                    else
                    {
                        unselectedData.Add(Recon.SelectedData(row));
                    }
                }
            }

            if (selectedData.Count > 0)
                Recon.AddComments(selectedData, true);
            if (unselectedData.Count > 0)
                Recon.AddComments(unselectedData, false);
            DialogResult result = Recon.CloseTask() == DialogResult.Yes ? DialogResult.OK : DialogResult.Cancel;
            if (result == DialogResult.Cancel) DidCancel = true;
            else ProcessMore = true;
            this.DialogResult = result;
        }

        /// <summary>
        /// Asks the user if they want to cancel and Informs them that there is a task still assigned to them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            DidCancel = true;
            CancelApp();
        }

        /// <summary>
        /// Asks if they want to cancel and informs the user that the task needs to be closed manually.
        /// </summary>
        private void CancelApp()
        {
            if (MessageBox.Show("Are you sure you want to Cancel?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("There is a WY/PN task assigned to you that will need to be manually unassigned.", "Task Assigned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Captures the form closing event and asks if they want to cancel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReconciliationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DidCancel && !ProcessMore)
                CancelApp();
        }
    }
}