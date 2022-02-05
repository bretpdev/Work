using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;
using Uheaa.Common.DataAccess;

namespace KEYIDCHNG
{
    public partial class MainForm : Form
    {
        private readonly DataAccess DA;
        private readonly ProcessLogRun PLR;
        private readonly ReflectionInterface RI;
        public MainForm(ProcessLogRun plr, ReflectionInterface ri)
        {
            InitializeComponent();
            PLR = plr;
            BorrowerSearch.LDA = plr.LDA;
            DA = new DataAccess(plr.LDA);
            RI = ri;
        }

        private bool Approved = false;
        private QuickBorrower LoadedBorrower;
        private bool FoundOnCompass = false;
        private bool FoundOnOneLink = false;
        public KeyIdentifierChangeModel KeyIdentifierChangeModel
        {
            get
            {
                return new KeyIdentifierChangeModel()
                {
                    Ssn = LoadedBorrower.SSN,
                    UpdateCompass = FoundOnCompass,
                    UpdateOneLink = FoundOnOneLink,
                    FirstName = FirstNameBox.Text,
                    MiddleInitial = MiddleInitialBox.Text,
                    LastName = LastNameBox.Text,
                    DOB = DobBox.Text.ToDateNullable(),
                    Comments = CommentsBox.Text,
                    Approve = Approved,
                };
            }
        }

        private void BorrowerSearchResults_OnBorrowerChosen(object sender, QuickBorrower selected)
        {
            AccountIdentifierBox.Text = selected.SSN;
            LoadByAccountIdentifier();
        }

        private void LoadBorrowerButton_Click(object sender, EventArgs e)
        {
            LoadByAccountIdentifier();
        }

        private void LoadBorrower(QuickBorrower borrower)
        {
            LoadedBorrower = borrower;
            SelectedNameBox.Text = borrower.FullName;
            SelectedDobBox.Text = borrower.DOB;
            NewInfoGroup.Enabled = true;
            SelectedGroup.Enabled = true;
            FirstNameBox.Focus();
        }

        private void UnloadBorrower()
        {
            LoadedBorrower = null;
            SelectedNameBox.Clear();
            SelectedDobBox.Clear();
            SelectedGroup.Enabled = false;

            FirstNameBox.Clear();
            LastNameBox.Clear();
            DobBox.Clear();
            MiddleInitialBox.Clear();
            CommentsBox.Clear();
            NewInfoGroup.Enabled = false;
        }

        private void BorrowerSearch_OnSearchResultsRetrieved(SimpleBorrowerSearchControl sender, List<QuickBorrower> results)
        {
            results = results.Where(o => o.RegionEnum != RegionSelectionEnum.CornerStone).ToList();

            AccountIdentifierBox.Text = "";
            BorrowerSearchResults.SetResults(results);
        }

        private void AccountIdentifierBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadByAccountIdentifier();
        }

        private void LoadByAccountIdentifier()
        {
            var validity = AccountIdentifierBox.Validate();
            if (validity.Success)
            {
                var udwDemos = DA.GetDemographicData_Udw(AccountIdentifierBox.Text);
                var odwDemos = DA.GetDemographicData_Odw(AccountIdentifierBox.Text);
                var demos = udwDemos ?? odwDemos;
                if (demos == null)
                {
                    UnloadBorrower();
                    Dialog.Def.Ok("Unable to locate borrower with identifier " + AccountIdentifierBox.Text);
                    return;
                }
                FoundOnCompass = udwDemos != null;
                FoundOnOneLink = odwDemos != null;
                var quickBorrower = new QuickBorrower()
                {
                    FirstName = demos.FirstName,
                    MiddleInitial = demos.MI ?? demos.MiddleInitial,
                    LastName = demos.LastName,
                    DOB = demos.DOB?.ToShortDateString(),
                    SSN = demos.Ssn
                };
                LoadBorrower(quickBorrower);
            }
            else
                Dialog.Def.Ok("Please enter a valid Account Identifier.");
        }

        private void AccountIdentifierBox_TextChanged(object sender, EventArgs e)
        {
            BorrowerSearchResults.SetResults(null);
            BorrowerSearch.ResetFields();
        }

        private void ApproveButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Approved = true;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput(true))
            {
                Approved = false;
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInput(bool commentOnlyIsOk = false)
        {
            var enteredDob = DobBox.Text.ToDateNullable();
            List<string> messages = new List<string>();
            if (!commentOnlyIsOk)
                if ((FirstNameBox.TextLength + LastNameBox.TextLength + MiddleInitialBox.TextLength) == 0 && enteredDob == null)
                    messages.Add("Please add some identifying information to update.");
            if (CommentsBox.TextLength == 0)
                messages.Add("Please enter a Comment.");
            if (enteredDob.HasValue)
            {
                int minBorAge = 15;
                var lastAcceptableDate = DateTime.Now.AddYears(-minBorAge);
                if (enteredDob > lastAcceptableDate)
                    messages.Add("Please enter a DOB earlier than " + lastAcceptableDate.ToShortDateString());
            }
            if (!FirstNameBox.Text.All(o => char.IsLetter(o) || o == ' '))
                messages.Add("Please enter a First Name with only alphabet characters.");
            if (!LastNameBox.Text.All(o => char.IsLetter(o) || o == ' '))
                messages.Add("Please enter a Last Name with only alphabet characters.");
            if (!MiddleInitialBox.Text.All(o => char.IsLetter(o) || o == ' '))
                messages.Add("Please enter a Middle Initial that is an alphabet character.");
            if (messages.Any())
            {
                Dialog.Warning.Ok(string.Join(Environment.NewLine, messages));
                return false;
            }
            return true;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!DA.HasAccess())
            {
                Dialog.Warning.Ok("You do not have access to run this script.  If you need access, please request to be added to [GENR_REF_AuthAccess] under Key [Key ID Agent]");
                this.Close();
            }
        }
    }
}
