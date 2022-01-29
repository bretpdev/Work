using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;
using DUPLREFS.Forms;

namespace DUPLREFS
{
    public partial class UserInput : Form
    {
        DataAccess DA { get; set; }
        Borrower Bwr { get; set; }
        private List<Reference> UserUpdatedReferences { get; set; } = new List<Reference>();
        private List<ReferenceControl> ReferenceControls;
        private bool FormSubmitted { get; set; } 

        public UserInput(DataAccess da, Borrower bwr)
        {
            DA = da;
            Bwr = bwr;
            InitializeComponent();
            ReferenceControls = new List<ReferenceControl>() { refControl1, refControl2, refControl3, refControl4 };
        }

        public void PopulateFields(ref int index)
        {
            // Populate fields for reference 1
            if (HasAnotherPossibleDuplicateReference(Bwr.PossibleDuplicateReferences, index))
            {
                TrackReferenceRecord(Bwr, index);
                ReferenceControls[0].PopulateFields(Bwr.PossibleDuplicateReferences[index], DA);
                index++;
            }

            // Populate fields for reference 2
            if (HasAnotherPossibleDuplicateReference(Bwr.PossibleDuplicateReferences, index) && MatchesPreviousReference(Bwr.PossibleDuplicateReferences, index))
            {
                TrackReferenceRecord(Bwr, index);
                ReferenceControls[1].PopulateFields(Bwr.PossibleDuplicateReferences[index], DA);
                index++;
            }

            // Populate fields for reference 3
            if (HasAnotherPossibleDuplicateReference(Bwr.PossibleDuplicateReferences, index) && MatchesPreviousReference(Bwr.PossibleDuplicateReferences, index))
            {
                TrackReferenceRecord(Bwr, index);
                ReferenceControls[2].PopulateFields(Bwr.PossibleDuplicateReferences[index], DA);
                index++;
            }

            // Populate fields for reference 4
            if (HasAnotherPossibleDuplicateReference(Bwr.PossibleDuplicateReferences, index) && MatchesPreviousReference(Bwr.PossibleDuplicateReferences, index))
            {
                TrackReferenceRecord(Bwr, index);
                ReferenceControls[3].PopulateFields(Bwr.PossibleDuplicateReferences[index], DA);
                index++;
            }
        }

        private void TrackReferenceRecord(Borrower bor, int index)
        {
            Reference rfr = new Reference();
            rfr.ReferenceQueueId = bor.PossibleDuplicateReferences[index].ReferenceQueueId;
            UserUpdatedReferences.Add(rfr); // Track which reference is being worked so we can update the ReferenceProcessing table later
        }

        private bool HasAnotherPossibleDuplicateReference(List<Reference> possibleDuplicateReference, int index)
        {
            return index < possibleDuplicateReference.Count() && !string.IsNullOrEmpty(possibleDuplicateReference[index].RefName);
        }

        private bool MatchesPreviousReference(List<Reference> possibleDuplicateReferences, int index)
        {
            return index > 0 && possibleDuplicateReferences[index].RefName == possibleDuplicateReferences[index - 1].RefName;
        }

        public List<Reference> GetUserUpdates()
        {
            return UserUpdatedReferences;
        }

        private List<Reference> RecordUserUpdates()
        {
            for (int i = 0; i < UserUpdatedReferences.Count(); i++)
            {
                UserUpdatedReferences[i].RefId = ReferenceControls[i].GetID();
                UserUpdatedReferences[i].RefName = ReferenceControls[i].GetName();
                UserUpdatedReferences[i].RefAddress1 = ReferenceControls[i].GetAddress1();
                UserUpdatedReferences[i].RefAddress2 = ReferenceControls[i].GetAddress2();
                UserUpdatedReferences[i].RefCity = ReferenceControls[i].GetCity();
                UserUpdatedReferences[i].RefState = ReferenceControls[i].GetState();
                UserUpdatedReferences[i].RefZip = ReferenceControls[i].GetZip();
                UserUpdatedReferences[i].RefPhone = ReferenceControls[i].GetPhone();
                UserUpdatedReferences[i].RefStatus = ReferenceControls[i].GetStatus();
                UserUpdatedReferences[i].ValidAddress = ReferenceControls[i].GetAddressValid();
                UserUpdatedReferences[i].ValidPhone = ReferenceControls[i].GetPhoneValid();
                UserUpdatedReferences[i].BorrowerQueueId = Bwr.BorrowerQueueId;
                UserUpdatedReferences[i].RefCountry = ReferenceControls[i].GetCountry();

                // Check to see if user updated any of the demos for ref
                UserUpdatedReferences[i].DemosChanged = UserMadeUpdate(UserUpdatedReferences[i], Bwr.References.Where(x => x.ReferenceQueueId == UserUpdatedReferences[i].ReferenceQueueId).FirstOrDefault());
                UserUpdatedReferences[i].ZipChanged = ZipCodeChanged(UserUpdatedReferences[i], Bwr.References.Where(x => x.ReferenceQueueId == UserUpdatedReferences[i].ReferenceQueueId).FirstOrDefault());
            }
            return UserUpdatedReferences;
        }

        private bool UserMadeUpdate(Reference userProvided, Reference original)
        {
            bool hasIncompleteAddressHistory = (userProvided == null || original == null);
            bool demosChanged = false;
            if (!hasIncompleteAddressHistory)
            {
                demosChanged = (userProvided.HasDifferentDemos(original)
                || userProvided.ValidAddress != original.ValidAddress // Validity change recorded as a demos change
                || userProvided.ValidPhone != original.ValidPhone
                || userProvided.RefStatus != original.RefStatus); // Ref active status change recorded as a demos change
            }

            return !hasIncompleteAddressHistory && demosChanged;
        }

        private bool ZipCodeChanged(Reference userProvided, Reference original)
        {
            bool hasIncompleteAddressHistory = (userProvided == null || original == null);
            bool zipChanged = false;
            if (!hasIncompleteAddressHistory)
                zipChanged = (userProvided.RefZip != original.RefZip);
            return zipChanged;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            RecordUserUpdates();
            if (IsValidData())
            {
                this.DialogResult = DialogResult.OK;
                FormSubmitted = true;
                this.Close();
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!FormSubmitted && !DoesUserWishToClose())
                e.Cancel = true;
            base.OnFormClosing(e);
        }

        public static bool DoesUserWishToClose()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to end the script?", "Cancel Script Run?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private bool IsValidData()
        {
            string validationMessage = "";
            foreach (Reference rfr in UserUpdatedReferences)
            {
                validationMessage += (rfr.RefAddress1.Length <= 35 && !rfr.RefAddress1.Contains(",")) ? "" : $"The Address1 field for Ref ID {rfr.RefId} must be 35 characters or less and contain no commas. ";
                validationMessage += (rfr.RefAddress2.Length <= 35 && !rfr.RefAddress2.Contains(",")) ? "" : $"The Address2 field for Ref ID {rfr.RefId} must be 35 characters or less and contain no commas. ";
                validationMessage += (rfr.RefCity.Length <= 30 && !rfr.RefCity.Contains(",")) ? "" : $"The City field for Ref ID {rfr.RefId} must be 30 characters or less and contain no commas. ";
                validationMessage += (rfr.RefState != "") ? "" : $"The State field for Ref ID {rfr.RefId} must be set. ";
                validationMessage += ((rfr.ZipChanged.HasValue && !rfr.ZipChanged.Value) || (rfr.RefZip.Length == 9 && !rfr.RefZip.Contains(","))) ? "" : $"The Zip field for Ref ID {rfr.RefId} must be 9 characters. "; // This is only flagged in the session if the zip is actually changed
                validationMessage += (rfr.RefPhone.Length == 10 && rfr.RefPhone.IsNumeric()) ? "" : $"The Phone field for Ref ID {rfr.RefId} must be 10 digits. "; // TODO: Do we need to record the EXT field? Ask BA.

                if (!string.IsNullOrWhiteSpace(rfr.RefCountry))
                    validationMessage += (!rfr.RefPhone.StartsWith("0") && rfr.RefPhone.StartsWith("1")) ? "" : $"The Phone field for {rfr.RefId} cannot start with 0 or 1 for a domestic number. ";
                else
                    validationMessage += (rfr.RefCountry.Length <= 25 && !rfr.RefCountry.Contains(",")) ? "" : $"The Country field for {rfr.RefId} must be 25 characters or less and contain no commas. ";

                validationMessage += (rfr.RefStatus == "A" || rfr.RefStatus == "I") ? "" : $"Incorrect Status value detected for Ref ID {rfr.RefId}. ";

            }

            if (validationMessage.Length == 0)
            {
                lbl_UserPrompt.Text = "";
                lbl_UserPrompt.BackColor = this.BackColor;
                return true;
            }
            else
            {
                SetValidationMessagePrompt(validationMessage);
                return false;
            }
        }

        private void SetValidationMessagePrompt(string validationMessage)
        {
            lbl_UserPrompt.Text = validationMessage;
            lbl_UserPrompt.BackColor = Color.Red;
        }
    }
}
