using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Q;

namespace QCDBUser
{
    public partial class frmViewQC : FormBase
    {
        private List<UserIntervention> _QCRecord;
        private EmailInfo _QCEmail;
        private EmailRecipients _QCEmailRecipients;
        private List<string> _QCIncidentUserBU = new List<string>();
        private List<String> _QCUserName;
        private List<String> _QCUserIDs;
        private List<String> _QCSubject;
         

        public bool testMode;
        public bool finished;
        public bool noIncidents;
        public bool repeat;

        public string uniqueID;
        public string userRunningScriptID;
        public string userRunningScriptName;

        public string category;
        public string urgency;

        public string reqDate;
        public string busUnit;

        public frmViewQC(string BU, bool testM, string UID)
        {
            BU = BU.Substring(0, BU.IndexOf("(") - 1);
            noIncidents = false;
            repeat = false;
            testMode = testM;
            finished = false;
            InitializeComponent();
            _QCRecord = DataAccess.GetQCIncidents(testMode, BU);
            if (_QCRecord.Count == 0)
            {
                noIncidents = true;
                DialogResult userResponse = MessageBox.Show("There were no QC issues under that business unit, would you like to search under another business unit?", "Discard Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userResponse == DialogResult.Yes)
                {
                    repeat = true;
                }
                return;
            }
            userRunningScriptID = UID; //This is the userID for the user running the script.
            _QCUserName = DataAccess.GetLoginName(testMode, UID);
            userRunningScriptName = _QCUserName.ElementAt(0).ToString();
            _QCUserName.Clear();
            busUnit = BU;


            processRecords(testM);
        }

        public void processRecords(bool testM)
        {
            if (_QCRecord.Count == 0)
            {
                finished = true;
                this.DialogResult = DialogResult.OK;
                DialogResult userResponse = MessageBox.Show("Finished processing records for this business unit, would you like to search under another business unit?", "Discard Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (userResponse == DialogResult.Yes)
                {
                    repeat = true;
                    return;
                }
                finished = true;
                this.DialogResult = DialogResult.OK;
            }

            else
            {
                foreach (UserIntervention record in _QCRecord)
                {
                    uniqueID = record.ID.ToString();
                    //record.UserID = ""; //Test Only
                    reqDate = record.RequiredDays.ToString();
                    category = record.PriorityCategory.ToString();
                    urgency = record.PriorityUrgency.ToString();

                    _QCSubject = DataAccess.GetSubject(testMode, record.ReportName);
                    txtSubjectName.Text = _QCSubject.ElementAt(0).ToString();

                    if (record.UserID == "") // then use business unit associated from record
                    {
                        _QCIncidentUserBU.Clear();
                        _QCIncidentUserBU.Add(record.BusinessUnit.ToString());
                        txtRespName.Text = "";
                        cmbUserID.Text = "";
                        txtFullName.Text = "";
                        txtReportName.Text = "";
                    }
                    else // query business unit associated with this id.
                    {
                        _QCIncidentUserBU = DataAccess.GetQCIncidentUserBU(testMode, record.UserID.ToString());

                        _QCUserName = DataAccess.GetLoginName(testMode, record.UserID);

                        if (_QCUserName.Count != 0)
                        {
                            txtRespName.Text = _QCUserName.ElementAt(0).ToString();
                            cmbUserID.Text = record.UserID;

                            _QCUserName = DataAccess.GetLoginName(testMode, record.UserID);
                            txtRespName.Text = _QCUserName.ElementAt(0).ToString();

                            _QCUserName = DataAccess.GetFullName(testMode, record.UserID);
                            txtFullName.Text = _QCUserName.ElementAt(0).ToString();

                            txtReportName.Text = record.ReportName;
                        }
                        else
                        {
                            txtRespName.Text = "";
                            cmbUserID.Text = "";
                            txtFullName.Text = "";
                            txtReportName.Text = "";
                        }



                    }
                    txtReportName.Text = record.ReportName; //This is hidden in case we need to use it later.  Original request asked for it.
                    txtActDate.Text = record.ActivityDate.ToString();
                    if (record.SavedDate != null)
                    {
                        txtSvdDate.Text = record.SavedDate.ToString();
                    }
                    else
                    {
                        txtSvdDate.Text = "";
                    }
                    txtDesc.Text = record.Description;

                    if (cmbUserID.Items.Count == 0)
                    {//Populater list.
                        _QCUserIDs = DataAccess.GetUserID(testMode);

                        foreach (string User in _QCUserIDs)
                        {
                            cmbUserID.Items.Add(User);
                        }
                    }
                    cmbUserID.SelectedItem = record.UserID;
                    _QCRecord.Remove(record);
                    return;
                    //record.processed = true;
                }
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            string body;
            string emailRecipients;
            emailRecipients = string.Empty;

            body = "Issue number not found.";
            if ((cmbUserID.SelectedIndex == -1) || (txtRespName.Text.ToString() == ""))
            {
                MessageBox.Show("You must select the appropriate UserID from the list in order to proceed.");
            }
            else
            {
                IssueInfo results = new IssueInfo();
                //DataAccess.InsertRecord(testMode, txtSubjectName.Text.ToString(), userRunningScriptName, txtSvdDate.Text.ToString(), busUnit, txtRespName.Text.ToString(), reqDate, txtActDate.Text.ToString(), txtDesc.Text.ToString(), category, urgency);
                results = DataAccess.InsertRecord(testMode, txtSubjectName.Text.ToString(), userRunningScriptName, txtSvdDate.Text.ToString(), busUnit, cmbUserID.SelectedItem.ToString(), reqDate, txtActDate.Text.ToString(), txtDesc.Text.ToString(), category, urgency);

                _QCEmail = DataAccess.GetEmailInfo(testMode, results.IssueID.ToString());

                //foreach (EmailInfo email in _QCEmail)
                //{
                body = _QCEmail.History + _QCEmail.Issue;
                //}

                _QCEmailRecipients = DataAccess.GetEmailRecipients(testMode, results.IssueID.ToString());

                //foreach (EmailRecipients recipients in _QCEmailRecipients)
                //{
                emailRecipients = _QCEmailRecipients.Recipients;
                //}

                string subject = "[" + results.Priority.ToString() + "] - " + results.IssueID.ToString() + " " + txtSubjectName.Text.ToString();
                //Common.SendMail(testMode, txtRespName.Text.ToString() + "@utahsbr.edu", "Quality Contol", subject, body, string.Empty, string.Empty, string.Empty, Common.EmailImportanceLevel.High, testMode);

                Common.SendMail(testMode, emailRecipients, "Quality Contol", " " + subject, body, string.Empty, string.Empty, string.Empty, Common.EmailImportanceLevel.High, testMode);
                
                DataAccess.DiscardRecord(testMode, uniqueID);

                processRecords(testMode);
            }
            if (finished)
            {
                this.DialogResult = DialogResult.OK;
            }
            //cmbUserID.Refresh();
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            DialogResult userResponse = MessageBox.Show("Are you sure you want to permanently remove this QC incident?", "Discard Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (userResponse == DialogResult.Yes)
            {
                DataAccess.DiscardRecord(testMode, uniqueID);
                processRecords(testMode);
            }

            if (finished)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnSaveLater_Click(object sender, EventArgs e)
        {
            DialogResult userResponse = MessageBox.Show("You will need to requeue the incident and have it reviewed later.  Do you want to proceed?", "Save for Later", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (userResponse == DialogResult.Yes)
            {
                DataAccess.SaveForLater(testMode, uniqueID, txtDesc.Text);
                processRecords(testMode);

            }
            if (finished)
            {
                this.DialogResult = DialogResult.OK;
            }
        }



        private void cmbUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            _QCUserName = DataAccess.GetLoginName(testMode, cmbUserID.SelectedItem.ToString());
            txtRespName.Text = _QCUserName.ElementAt(0).ToString();

            _QCUserName = DataAccess.GetFullName(testMode, cmbUserID.SelectedItem.ToString());
            txtFullName.Text = _QCUserName.ElementAt(0).ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
