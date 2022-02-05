using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SCHUPDATES
{
    public partial class SchoolInput : Form
    {
        private bool consolLoansChecked = false;
        private bool spousalLoansChecked = false;
        public SchoolData Data { get; set; }
        private List<string> DirectLoans { get; set; }
        private List<string> FFelpLoans { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }
        public bool SettingAll { get; set; }

        public SchoolInput(ProcessLogRun logRun)
        {
            InitializeComponent();
            LogRun = logRun;
            DA = new DataAccess(logRun);
            DirectLoans = new List<string>();
            FFelpLoans = new List<string>();
            LoadLoanPgms();
            LoadGuarantors();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            TX10Approval.DataSource = new object[] { "B – Education/Vocation", "C - Education", "G - Eligibility Withdrawn", "I – International", "K - Inelig for FED Loans", "O - Branch Campus Inelig", "T - Vocational", "Z - Unknown" };
            TX13Approval.DataSource = new object[] { "D – Defer Only", "E – Eligible (Disb & Defer)", "I – Ineligible" };
            TX13Reason.DataSource = new object[] { "", "A - Refused to sign", "B - Program review", "C - USDE Not Approved", "D – Didn't Return cnfrm", "E – Bankruptcy", "F - School Closed" };
            TX10Approval.SelectedIndex = -1;
            TX13Approval.SelectedIndex = -1;
            TX13Reason.SelectedIndex = -1;
        }

        private void LoadLoanPgms()
        {
            List<string> loans = new List<string>();
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
                loans.AddRange(DA.GetFedLoans());
            else
                loans.AddRange(DA.GetFfelLoanPrograms());

            LoanPgms.Items.AddRange(loans.ToArray());
        }

        private void LoadGuarantors()
        {
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
                Guarantor.Items.AddRange(new object[] { "000706", "000708", "000712", "000717", "000722", "000723", "000725", "000729", "000730", "000731", "000733", "000734", "000736", "000740", "000742", "000744", "000747", "000748", "000749", "000751", "000753", "000755", "000800", "000927", "000951" });
            else
            {
                Guarantor.Items.AddRange(new object[] { "000502" });
                Guarantor.SetItemChecked(0, true);
                Guarantor.Enabled = false;
            }
        }

        private void LoanPgms_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("SUBCNS", "UNCNS") && e.NewValue == CheckState.Checked && !consolLoansChecked)//These next set a validators ensure that if a user selects a consol loan both the sub and unsub loans get updated.
            {
                consolLoansChecked = true;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("SUBCNS"), true);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("UNCNS"), true);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("SUBCNS", "UNCNS") && e.NewValue == CheckState.Unchecked && consolLoansChecked)
            {
                consolLoansChecked = false;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("SUBCNS"), false);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("UNCNS"), false);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("SUBSPC", "UNSPC") && e.NewValue == CheckState.Checked && !spousalLoansChecked)
            {
                spousalLoansChecked = true;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("SUBSPC"), true);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("UNSPC"), true);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("SUBSPC", "UNSPC") && e.NewValue == CheckState.Unchecked && spousalLoansChecked)
            {
                spousalLoansChecked = false;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("SUBSPC"), false);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("UNSPC"), false);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("DLSCNS", "DLUCNS") && e.NewValue == CheckState.Checked && !consolLoansChecked)
            {
                consolLoansChecked = true;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLSCNS"), true);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLUCNS"), true);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("DLSCNS", "DLUCNS") && e.NewValue == CheckState.Unchecked && consolLoansChecked)
            {
                consolLoansChecked = false;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLSCNS"), false);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLUCNS"), false);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("DLSSPL", "DLUSPL") && e.NewValue == CheckState.Checked && !spousalLoansChecked)
            {
                spousalLoansChecked = true;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLSSPL"), true);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLUSPL"), true);
            }
            else if (LoanPgms.Items[e.Index].ToString().ToUpper().IsIn("DLSSPL", "DLUSPL") && e.NewValue == CheckState.Unchecked && spousalLoansChecked)
            {
                spousalLoansChecked = false;
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLSSPL"), false);
                LoanPgms.SetItemChecked(LoanPgms.Items.IndexOf("DLUSPL"), false);
            }
        }

        private void Submit_OnValidate(object sender, Uheaa.Common.WinForms.ValidationEventArgs e)
        {
            if (!ValidateInput())
                return;

            if (e.FormIsValid)
            {
                SaveData();
                DialogResult = DialogResult.OK;
            }
        }

        private void SaveData()
        {
            Data = new SchoolData()
            {
                SchoolCode = SchoolId.Text,
                MergedSchool = MergedSchool.Text.IsPopulated() ? MergedSchool.Text : "",
                MergedSchoolDate = MergedDate.Text.Replace("/", "").Trim().IsPopulated() ? MergedDate.Text.ToDateNullable() : (DateTime?)null,
                TX10Approval = TX10Approval.Text,
                TX13Approval = TX13Approval.Text,
                TX13Reason = TX13Reason.Text,
                ApprovalDate = ApprovalDate.Text.ToDate()
            };

            foreach (var item in LoanPgms.CheckedItems)
                Data.LoanPgms.Add(item.ToString());

            foreach (var item in Guarantor.CheckedItems)
                Data.Guarantors.Add(item.ToString());
        }

        private bool ValidateInput()
        {
            DateTime? date = ApprovalDate.Text.ToDateNullable();
            List<string> errors = new List<string>();
            if (SchoolId.Text.Length != 8)
                errors.Add("- The school code must be 8 characters.");
            if (MergedSchool.Text.IsPopulated() && MergedSchool.Text.Length != 8)
                errors.Add("- The Merged School code must be 8 characters.");
            if (TX10Approval.SelectedIndex == -1)
                errors.Add("- You must select the TX10 Approval Type.");
            if (TX13Approval.SelectedIndex == -1)
                errors.Add("- You must select the TX13 Approval Status.");
            if (LoanPgms.CheckedItems.Count == 0)
                errors.Add("- You must select the applicable Loan Programs.");
            if (Guarantor.CheckedItems.Count == 0)
                errors.Add("- You must select the applicable Guarantors.");
            if (date == null)
                errors.Add("- You must enter the approval date.");
            else if (date.Value > DateTime.Now)
                errors.Add("- You cannot enter a future date.");

            if (TX13Approval.Text.IsPopulated() && TX13Approval.Text.Substring(0, 1) == "I" && TX13Reason.Text.IsNullOrEmpty())
                errors.Add("- A TX13 Status Reason is required when the TX13 Approval Status is I - Ineligible");

            if (MergedSchool.Text.IsPopulated() && MergedDate.Text.Replace("/", "").Trim().IsNullOrEmpty())
            {
                errors.Add("- A date is required when a merged school code is provided.");
                MergedDate.BackColor = Color.Pink;
            }
            else if (MergedDate.Text.Replace("/", "").Trim().IsPopulated()  && MergedDate.Text.ToDateNullable().Value.Date > DateTime.Now.Date)
            {
                errors.Add("- You cannot enter a future date for merged schools.");
                MergedDate.BackColor = Color.Pink;
            }
            if (MergedSchool.Text.IsNullOrEmpty() && MergedDate.Text.Replace("/", "").Trim().IsPopulated())
            {
                errors.Add("- A merged school code is required when a merged school date is provided.");
                MergedSchool.BackColor = Color.Pink;
            }

            if (errors.Any())
            {
                Dialog.Error.Ok($"Please review the following errors: {Environment.NewLine}{Environment.NewLine} {string.Join(Environment.NewLine, errors)}");
                return false;
            }

            return true;
        }

        private void ApprovalDate_Leave(object sender, EventArgs e)
        {
            DateTime? date = ApprovalDate.Text.ToDateNullable();
        }

        private void SelectAllPgms_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingAll)
            {
                SettingAll = true;
                for (int Index = 0; Index < LoanPgms.Items.Count; Index++)
                    LoanPgms.SetItemChecked(Index, SelectAllPgms.Checked);
                SettingAll = false;
            }
        }

        private void SelectAllGuar_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingAll)
            {
                SettingAll = true;
                for (int index = 0; index < Guarantor.Items.Count; index++)
                    Guarantor.SetItemChecked(index, SelectAllGuar.Checked);
                SettingAll = false;
            }
        }

        private void MergedSchool_TextChanged(object sender, EventArgs e)
        {
            if (MergedSchool.Text.IsNullOrEmpty())
                MergedDate.BackColor = System.Drawing.SystemColors.Window;
        }

        private void MergedDate_TextChanged(object sender, EventArgs e)
        {
            if (MergedDate.Text.Replace("/", "").Trim().IsNullOrEmpty())
                MergedSchool.BackColor = System.Drawing.SystemColors.Window;
        }
    }
}