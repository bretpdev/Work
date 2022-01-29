using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.WinForms;
using Uheaa.Common;

namespace KEYIDENCHN
{
    public partial class KeyIdentifierUpdateForm : Form
    {
        public KeyIdentifierUpdateForm(AccountInfo accountInfo, SupervisorInfo supervisorInfo)
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            InitValidation();

            this.accountInfo = accountInfo;
            this.supervisorInfo = supervisorInfo;
            LoadAccount();
        }

        private AccountInfo accountInfo;
        private SupervisorInfo supervisorInfo;
        private void LoadAccount()
        {
            NameOnRecordBox.Text = accountInfo.NameOnRecord;
            if (!string.IsNullOrEmpty(accountInfo.DOB))
                DobOnRecordBox.Text = accountInfo.DOB;
            if (!string.IsNullOrEmpty(accountInfo.AccountNumber))
                AcctOnRecordBox.Text = accountInfo.AccountNumber;
            if (supervisorInfo != null)
            {
                FirstNameBox.Text = supervisorInfo.FirstName;
                MiddleNameBox.Text = supervisorInfo.MiddleName;
                LastNameBox.Text = supervisorInfo.LastName;
                SuffixBox.Text = supervisorInfo.Suffix;
                DobBox.Text = supervisorInfo.DOB;
                RejectButton.Visible = ApproveButton.Visible = true;
                BeginVerificationButton.Visible = CancelVerificationButton.Visible = false;
            }
            MiddleNameCheck.Visible = !string.IsNullOrEmpty(accountInfo.MiddleName);
            SuffixCheck.Visible = !string.IsNullOrEmpty(accountInfo.Suffix);
        }

        private void MiddleNameCheck_CheckedChanged(object sender, EventArgs e)
        {
            MiddleNameBox.Enabled = !MiddleNameCheck.Checked;
            UnfocusLabel.Focus();
            ValidateInput();
        }

        private void SuffixCheck_CheckedChanged(object sender, EventArgs e)
        {
            SuffixBox.Enabled = !SuffixCheck.Checked;
            UnfocusLabel.Focus();
            ValidateInput();
        }

        private void InputBox_ValidationOnLeave(object sender, SimpleValidationEventArgs e)
        {
            if (this.Controls.Filter<OmniTextBox>().All(o => o.IsValid != false))
                BeginVerificationButton.ForeColor = Color.Black;
            var textbox = (sender as TextBox);
            var error = pendingResults.IsNull(p => p.Errors.SingleOrDefault(o => o.FieldName == fieldNames[textbox]));
            if (verifyInfo != null && !string.IsNullOrEmpty(textbox.Text) && textbox.Text == validationSettings[textbox].GetString()) //only check these settings on the second step
            {
                e.Valid = false;
                e.ValidationMessage = validationSettings[textbox].ErrorMessage;
                if (!inVerifyMode)
                {
                    BeginVerificationButton.ForeColor = Color.Red;
                }
            }
            else if (error != null)
            {
                e.Valid = false;
                e.ValidationMessage = error.ErrorMessage;
            }
            else
                e.Valid = true;
        }

        private void DobBox_ValidationOnLeave(object sender, SimpleValidationEventArgs e)
        {
            if (DobBox.Text.ToDateNullable() > DateTime.Now)
            {
                e.Valid = false;
                e.ValidationMessage = "DOB " + DobBox.Text + " is in the future.";
            }
            else
                InputBox_ValidationOnLeave(sender, e);
        }

        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        string pendingMessage = null;
        VerificationResults pendingResults = null;
        private void ValidateInput()
        {
            RejectButton.ForeColor = ApproveButton.ForeColor = Color.Black;
            pendingMessage = null;
            pendingResults = null;
            bool notSupervisor = supervisorInfo == null;
            bool firstOrDobEntered = !string.IsNullOrEmpty(FirstNameBox.Text.Trim() + DobBox.Text.Replace("/", "").Trim());
            bool notAllowed = notSupervisor && firstOrDobEntered;
            bool anyChanges = (FirstNameBox.Text + DobBox.Text + LastNameBox.Text + MiddleNameBox.Text + SuffixBox.Text).Trim().Length > 0;
            if (MiddleNameCheck.Checked || SuffixCheck.Checked) anyChanges = true;

            if (!inVerifyMode && supervisorInfo == null)
            {
                BeginVerificationButton.Enabled = anyChanges;
            }
            else if (verifyInfo != null)
            {
                pendingResults = verifyInfo.Verify(this);
                if (!pendingResults.ValidEntry)
                {
                    var errors = pendingResults.Errors.Select(o => o.ErrorMessage).ToList();
                    foreach (var textbox in validationSettings.Keys)
                        if (!string.IsNullOrEmpty(textbox.Text) && textbox.Text == validationSettings[textbox].GetString())
                            errors.Add(validationSettings[textbox].ErrorMessage);
                    pendingMessage = string.Join(Environment.NewLine, errors.ToArray());
                    RejectButton.ForeColor = ApproveButton.ForeColor = Color.Red;
                }
                else
                    pendingResults = null;
            }
        }

        Dictionary<TextBox, ValidationSetting> validationSettings = new Dictionary<TextBox, ValidationSetting>();
        Dictionary<TextBox, string> fieldNames = new Dictionary<TextBox, string>();
        private void InitValidation()
        {
            Action<TextBox, Func<string>, string> add = new Action<TextBox, Func<string>, string>(
                (t, f, s) =>
                {
                    validationSettings[t] = new ValidationSetting(f, s);
                    fieldNames[t] = s;
                }
            );
            add(FirstNameBox, () => accountInfo.FirstName, FieldNames.FirstName);
            add(MiddleNameBox, () => accountInfo.MiddleName, FieldNames.MiddleName);
            add(LastNameBox, () => accountInfo.LastName, FieldNames.LastName);
            add(SuffixBox, () => accountInfo.Suffix, FieldNames.Suffix);
            add(DobBox, () => accountInfo.DOB, FieldNames.DOB);
        }
        class ValidationSetting
        {
            public string ErrorMessage { get { return string.Format("Entered {0} is identical to {0} on Record.", FieldName); } }
            public Func<string> GetString { get; set; }
            public string FieldName { get; set; }
            public ValidationSetting(Func<string> getString, string fieldName)
            {
                GetString = getString;
                FieldName = fieldName;
            }
        }

        private void BeginVerificationButton_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();
            foreach (var box in this.Controls.Filter<OmniTextBox>())
                if (box.IsValid == false)
                    messages.Add(box.ValidationMessage);
            if (messages.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, messages.ToArray()));
                return;
            }
            VerifyMode();
        }

        private void CancelVerificationButton_Click(object sender, EventArgs e)
        {
            NormalMode();
        }

        bool inVerifyMode = false;
        VerificationInfo verifyInfo = null;
        private void VerifyMode()
        {
            verifyInfo = VerificationInfo.FromForm(this);
            VerifyToggle(true);
            new VerificationInfo().PopulateForm(this);//load fields with empty information
        }

        private void NormalMode()
        {
            VerifyToggle(false);
            verifyInfo.PopulateForm(this);//revert back to original info
            verifyInfo = null;
        }

        private void VerifyToggle(bool verify)
        {
            inVerifyMode = verify;
            CancelVerificationButton.Visible = RejectButton.Visible = ApproveButton.Visible = verify;
            if (verify)
            {
                if (verifyInfo.AdminRequired)
                    ApproveButton.Text = "Supervisor Review";
                else
                    ApproveButton.Text = "Approve";
            }
            BeginVerificationButton.Visible = !verify;
        }


        public VerificationInfo VerifiedChanges { get; private set; }
        public Supervisor ReviewSupervisor { get; private set; }
        public bool Approval { get; set; }
        private void ApproveButton_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();
            foreach (var box in this.Controls.Filter<OmniTextBox>())
                if (box.IsValid == false)
                    messages.Add(box.ValidationMessage);
            if (messages.Any())
            {
                messages.Add(pendingMessage);
                MessageBox.Show(string.Join(Environment.NewLine, messages.ToArray()));
                return;
            }
            if (supervisorInfo == null && VerificationInfo.FromForm(this).AdminRequired) //not supervisor
            {
                using (var selector = new SupervisorSelector())
                {
                    if (selector.ShowDialog() == DialogResult.OK)
                        ReviewSupervisor = selector.SelectedSupervisor;
                    else
                        return;
                }
            }
            VerifiedChanges = VerificationInfo.FromForm(this);
            Approval = true;
            this.DialogResult = DialogResult.OK;
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (!pendingMessage.IsNullOrEmpty())
            {
                MessageBox.Show(pendingMessage);
                return;
            }
            Approval = false;
            this.DialogResult = DialogResult.OK;
        }
    }
}
