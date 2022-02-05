using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace VERFORBUH
{
    public partial class UserForm : Form
    {
        public ForbearanceDetails ForbData { get; set; }
        private DateTime today;
        public UserForm(ForbearanceDetails forbData, DateTime today, bool oneTime120 = false, bool hasSpousalLoans = false, string intentComment = "")
        {
            InitializeComponent();
            this.today = today;
            ForbData = forbData;
            CoBwrAck.Enabled = hasSpousalLoans;
            ForbearanceStartDate.Value = forbData.StartDate.Value;
            if (forbData.HadDefForb)
                ForbearanceStartDate.Enabled = false;

            ForbData.BorrowerIsDeliquent = forbData.StartDate.Value.Date != today.Date;

            int maxMonths = 6;

            for (int i = 0; i <= maxMonths; i++)
                NumberOfMonths.Items.Add(i.ToString());

            NumberOfMonths.SelectedIndex = 0;
            if (oneTime120)
            {
                Reason.Text = string.Format("Borrower is 270+ delq requesting a one-time 120 day forbearance due to financial difficulty.  {0}.", intentComment);
                Reason.Enabled = false;
                ForbearanceStartDate.Value = today.AddDays(-119);
                ForbearanceStartDate.Enabled = false;
                NumberOfMonths.Enabled = false;
            }
            else
            {
                Reason.Text = string.Format("Borrower requests verbal forbearance for {0} month(s) due to financial difficulty. {1}.", NumberOfMonths.Text, intentComment);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            List<string> errors = new List<string>();
            if (!Acknowledgement.Checked)
                errors.Add("You must select if the borrower has acknowledged their intent to repay this debt.");

            if (string.IsNullOrEmpty(Reason.Text))
                errors.Add("You must input a reason for the for the forbearance.");

            if (CoBwrAck.Enabled && !CoBwrAck.Checked)
                errors.Add("You must select if the co-borrower has acknowledged their intent to repay this debt.");

            if(!Interest_Notice_CB.Checked)
            {
                errors.Add("You must acknowledge that the borrower has been notified of interest accrual and potential capitalization by checking the checkbox at the bottom of the form.");
            }

            if (errors.Any())
            {
                Dialog.Error.Ok(string.Format("Please review the following errors: \r\n {0}", string.Join(Environment.NewLine, errors)));
                return;
            }

            ForbData.StartDate = ForbearanceStartDate.Value;
            ForbData.NumberOfMonthsRequested = NumberOfMonths.Text;
            ForbData.ForbearanceReason = Reason.Text;

            DialogResult = DialogResult.OK;
        }

        private void NumberOfMonths_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Reason.Text = string.Format("Borrower requests verbal forbearance for {0} month(s) due to financial difficulty. Borrower acknowledged intent to repay this debt.", NumberOfMonths.SelectedItem);
        }

        private void ForbearanceStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (NumberOfMonths.SelectedItem != null)
                if ((today - ForbearanceStartDate.Value).Days + (today.AddMonths(NumberOfMonths.SelectedItem.ToString().ToInt()) - today).Days > 365)
                {
                    Dialog.Error.Ok("The duration of the forbearance cannot exceed 12 months.  Please try again.");
                    NumberOfMonths.SelectedIndex = 0;
                    ForbearanceStartDate.Focus();
                }
        }
    }
}
