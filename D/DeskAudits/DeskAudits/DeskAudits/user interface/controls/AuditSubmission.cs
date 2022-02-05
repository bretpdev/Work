using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace DeskAudits
{
    public partial class AuditSubmission : UserControl
    {
        DataAccess DA { get; set; }
        ProcessLogRun LogRun { get; set; }
        private string UserName { get; set; }

        public AuditSubmission()
        {
            InitializeComponent();
            SharedFieldsControl.FailReasonChanged += OtherReasonChanged;
        }

        public void InitializeValues(DataAccess da, string userName, ProcessLogRun logRun)
        {
            DA = da;
            LogRun = logRun;
            UserName = userName;
            AuditDatePicker.MaxDate = DateTime.Today;
            AuditDatePicker.Value = DateTime.Today;
            AuditTimePicker.Value = DateTime.Now;
            SharedFieldsControl.InitializeValues(DA, false, UserName);
        }

        private Audit GetSelections()
        {
            Audit audit = new Audit();
            audit.Auditee = SharedFieldsControl.GetAuditee();
            audit.Passed = SharedFieldsControl.GetAuditResult();
            audit.CommonFailReasonId = SharedFieldsControl.GetFailReason()?.FailReasonId;
            audit.CommonFailReasonDescription = SharedFieldsControl.GetFailReason()?.FailReasonDescription;
            audit.Auditor = UserName;
            audit.AuditDate = GetAuditDate();
            audit.CustomFailReasonDescription = GetCustomFailReasonDescription();

            if (!string.IsNullOrWhiteSpace(audit.CustomFailReasonDescription)) 
                audit.CommonFailReasonId = DA.GetCommonFailReasons().Where(p => p.FailReasonDescription == "Other (Fill in the blank)").Select(n => n.FailReasonId).ToList().First();

            return audit;
        }

        private DateTime GetAuditDate()
        {
            DateTime auditDate = new DateTime(AuditDatePicker.Value.Year, AuditDatePicker.Value.Month, AuditDatePicker.Value.Day, AuditTimePicker.Value.Hour, AuditTimePicker.Value.Minute, AuditTimePicker.Value.Second);
            return auditDate;
        }

        private string GetCustomFailReasonDescription()
        {
            return !string.IsNullOrWhiteSpace(OtherReasonField?.Text) ? OtherReasonField.Text : null;
        }

        private void SubmitAuditButton_Click(object sender, EventArgs e)
        {
            Audit audit = GetSelections();
            if (IsValid(audit))
            {
                if (!DA.AuditExists(audit))
                {
                    if (!DA.AddAudit(audit))
                    {
                        string errorMessage = "Error encountered on the database. Please try submitting the audit again.";
                        LogRun.AddNotification($"Error encountered trying to submit an audit by {audit.Auditor}. User notified.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                        Dialog.Warning.Ok(errorMessage);
                    }
                    else
                    {
                        Dialog.Info.Ok("Audit successfully submitted.");
                        ResetFields();
                    }
                }
                else
                    Dialog.Warning.Ok("A matching audit already exists. Audit not submitted.");
            }
        }

        private bool IsValid(Audit audit)
        {
            bool allFieldsPopulated = audit.Auditee != null && audit.Passed != null && audit.AuditDate != null && (audit.Passed.Value || (!audit.Passed.Value && audit.CommonFailReasonId != null));
            if (allFieldsPopulated && !audit.Passed.Value && audit.CommonFailReasonDescription.StartsWith("Other"))
                allFieldsPopulated &= audit.CustomFailReasonDescription != null;

            if (audit.AuditDate < DateTime.Today.AddDays(-31))
            {
                if (!Dialog.Warning.YesNo("The date you selected is more than a month in the past. Are you sure you want to use that date?"))
                    return false;
            }

            if (audit.AuditDate > DateTime.Now)
            {
                Dialog.Warning.Ok("The date you selected is in the future. Please update the date and try again.");
                return false;
            }
            
            if (!allFieldsPopulated)
            {
                Dialog.Warning.Ok("Audit not submitted. All fields must be filled out.");
            }
            return allFieldsPopulated;
        }

        private void ResetFields()
        {
            ResetText();
            SharedFieldsControl.PopulateFields(false, UserName);
            OtherReasonField.Text = "";
            AuditDatePicker.Value = DateTime.Today;
            AuditTimePicker.Value = DateTime.Now;
        }

        private void ClearFormButton_Click(object sender, EventArgs e)
        {
            ResetFields();
        }
    }
}
