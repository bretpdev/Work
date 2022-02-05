using SubSystemShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Runtime.InteropServices;
using Uheaa.Common;

namespace NHGeneral
{
    partial class ChangeRequest : MiddleClassBetweenBaseAndControls
    {
        private string _formsFolder = EnterpriseFileSystem.GetPath("NeedHelpForms");
        private List<SqlUser> _usersListNotChanged;
        List<string> Categories { get; set; }
        List<string> Urgencies { get; set; }
        public ToolTip Tip { get; set; }

        public ChangeRequest(Ticket activeTicket, List<BusinessUnit> businessUnit, List<string> categoryList, List<string> systemList, List<string> urgencyList, List<SqlUser> userList, ProcessLogRun logRun)
        {
            InitializeComponent();
            LogRun = logRun;
            dtpReqDate.MinDate = activeTicket.Data.TheTicketData.Required;
            dtpReqDate.Value = activeTicket.Data.TheTicketData.Required;
            dtpRequestDate.MinDate = activeTicket.Data.TheTicketData.Requested;
            dtpRequestDate.Value = activeTicket.Data.TheTicketData.Requested;
            List<BusinessUnit> businessList = new List<BusinessUnit>(businessUnit);
            Categories = new List<string>(categoryList);
            Urgencies = new List<string>(urgencyList);
            List<string> systems = new List<string>(systemList);
            List<SqlUser> users = new List<SqlUser>(userList);
            _usersListNotChanged = new List<SqlUser>(userList);

            TheTicket = activeTicket;

            AttachForms();
            GetAttachedForms();

            businessList.Insert(0, new BusinessUnit());
            cboBussUnit.DataSource = businessList;
            cboBussUnit.DisplayMember = "Name";
            cboBussUnit.ValueMember = "ID";
            cboCategory.DataSource = Categories;
            cboBussUnit.SelectedItem = activeTicket.Data.TheTicketData.Unit == null ? null : businessList.Where(b => b.ID == activeTicket.Data.TheTicketData.Unit.ID).SingleOrDefault();
            systems.Insert(0, string.Empty);
            cboSystem.DataSource = systems;
            cboSystem.SelectedItem = activeTicket.Data.TheTicketData.Systems.Count == 0 ? null : systems.Where(s => s.ToString() == activeTicket.Data.TheTicketData.Systems[0]).SingleOrDefault();
            cboUrgency.DataSource = Urgencies;
            users.Insert(0, new SqlUser());
            cboCourt.DataSource = new List<SqlUser>(users);
            cboCourt.SelectedItem = activeTicket.Data.TheTicketData.Court == null || activeTicket.Data.TheTicketData.Court.FirstName == null ? null : userList.Where(p => p.ID == activeTicket.Data.TheTicketData.Court.ID).SingleOrDefault();
            cboAssignedTo.DataSource = new List<SqlUser>(users);
            cboAssignedTo.SelectedItem = activeTicket.Data.TheTicketData.AssignedTo == null || activeTicket.Data.TheTicketData.AssignedTo.FirstName == null ? null : userList.Where(p => p.ID == activeTicket.Data.TheTicketData.AssignedTo.ID).SingleOrDefault();
            cboRequester.DataSource = new List<SqlUser>(users);
            cboRequester.SelectedItem = activeTicket.Data.TheTicketData.Requester == null || activeTicket.Data.TheTicketData.Requester.FirstName == null ? null : userList.Where(p => p.ID == activeTicket.Data.TheTicketData.Requester.ID).SingleOrDefault();
            ticketDataBindingSource.DataSource = activeTicket.Data.TheTicketData;
            foreach (SqlUser item in activeTicket.Data.TheTicketData.UserSelectedEmailRecipients)
            {
                lbxEmailRecip.Items.Add(item.LegalName);
            }
            SetToolTips();
        }

        private void SetToolTips()
        {
            Tip = new ToolTip();
            Tip.SetToolTip(lnkExport, "Exports the history into a PDF file.");
            Tip.SetToolTip(txtIssue, "Initial issue explaining what the ticket is needed for. DO NOT ADD SSN TO ISSUE.");
            Tip.SetToolTip(txtUpdate, "Adds any updates to the history of the ticket. DO NOT ADD SSN TO UPDATE.");
            Tip.SetToolTip(cboUrgency, "The urgency of the ticket to determine priority.");
            Tip.SetToolTip(cboCategory, "The category of the ticket to determine priority.");
        }

        public override void BindNewTicket(Ticket activeTicket)
        {
            TheTicket = activeTicket;
            ticketDataBindingSource.DataSource = activeTicket.Data.TheTicketData;
            cboBussUnit.SelectedItem = TheTicket.Data.TheTicketData.Unit;
            cboCategory.Text = TheTicket.Data.TheTicketData.CatOption;
            cboUrgency.Text = TheTicket.Data.TheTicketData.UrgencyOption;
            txtQC.Text = TheTicket.Data.TheTicketData.RelatedQCIssues;
            if (TheTicket.Data.TheTicketData.Systems.Count > 0) { cboSystem.Text = TheTicket.Data.TheTicketData.Systems[0]; }
            if (TheTicket.Data.TheTicketData.AssignedTo != null) { cboAssignedTo.Text = TheTicket.Data.TheTicketData.AssignedTo.FirstName + " " + TheTicket.Data.TheTicketData.AssignedTo.LastName; }
            else { cboAssignedTo.Text = ""; }
            if (TheTicket.Data.TheTicketData.Court != null) { cboCourt.Text = TheTicket.Data.TheTicketData.Court.FirstName + " " + TheTicket.Data.TheTicketData.Court.LastName; }
            else { cboCourt.Text = ""; }
            if (TheTicket.Data.TheTicketData.Requester != null) { cboRequester.Text = TheTicket.Data.TheTicketData.Requester.FirstName + " " + TheTicket.Data.TheTicketData.Requester.LastName; }
            else { cboRequester.Text = ""; }
            GetAttachedForms();
        }

        private void lnkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
            theTicket.Comments = txtOtherComments.Text;
            if (cboCourt.Text.Trim() != "") { theTicket.Court = userList.Where(p => p.LegalName.ToUpper() == cboCourt.Text.ToUpper()).FirstOrDefault(); }
            theTicket.CourtDate = dtpCourtDate.Value.Date;
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
            theTicket.RelatedQCIssues = txtQC.Text;
            theTicket.Requested = dtpRequestDate.Value.Date;
            if (cboRequester.Text.Trim() != "") { theTicket.Requester = userList.Where(p => p.LegalName.ToUpper() == cboRequester.Text.ToUpper()).FirstOrDefault(); }
            theTicket.Required = dtpReqDate.Value.Date;
            theTicket.Status = txtStatus.Text;
            theTicket.StatusDate = dtpStatusDate.Value.Date;
            theTicket.Subject = txtSubject.Text;
            theTicket.Systems.Add(cboSystem.Text);
            theTicket.TicketCode = activeTicket.Data.TheTicketData.TicketCode;
            theTicket.TicketID = activeTicket.Data.TheTicketData.TicketID;
            theTicket.Unit = cboBussUnit.SelectedItem as BusinessUnit;
            theTicket.UrgencyOption = cboUrgency.Text;
            foreach (AttachedFile item in lbxCurrentFiles.Items.Cast<AttachedFile>())
            {
                theTicket.AttachedFiles.Add(item.FileName);
            }
            return theTicket;
        }

        private void GetAttachedForms()
        {
            lbxCurrentFiles.Items.Clear();
            string folder = "";
            folder = string.Format("{0} - {1}", EnterpriseFileSystem.GetPath("NHUHTicket"), TheTicket.Data.TheTicketData.TicketID);
            lbxCurrentFiles.DisplayMember = "DisplayText";
            if (Directory.Exists(folder))
                lbxCurrentFiles.Items.AddRange(Directory.GetFiles(folder).Select(f => new AttachedFile(f)).ToArray());
        }

        private void AttachForms()
        {
            _formsFolder += TheTicket.Data.TheTicketData.TicketCode.ToUpper();
            if (!Directory.Exists(_formsFolder))
                Directory.CreateDirectory(_formsFolder);
            foreach (string item in Directory.GetFiles(_formsFolder, "*.*"))
            {
                int index = item.LastIndexOf('\\');
                lbxForms.Items.Add(item.Substring(index + 1));
            }
        }

        private void LbxForms_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbxForms.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                string filePath = _formsFolder + "\\" + lbxForms.Items[index];
                string copyPath = EnterpriseFileSystem.TempFolder + lbxForms.Items[index];
                try
                {
                    File.Copy(filePath, copyPath, true);
                    Process.Start(copyPath);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please close '" + lbxForms.Items[index] + "' before opening another blank copy.");
                }
            }
        }

        private void LbxCurrentFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbxCurrentFiles.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var selected = (AttachedFile)lbxCurrentFiles.SelectedItem;
                string filePath = $"{EnterpriseFileSystem.GetPath("NHUHTicket")} - {TheTicket.Data.TheTicketData.TicketID}\\{selected.FileName}";
                string copyPath = $"{EnterpriseFileSystem.TempFolder}{selected.FileName}";
                if (!File.Exists(filePath))
                    MessageBox.Show("Need Help was unable to find the specified file.  Please submit a ticket to have this issue resolved.");

                try
                {
                    File.Copy(filePath, copyPath, true);
                    Process.Start(copyPath);
                }
                catch (Exception)
                {
                    MessageBox.Show("Please close '" + selected.FileName + "' before opening another blank copy.");
                }
            }
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

        public override void AddUploadedFile(AttachedFile file)
        {
            lbxCurrentFiles.Items.Add(file);
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
            if (!cboCategory.Text.IsIn(Categories.Select(p => p.ToString()).ToArray()))
                cboCategory.Text = "";
        }

        private void CboUrgency_Leave(object sender, EventArgs e)
        {
            if (!cboUrgency.Text.IsIn(Urgencies.Select(p => p.ToString()).ToArray()))
                cboUrgency.Text = "";
        }
    }
}