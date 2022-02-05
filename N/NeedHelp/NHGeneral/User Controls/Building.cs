using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Word = Microsoft.Office.Interop.Word;

namespace NHGeneral
{
    partial class Building : MiddleClassBetweenBaseAndControls
    {
        private List<SqlUser> _usersListNotChanged;
        List<TextValueOption> Categories { get; set; }
        List<TextValueOption> Urgencies { get; set; }
        public ToolTip Tip { get; set; }

        public Building(Ticket activeTicket, List<TextValueOption> categoryListForFacilities, List<TextValueOption> urgencyListForFacilities, List<SqlUser> userList, ProcessLogRun logRun)
        {
            InitializeComponent();
            LogRun = logRun;
            dtpReqDate.MinDate = activeTicket.Data.TheTicketData.Required;
            dtpReqDate.Value = activeTicket.Data.TheTicketData.Required;
            dtpRequestDate.MinDate = activeTicket.Data.TheTicketData.Requested;
            dtpRequestDate.Value = activeTicket.Data.TheTicketData.Requested;
            List<SqlUser> users = new List<SqlUser>(userList);
            _usersListNotChanged = new List<SqlUser>(userList);
            TheTicket = activeTicket;
            SetPriorityDropDownOptions(activeTicket, categoryListForFacilities, urgencyListForFacilities);
            users.Insert(0, new SqlUser());
            cboCourt.DataSource = new List<SqlUser>(users);
            cboCourt.SelectedItem = activeTicket.Data.TheTicketData.Court == null || activeTicket.Data.TheTicketData.Court.FirstName == null ? null : userList.Where(p => p.ID == activeTicket.Data.TheTicketData.Court.ID).SingleOrDefault();
            cboAssignedTo.DataSource = new List<SqlUser>(users);
            cboAssignedTo.SelectedItem = activeTicket.Data.TheTicketData.AssignedTo == null || activeTicket.Data.TheTicketData.AssignedTo.FirstName == null ? null : userList.Where(p => p.ID == activeTicket.Data.TheTicketData.AssignedTo.ID).SingleOrDefault();
            cboRequester.DataSource = new List<SqlUser>(users);
            cboRequester.SelectedItem = activeTicket.Data.TheTicketData.Requester == null || activeTicket.Data.TheTicketData.Requester.FirstName == null ? null : userList.Where(p => p.ID == activeTicket.Data.TheTicketData.Requester.ID).SingleOrDefault();
            ticketDataBindingSource.DataSource = activeTicket.Data.TheTicketData;
            foreach (SqlUser item in activeTicket.Data.TheTicketData.UserSelectedEmailRecipients)
                lbxEmailRecip.Items.Add(item.LegalName);
            SetToolTips();
        }

        private void SetToolTips()
        {
            Tip = new ToolTip();
            Tip.SetToolTip(lnkExport, "Exports the history into a PDF file.");
            Tip.SetToolTip(txtIssue, "Initial issue explaining what the ticket is needed for. DO NOT ADD SSN TO ISSUE.");
            Tip.SetToolTip(txtUpdate, "Adds any updates to the history of the ticket. DO NOT ADD SSN TO UPDATE.");
            Tip.SetToolTip(cboUrgency, "The urgency of the ticket to determine priority, not available for BAC ticket type, Automatic 9 priority.");
            Tip.SetToolTip(cboCategory, "The category of the ticket to determine priority, not available for BAC ticket type, Automatic 9 priority.");
        }

        private void SetPriorityDropDownOptions(Ticket activeTicket, List<TextValueOption> categoryListForFacilities, List<TextValueOption> urgencyListForFacilities)
        {
            if (activeTicket.Data.TheTicketData.TicketCode.ToLower() != "bac")
            {
                Categories = new List<TextValueOption>(categoryListForFacilities);
                Urgencies = new List<TextValueOption>(urgencyListForFacilities);
                cboCategory.DataSource = Categories;
                cboCategory.DisplayMember = "DisplayText";
                cboCategory.ValueMember = "BackgroundValue";
                cboUrgency.DataSource = Urgencies;
                cboUrgency.DisplayMember = "DisplayText";
                cboUrgency.ValueMember = "BackgroundValue";
            }
            else
            {
                cboCategory.Enabled = false;
                cboUrgency.Enabled = false;
            }
        }

        public override void BindNewTicket(Ticket activeTicket)
        {
            TheTicket = activeTicket;
            ticketDataBindingSource.DataSource = activeTicket.Data.TheTicketData;
            cboCategory.Text = TheTicket.Data.TheTicketData.CatOption;
            cboUrgency.Text = TheTicket.Data.TheTicketData.UrgencyOption;
            if (TheTicket.Data.TheTicketData.AssignedTo != null) { cboAssignedTo.Text = TheTicket.Data.TheTicketData.AssignedTo.FirstName + " " + TheTicket.Data.TheTicketData.AssignedTo.LastName; }
            else { cboAssignedTo.Text = ""; }
            if (TheTicket.Data.TheTicketData.Court != null) { cboCourt.Text = TheTicket.Data.TheTicketData.Court.FirstName + " " + TheTicket.Data.TheTicketData.Court.LastName; }
            else { cboCourt.Text = ""; }
            if (TheTicket.Data.TheTicketData.Requester != null) { cboRequester.Text = TheTicket.Data.TheTicketData.Requester.FirstName + " " + TheTicket.Data.TheTicketData.Requester.LastName; }
            else { cboRequester.Text = ""; }
        }

        private void LnkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadHistoryData();
                string saveAs = string.Format("{0}NH - {1}_History_{2}.pdf", EnterpriseFileSystem.TempFolder, TheTicket.Data.TheTicketData.TicketID, Guid.NewGuid().ToBase64String());
                CreatePdfDocument(saveAs);
                string message = "The report is located at:\r\n\r\n" + saveAs + ".\r\n\r\nDo you want to open the report?";
                if (MessageBox.Show(message, "Open Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    Process.Start(saveAs);
            }
            catch (IOException)
            {
                MessageBox.Show("You currently have this file open. Please close the NH - " + TheTicket.Data.TheTicketData.TicketID + "_History.pdf before exporting a new one", "File Already Open", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CreatePdfDocument(string saveAs)
        {
            Word.Application wordApp = new Word.Application();
            wordApp.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
            string letterId = "Need Help Ticket History.docx";
            object fileName = EnterpriseFileSystem.GetPath("NeedHelpForms") + letterId;
            object missing = System.Type.Missing;
            object refFalse = false;
            object refTrue = true;
            object mergeType = Word.WdMergeSubType.wdMergeSubTypeOther;
            object pause = false;
            object saveAsPath = saveAs;
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            var doc = wordApp.Application.Documents.Open(ref fileName, ref missing, ref refTrue, ref refFalse, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            var temp = wordApp.ActiveDocument.MailMerge;
            wordApp.ActiveDocument.MailMerge.OpenDataSource(DataFile, ref missing, ref missing, ref missing, ref refTrue, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref mergeType);
            wordApp.ActiveDocument.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
            wordApp.ActiveDocument.MailMerge.Execute(ref pause);
            while (true)
            {
                try
                {
                    wordApp.ActiveDocument.SaveAs(ref saveAsPath, ref format, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            object noSave = Word.WdSaveOptions.wdDoNotSaveChanges;
            doc.Close(ref noSave);
            Marshal.FinalReleaseComObject(doc);
            (wordApp.Application).Quit(ref noSave, ref missing, ref missing);
            Repeater.TryRepeatedly(() => File.Delete(DataFile));
        }

        public override TicketData GetModifiedTicketData(Ticket activeTicket, List<SqlUser> userList, NotifyType.Type type)
        {
            TicketData theTicket = new TicketData();
            if (cboAssignedTo.Text.Trim() != "") { theTicket.AssignedTo = userList.Where(p => p.LegalName.ToUpper() == cboAssignedTo.Text.ToUpper()).FirstOrDefault(); }
            theTicket.CatOption = cboCategory.Text;
            if (cboCourt.Text.Trim() != "") { theTicket.Court = userList.Where(p => p.LegalName.ToUpper() == cboCourt.Text.ToUpper()).FirstOrDefault(); }
            theTicket.CourtDate = Convert.ToDateTime(dtpCourtDate.Text);
            foreach (string item in lbxEmailRecip.Items)
            {
                SqlUser user = GetUSingleUser(item, _usersListNotChanged);
                if (user != null)
                    theTicket.UserSelectedEmailRecipients.Add(user);
            }
            theTicket.History = txtHistory.Text;
            theTicket.Issue = txtIssue.Text;
            theTicket.IssueUpdate = txtUpdate.Text;
            theTicket.PreviousCourt = activeTicket.Data.TheTicketData.PreviousCourt;
            theTicket.PreviousStatus = activeTicket.Data.TheTicketData.PreviousStatus;
            theTicket.Priority = activeTicket.Data.TheTicketData.Priority;
            theTicket.Requested = Convert.ToDateTime(dtpRequestDate.Text);
            if (cboRequester.Text.Trim() != "") { theTicket.Requester = userList.Where(p => p.LegalName.ToUpper() == cboRequester.Text.ToUpper()).FirstOrDefault(); }
            theTicket.Required = Convert.ToDateTime(dtpReqDate.Text);
            theTicket.Status = txtStatus.Text;
            theTicket.StatusDate = Convert.ToDateTime(dtpStatusDate.Text);
            theTicket.Subject = txtSubject.Text;
            theTicket.TicketCode = activeTicket.Data.TheTicketData.TicketCode;
            theTicket.TicketID = activeTicket.Data.TheTicketData.TicketID;
            theTicket.UrgencyOption = cboUrgency.Text;
            return theTicket;
        }

        public override void SetPriority()
        {
            //Is only called when ticket type is BAC, it defaults to a 9.
            TheTicket.Data.TheTicketData.Priority = 9;
        }

        private void CboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnCategoryChanged(sender, new PriorityOptionEventArgs(cboCategory.Text, cboUrgency.Text, TheTicket.Data.TheTicketData.TicketCode));
        }

        private void CboUrgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnUrgencyChanged(sender, new PriorityOptionEventArgs(cboCategory.Text, cboUrgency.Text, TheTicket.Data.TheTicketData.TicketCode));
        }

        public override void SetIssueToReadOnly(bool isReadOnly)
        {
            if (isReadOnly) { txtIssue.ReadOnly = true; }
            else { txtIssue.ReadOnly = false; }
        }

        public override void SetEmailRecipientDataSource(List<SqlUser> emailUser)
        {
            lbxEmailRecip.Items.Clear();
            foreach (SqlUser user in emailUser)
            {
                lbxEmailRecip.Items.Add(user.LegalName);
            }
        }

        private void RemoveRecipient_Click(object sender, EventArgs e)
        {
            List<SqlUser> usersRemoved = new List<SqlUser>();
            foreach (string item in lbxEmailRecip.SelectedItems)
            {
                SqlUser user = _usersListNotChanged.Where(p => p.LegalName == item).SingleOrDefault();
                TheTicket.Data.TheTicketData.UserSelectedEmailRecipients.Remove(user);
                usersRemoved.Add(user);
            }
            while (lbxEmailRecip.SelectedItems.Count > 0)
            {
                lbxEmailRecip.Items.Remove(lbxEmailRecip.SelectedItem);
            }
            OnEmailRecipientChanged(sender, new EmailRecipientEvantArgs(usersRemoved));
        }

        private void CboCourt_Leave(object sender, EventArgs e)
        {
            if (cboCourt.Text.Trim() != "")
            {
                string court = _usersListNotChanged.Where(p => p.LegalName.ToUpper() == cboCourt.Text.Trim().ToUpper()).Select(r => r.LegalName).FirstOrDefault();
                if (court != null && (court.ToUpper() == cboCourt.Text.Trim().ToUpper()))
                    cboCourt.Text = court;
                else
                    cboCourt.Text = "";
            }
        }

        private void CboCategory_Leave(object sender, EventArgs e)
        {
            if (!cboCategory.Text.IsIn(Categories.Select(p => p.DisplayText).ToArray()))
                cboCategory.Text = "";
        }

        private void CboUrgency_Leave(object sender, EventArgs e)
        {
            if (!cboUrgency.Text.IsIn(Urgencies.Select(p => p.DisplayText).ToArray()))
                cboUrgency.Text = "";
        }
    }
}