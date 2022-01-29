using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeskAudits
{
    public partial class SharedFields : UserControl
    {
        DataAccess DA { get; set; }
        public bool IsOtherReasonSelected { get; private set; } = false;
        public event EventHandler<bool> FailReasonChanged;

        public SharedFields()
        {
            InitializeComponent();
        }

        public void InitializeValues(DataAccess da, bool isSearch, string userName)
        {
            DA = da;
            PopulateFields(isSearch, userName);
        }

        public void PopulateFields(bool isSearch, string userName)
        {
            PopulateAuditee(isSearch, userName);
            PopulateFailReasons();
            PopulateResult();
            PopulateAuditor(isSearch, userName);
            IsOtherReasonSelected = false;
        }

        private void PopulateAuditor(bool isSearch, string userName)
        {
            if (!isSearch)
            {
                List<string> auditor = new List<string>() { userName };
                AuditorField.DataSource = auditor;
                AuditorField.SelectedIndex = auditor.IndexOf(userName);
                AuditorField.Enabled = false;
            }
            else
            {
                List<string> auditors = new List<string>();
                auditors.Add("");
                auditors.AddRange(DA.GetLoggedAuditors());
                AuditorField.DataSource = auditors;
                AuditorField.SelectedIndex = auditors.IndexOf("");
                AuditorField.Enabled = true;
            }
        }

        private void PopulateAuditee(bool isSearch, string userName)
        {
            List<string> agents = new List<string>();
            agents.Add("");
            if (isSearch)
                agents.AddRange(DA.GetLoggedAuditees());
            else
            {
                agents.AddRange(DA.GetPossibleAuditees(userName)?.OrderBy(p => p.LastName).Select(n => n.FirstName + " " + n.LastName).ToList());
            }
            AuditeeField.DataSource = agents;
            AuditeeField.SelectedIndex = agents.IndexOf("");
        }

        private void PopulateResult()
        {
            ResultField.DataSource = new List<string>() { "", "Pass", "Fail" };
        }

        private void PopulateFailReasons()
        {
            FailReasonField.DisplayMember = "FailReasonDescription";
            FailReasonField.ValueMember = "FailReasonId";
            List<FailReason> failReasons = new List<FailReason>();
            FailReason placeholder = new FailReason { FailReasonId = null, FailReasonDescription = "" };
            failReasons.Add(placeholder);
            failReasons.AddRange(DA.GetCommonFailReasons());
            FailReasonField.DataSource = failReasons;
            FailReasonField.SelectedIndex = failReasons.IndexOf(placeholder);
        }

        public string GetAuditee()
        {
            if (string.IsNullOrWhiteSpace(AuditeeField.Text))
                return null;
            return AuditeeField.Text;
        }

        public FailReason GetFailReason()
        {
            return (FailReason)FailReasonField.SelectedItem;
        }

        public bool? GetAuditResult()
        {
            if (string.IsNullOrWhiteSpace(ResultField.Text))
                return null;
            return ResultField.Text == "Pass";
        }

        public string GetAuditor()
        {
            if (string.IsNullOrWhiteSpace(AuditorField.Text))
                return null;
            return AuditorField.Text;
        }

        private void ResultField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResultField.Text == "Pass")
            {
                List<FailReason> failReasons = (List<FailReason>)FailReasonField.DataSource;
                FailReasonField.SelectedIndex = failReasons.IndexOf(new FailReason { FailReasonId = null, FailReasonDescription = "" });
                FailReasonField.Enabled = false;
            }
            else if (!FailReasonField.Enabled)
                FailReasonField.Enabled = true;
        }

        private void FailReasonField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResultField.DataSource != null)
            {
                List<FailReason> failReasons = (List<FailReason>)FailReasonField.DataSource;
                List<string> results = (List<string>)ResultField.DataSource;
                if (FailReasonField.SelectedIndex != failReasons.IndexOf(new FailReason { FailReasonId = null, FailReasonDescription = "" }))
                    ResultField.SelectedIndex = results.IndexOf("Fail");

                if (FailReasonField.Text != null && FailReasonField.Text.StartsWith("Other"))
                    IsOtherReasonSelected = true;
                else
                    IsOtherReasonSelected = false;
            }

            OnFailReasonChanged();
        }

        protected virtual void OnFailReasonChanged()
        {
            FailReasonChanged?.Invoke(this, IsOtherReasonSelected);
        }
    }
}
