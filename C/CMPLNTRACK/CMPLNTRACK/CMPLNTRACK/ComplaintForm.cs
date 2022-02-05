using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace CMPLNTRACK
{
    public partial class ComplaintForm : Form
    {
        List<ComplaintParty> parties;
        List<ComplaintType> types;
        List<ComplaintGroup> groups;
        List<Flag> flags;
        ComplaintDataAccess da;
        Complaint existingComplaint;
        public ComplaintForm(ComplaintDataAccess da, string startingAccountNumber)
            : this(da, (Complaint)null)
        {
            AccountNumberBox.Text = startingAccountNumber;

            try
            {
                var borrower = DataAccessHelper.ExecuteSingle<BorrowerName>("BorrowerSelectByAccountIdentifier",
                    DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? DataAccessHelper.Database.Udw : DataAccessHelper.Database.Cdw, SqlParams.Single("AccountIdentifier", startingAccountNumber));
                if (borrower != null)
                    BorrowerNameBox.Text = borrower.FirstName + " " + borrower.LastName;
            }
            catch
            {
                //borrower not found
            }
        }
        public ComplaintForm(ComplaintDataAccess da, Complaint existingComplaint)
        {
            InitializeComponent();

            this.da = da;
            this.existingComplaint = existingComplaint;

            parties = da.ComplaintPartiesGetAll();
            ComplaintPartyBox.DataSource = new ComplaintParty[] { new ComplaintParty() }.Union(parties).ToList();
            ComplaintPartyBox.DisplayMember = "PartyName";
            ComplaintPartyBox.ValueMember = "ComplaintPartyId";

            types = da.ComplaintTypesGetAll();
            ComplaintTypesBox.DataSource = new ComplaintType[] { new ComplaintType() }.Union(types).ToList();
            ComplaintTypesBox.DisplayMember = "TypeName";
            ComplaintTypesBox.ValueMember = "ComplaintTypeId";

            groups = da.GetComplaintGroupsGetAll();
            ComplaintGroupBox.DataSource = new ComplaintGroup[] { new ComplaintGroup() }.Union(groups).ToList();
            ComplaintGroupBox.DisplayMember = "GroupName";
            ComplaintGroupBox.ValueMember = "ComplaintGroupId";

            flags = da.FlagsGetAll();
            FlagsList.Items.AddRange(flags.Select(o => o.FlagName).ToArray());


            if (this.existingComplaint != null)
            {
                AccountNumberBox.ReadOnly = true;
                AccountNumberBox.Text = existingComplaint.AccountNumber;
                BorrowerNameBox.Text = existingComplaint.BorrowerName;
                ComplaintDescriptionBox.Text = existingComplaint.ComplaintDescription;
                SubmitUpdateButton.Enabled = true;
                ComplaintUpdateBox.Enabled = true;
                UpdateResolvesComplaintCheck.Enabled = this.existingComplaint.ResolutionComplaintHistoryId == null;
                ComplaintPartyBox.SelectedValue = existingComplaint.ComplaintPartyId;
                ComplaintTypesBox.SelectedValue = existingComplaint.ComplaintTypeId;
                ComplaintGroupBox.SelectedValue = existingComplaint.ComplaintGroupId;
                DaysToRespondBox.Value = existingComplaint.DaysToRespond;
                DateReceivedBox.Value = existingComplaint.ComplaintDate;
                ControlMailNumberBox.Text = existingComplaint.ControlMailNumber;
                NeedHelpNumberBox.Text = existingComplaint.NeedHelpTicketNumber;
                ReportedByBox.Text = GetFullName(existingComplaint.AddedBy);

                foreach (var flag in da.FlagsGetByComplaintId(existingComplaint.ComplaintId))
                    for (int i = 0; i < FlagsList.Items.Count; i++)
                    {
                        if ((string)FlagsList.Items[i] == flag.FlagName)
                        {
                            FlagsList.SetItemChecked(i, true);
                            break;
                        }
                    }
                GenerateHistories();
            }
            else
            {
                AutoResolveBox.Show();
                ReportedByBox.Text = GetFullName(Environment.UserName);
            }

            DateReceivedBox.MaxDate = DateTime.Now;

            CheckFlags();
        }

        private class BorrowerName
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private void GenerateHistories()
        {
            if (da != null || this.existingComplaint != null)
            {
                var histories = da.GetHistories(existingComplaint.ComplaintId);
                string history = "";
                foreach (var hist in histories.OrderBy(o => o.AddedOn))
                {
                    string entry = GetFullName(hist.AddedBy) + " - " + hist.AddedOn.ToString();
                    if (existingComplaint.ResolutionComplaintHistoryId == hist.ComplaintHistoryId)
                        entry += " - RESOLVED";
                    entry += Environment.NewLine + Environment.NewLine + hist.HistoryDetail + Environment.NewLine + Environment.NewLine;
                    history = entry + history;
                }
                HistoryBox.Text = history;
            }
        }

        private string GetFullName(string windowsUsername)
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                string displayName = "";
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, windowsUsername);
                displayName = up?.DisplayName;
                if (displayName.IsNullOrEmpty())
                    displayName = da.GetUserName(windowsUsername.Replace(@"UHEAA\", ""));
                return displayName;
            }
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                Complaint complaint = existingComplaint ?? new Complaint();
                complaint.AccountNumber = AccountNumberBox.Text;
                complaint.BorrowerName = BorrowerNameBox.Text;
                complaint.ComplaintDescription = ComplaintDescriptionBox.Text;
                complaint.ComplaintPartyId = (int)ComplaintPartyBox.SelectedValue;
                complaint.ComplaintTypeId = (int)ComplaintTypesBox.SelectedValue;
                complaint.ComplaintGroupId = (int)ComplaintGroupBox.SelectedValue;
                complaint.DaysToRespond = (int)DaysToRespondBox.Value;
                complaint.ComplaintDate = DateReceivedBox.Value;
                complaint.ControlMailNumber = ControlMailNumberBox.Text;
                complaint.NeedHelpTicketNumber = NeedHelpNumberBox.Text;

                if (existingComplaint == null)
                {
                    complaint.ComplaintId = da.ComplaintAdd(complaint);
                    existingComplaint = complaint;
                }
                else
                {
                    da.ComplaintSave(complaint);
                }

                da.ComplaintSetFlags(complaint.ComplaintId, FlagsList.CheckedItems.Cast<string>().Select(o => flags.Single(f => f.FlagName == o)).Select(o => o.FlagId).ToArray());
                if (AutoResolveBox.Checked)
                    da.AddHistory(existingComplaint.ComplaintId, "Ticket submitted as resolved.", true);
                this.Close();
            }
        }

        private bool Validation()
        {
            List<string> messages = new List<string>();
            if (AccountNumberBox.TextLength < 10)
                messages.Add("Please enter an account number");
            if (ComplaintPartyBox.SelectedIndex == 0)
                messages.Add("Please select a complaint party.");
            if (ComplaintTypesBox.SelectedIndex == 0)
                messages.Add("Please select a complaint type.");
            if (ComplaintGroupBox.SelectedIndex == 0)
                messages.Add("Please selecct a complaint group.");
            if (string.IsNullOrWhiteSpace(BorrowerNameBox.Text))
                messages.Add("Please enter a borrower name.");
            if (string.IsNullOrWhiteSpace(ComplaintDescriptionBox.Text))
                messages.Add("Please enter a description.");
            if (FlagsList.CheckedItems.Cast<string>().Select(o => flags.Single(f => f.FlagName == o)).Select(o => o.FlagName).SingleOrDefault() == "Control Mail" && ControlMailNumberBox.Text.IsNullOrEmpty())
                messages.Add("Please enter a control mail number when selecting Control Mail");
            if (FlagsList.CheckedItems.Cast<string>().Select(o => flags.Single(f => f.FlagName == o)).Select(o => o.FlagName).SingleOrDefault() == "Control Mail" && DaysToRespondBox.Value < 1)
                messages.Add("Please enter the number of days to respond when selecting Control Mail");
            if (messages.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, messages));
                return false;
            }
            return true;
        }

        private void CancelChangesButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SubmitUpdateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ComplaintUpdateBox.Text))
            {
                Dialog.Warning.Ok("Please enter an update first.");
                return;
            }
            int id = da.AddHistory(existingComplaint.ComplaintId, ComplaintUpdateBox.Text, UpdateResolvesComplaintCheck.Checked);
            if (UpdateResolvesComplaintCheck.Checked)
                existingComplaint.ResolutionComplaintHistoryId = id;
            ComplaintUpdateBox.Text = "";
            UpdateResolvesComplaintCheck.Checked = false;
            GenerateHistories();
        }

        private void FlagsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                CheckFlags();
            }));
        }

        private void CheckFlags()
        {
            bool enabled = FlagsList.CheckedItems.Cast<string>().Select(o => flags.Single(f => f.FlagName == o)).Any(o => o.EnablesControlMailFields);
            ControlMailNumberBox.Enabled = DaysToRespondBox.Enabled = enabled;
        }

        private void FlagsList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                for (int i = 0; i < FlagsList.Items.Count; i++)
                    if (i != e.Index)
                        FlagsList.SetItemChecked(i, false);
        }
    }
}