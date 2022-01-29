using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;

namespace TRDPRTYRES
{
    public partial class AcctSSNInput : Form
    {
        public bool IsOnelink { get; set; } = false;
        public SystemBorrowerDemographics Demographics { get; set; }
        public DataAccess DA { get; set; }
        public BorReferenceInfo BorData { get; set; }
        public List<References> Refs { get; set; }

        public AcctSSNInput(BorReferenceInfo bData, DataAccess da)
        {
            InitializeComponent();
            BorData = bData;
            borInfoBindingSource.DataSource = bData;
            DA = da;
        }

        private bool LoadDemos()
        {
            Demographics = DA.GetOnelinkDemos(AcctSsn.Text);
            if (Demographics != null && DA.CheckLoanStatus(Demographics.Ssn))
                IsOnelink = true;
            else
            {
                Demographics = DA.GetCompassDemos(AcctSsn.Text);
                if (Demographics != null && DA.HasOpenLoans(Demographics.Ssn))
                    Demographics.LastName = $"{Demographics.LastName}{(Demographics.Suffix.IsPopulated() ? $" {Demographics.Suffix}" : "")}";
            }
            if (Demographics == null)
            {
                Warning.Ok("The borrower was not found in the warehouse. Please verify the borrower exists and try again.");
                return false;
            }
            return true;
        }

        private void LoadReferences()
        {
            if (IsOnelink)
                Refs = DA.GetOnelinkReferences(Demographics.Ssn);
            else
                Refs = DA.GetCompassReferences(Demographics.Ssn);
            foreach (References item in Refs)
                ReferenceList.Items.Add(item.ReferenceId).SubItems.Add(item.ReferenceName);
        }

        private void AcctSsn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                Go_Click(sender, new EventArgs());
        }

        private void AcctSsn_TextChanged(object sender, EventArgs e)
        {
            AcceptButton = Go;
            NewReference.Enabled = false;
            if (AcctSsn.Text.Length < 9)
                ReferenceList.Items.Clear();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            NewReference.Enabled = false;
            ReferenceList.Items.Clear();
            if (AcctSsn.Text.Length >= 9)
            {
                if (LoadDemos())
                {
                    LoadReferences();
                    NewReference.Enabled = true;
                }
                else
                    ReferenceList.Items.Clear();
            }
        }

        private void ReferenceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReferenceList.SelectedItems.Count > 0)
            {
                BorData.Ssn = Demographics.Ssn;
                BorData.ReferenceId = ReferenceList.SelectedItems[0].Text;
                if (BorData.ReferenceId.IsPopulated() && LoadReferenceDemo(BorData.ReferenceId))
                {
                    borInfoBindingSource.DataSource = BorData;
                    OK.Enabled = true;
                    AcceptButton = OK;
                }
            }
        }

        private void ReferenceList_DoubleClick(object sender, EventArgs e)
        {
            if (ReferenceList.SelectedItems.Count > 0)
            {
                BorData.Ssn = Demographics.Ssn;
                BorData.ReferenceId = ReferenceList.SelectedItems[0].Text;
                if (BorData.ReferenceId.IsPopulated() && LoadReferenceDemo(BorData.ReferenceId))
                {
                    borInfoBindingSource.DataSource = BorData;
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private bool LoadReferenceDemo(string refId)
        {
            References r = Refs.Where(p => p.ReferenceId == refId).SingleOrDefault();
            if (r != null)
            {
                BorData.RefFirstName = (r.FirstName ?? "").Trim();
                BorData.RefMI = (r.MiddleInitial ?? "").Trim();
                BorData.RefLastName = (r.LastName ?? "").Trim();
                BorData.BorHasValidAddr = r.IsValidAddress;
                BorData.Address1 = (r.Address1 ?? "").Trim();
                BorData.Address2 = (r.Address2 ?? "").Trim();
                BorData.City = (r.City ?? "").Trim();
                BorData.State = (r.State ?? "").Trim();
                BorData.Zip = (r.ZipCode ?? "").Trim();
                BorData.HomePhone = (r.HomePhone ?? "").Trim();
                BorData.HomePhoneExt = (r.HomeExt ?? "").Trim();
                BorData.OtherPhone = (r.AltPhone ?? "").Trim();
                BorData.OtherPhoneExt = (r.AltExt ?? "").Trim();
                BorData.Foreign = (r.ForeignPhone ?? "").Trim();
                BorData.ForeignExt = (r.ForeignExt ?? "").Trim();
                BorData.Email = (r.EmailAddress ?? "").Trim();
                BorData.SourceCode = (r.SourceCode ?? "").Trim();
                BorData.RelationshipCode = (r.RelationshipCode ?? "").Trim();
                return true;
            }
            return false;
        }

        private void NewReference_Click(object sender, EventArgs e)
        {
            if (Demographics != null && Demographics.Ssn.IsPopulated())
            {
                BorData = new BorReferenceInfo();
                BorData.Ssn = Demographics.Ssn;
                BorData.ReferenceId = "";
                borInfoBindingSource.DataSource = BorData;
                DialogResult = DialogResult.OK;
            }
            else
                Warning.Ok("You must enter a valid SSN or Account Number");
        }
    }
}