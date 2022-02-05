using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.WinForms
{
    public partial class ArcAddProcessingUI : Form
    {
        private List<ArcAddProcessingTypes> ArcTypes { get; set; }
        private DataAccessHelper.Database ArcDB { get; set; }
        private DataAccessHelper.Database WarehouseDB { get; set; }
        private List<int> LoanSeqs { get; set; }
        private List<string> LoanPgms { get; set; }
        private string[] RegardsCodes { get; set; }
        private bool IsValid { get; set; }
        private List<string> ValidArcs { get; set; }

        public ArcAddProcessingUI()
        {
            InitializeComponent();
            ArcDB = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? DataAccessHelper.Database.Uls : DataAccessHelper.Database.Cls;
            WarehouseDB = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa ? DataAccessHelper.Database.Udw : DataAccessHelper.Database.Cdw;
            ArcTypes = DataAccessHelper.ExecuteList<ArcAddProcessingTypes>("ArcAdd_GetArcTypes", ArcDB);
            ArcTypeSelection.DataSource = ArcTypes.Select(p => p.ArcType).ToList();
            ArcTypeSelection.SelectedIndex = -1;
            LoanSeqs = new List<int>();
            LoanPgms = new List<string>();
            RegardsCodes = new string[] { "B", "E" };
            RegardsCode.Items.AddRange(RegardsCodes);
        }

        public ArcAddProcessingUI(string accountNumber, string scriptId,List<string> validArcs = null, string recipientId = null, string arc = null, DateTime? processOn = null, string comment = null,
            bool isReference = false, bool isEndorser = false, DateTime? processFrom = null, DateTime? processTo = null, DateTime? neededBy = null, string regardsTo = null, string regardsCode = null)
            : this()
        {
            AccountIdentifer.Text = accountNumber;
            ScriptId.Text = scriptId;
            IsReference.Checked = isReference;
            IsEndorser.Checked = isEndorser;
            ValidArcs = validArcs;
            if (recipientId.IsPopulated())
                RecipientId.Text = recipientId;
            if (arc.IsPopulated())
                ARC.Text = arc;
            if (processOn.HasValue)
                ProcessOn.Value = processOn.Value;
            if (comment.IsPopulated())
                Comment.Text = comment;
            if (processFrom.HasValue)
                ProcessFrom.Text = processFrom.Value.ToShortDateString();
            if (processTo.HasValue)
                ProcessTo.Text = processTo.Value.ToShortDateString();
            if (neededBy.HasValue)
                NeededBy.Text = neededBy.Value.ToShortDateString();
            if (regardsTo.IsPopulated())
                RegardsTo.Text = regardsTo;
            if (regardsCode.IsPopulated())
                RegardsCode.SelectedIndex = Array.IndexOf(RegardsCodes, regardsCode);
        }



        private void validationButton1_Click(object sender, EventArgs e)
        {
            if (!IsValid)
                return;

            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = (ArcData.ArcType)ArcTypes.Where(p => p.ArcType == this.ArcTypeSelection.Text).SingleOrDefault().ArcTypeId,
                AccountNumber = AccountIdentifer.Text,
                RecipientId = RecipientId.Text,
                Arc = ARC.Text,
                ProcessOn = ProcessOn.Value,
                Comment = Comment.Text,
                ScriptId = ScriptId.Text,
                IsReference = IsReference.Checked,
                IsEndorser = IsEndorser.Checked,
                ProcessTo = ProcessTo.Text.ToDateNullable(),
                ProcessFrom = ProcessFrom.Text.ToDateNullable(),
                NeedBy = NeededBy.Text.ToDateNullable(),
                RegardsTo = RegardsTo.Text,
                RegardsCode = RegardsCode.Text,
                LoanSequences = LoanSeqs.Count == 0 ? null : LoanSeqs,
                LoanPrograms = LoanPgms.Count == 0 ? null : LoanPgms
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
                MessageBox.Show(string.Join("\r\n", result.Errors));

            DialogResult = DialogResult.OK;
        }

        private void AccountIdentifer_Leave(object sender, EventArgs e)
        {
            if (!ValidateAccountIdentifer(AccountIdentifer.Text, "Account Number/SSN"))
                AccountIdentifer.Focus();
        }

        private bool ValidateAccountIdentifer(string account, string field)
        {
            try
            {
                if (account.Length == 9)
                    DataAccessHelper.ExecuteSingle<string>("spGetAccountNumberFromSsn", WarehouseDB, SqlParams.Single("Ssn", account));
                else if (account.Length == 10)
                    DataAccessHelper.ExecuteSingle<string>("spGetSSNFromAcctNumber", WarehouseDB, SqlParams.Single("AccountNumber", account));
                else
                {
                    MessageBox.Show(string.Format("The {0} must be 9 or 10 digits.  Please try again.", field));
                    return false;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(string.Format("The Account Number or SSN {1} does not exist in {0}.  Please contact Systems Support if you believe this is an error.", WarehouseDB, account));
                return false;
            }

            return true;
        }

        private void RecipientId_Leave(object sender, EventArgs e)
        {
            if (!RecipientId.Text.IsNullOrEmpty())
                if (!ValidateAccountIdentifer(RecipientId.Text, "Recipient Id"))
                    RecipientId.Focus();
        }

        private void RegardsTo_Leave(object sender, EventArgs e)
        {
            if (!RegardsTo.Text.IsNullOrEmpty())
                if (!ValidateAccountIdentifer(RegardsTo.Text, "Regards To"))
                    RegardsTo.Focus();
        }

        private void OK_OnValidate(object sender, ValidationEventArgs e)
        {
            IsValid = e.FormIsValid;
        }

        private void ArcTypeSelection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ArcTypeSelection.SelectedItem.ToString().Contains("LoanProgram"))
            {
                using (LoanSelection sel = new LoanSelection(false, AccountIdentifer.Text))
                {
                    if (sel.ShowDialog() == DialogResult.Cancel)
                        ArcTypeSelection.SelectedIndex = -1;

                    LoanPgms = sel.SelectedLoans;
                }
            }
            else if (ArcTypeSelection.SelectedItem.ToString().Contains("ByLoan"))
            {
                using (LoanSelection sel = new LoanSelection(true, AccountIdentifer.Text))
                {
                    if (sel.ShowDialog() == DialogResult.Cancel)
                        ArcTypeSelection.SelectedIndex = -1;

                    LoanSeqs = sel.SelectedLoans.Select(int.Parse).ToList();
                }
            }

        }

        private void ArcTypeSelection_Click(object sender, EventArgs e)
        {
            if (AccountIdentifer.Text.IsNullOrEmpty())
            {
                MessageBox.Show("You must enter an account number before you select an Arc Type");
                AccountIdentifer.Focus();
                return;
            }
        }

        private void ARC_Leave(object sender, EventArgs e)
        {
            if(ValidArcs != null && ValidArcs.Any() && !ValidArcs.ContainsToUpper(ARC.Text.ToUpper()))
            {
                string message = string.Format("Invalid ARC entered. Please enter one of the following ARCS: {1}{1} {0}", string.Join(Environment.NewLine, ValidArcs), Environment.NewLine);
                Dialog.Error.Ok(message);
                ARC.Focus();
            }
        }
    }
}
