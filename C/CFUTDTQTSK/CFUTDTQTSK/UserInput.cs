using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CFUTDTQTSK
{
    public partial class UserInput : Form
    {
        public ArcData UserData { get; set; }
        public DataAccess DA { get; set; }
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public UserInput(ProcessLogRun logRun, ReflectionInterface ri)
        {
            InitializeComponent();
            ArcAddDate.MinDate = DateTime.Now.AddDays(1);
            DA = new DataAccess(logRun);
            RI = ri;
            LogRun = logRun;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                UserData = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    AccountNumber = AccountNumber.Text,
                    RecipientId = RecipientId.Text,
                    Arc = Arc.Text,
                    ProcessOn = ArcAddDate.Value,
                    Comment = Comment.Text,
                    IsEndorser = RecipientId.Text.Contains("P") ? true : false,
                    IsReference = false,
                    ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                    ScriptId = "CFUTDTQTSK"
                };

                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateData()
        {
            List<string> errors = new List<string>();

            if ((AccountNumber.Text.IsPopulated() && AccountNumber.Text.Length != 10) || AccountNumber.Text.IsNullOrEmpty())
                errors.Add("The Account Number is required and must be 10 characters");
            if (RecipientId.Text.IsPopulated() && RecipientId.Text.Length != 9)
                errors.Add("The Recipient Id must be 9 characters for references");
            if ((Arc.Text.IsPopulated() && Arc.Text.Length != 5) || Arc.Text.IsNullOrEmpty())
                errors.Add("The ARC is required and must be 5 Alpha Numeric charaters");

            if (AccountNumber.Text.Length == 10 && !ValidateAccountExists(errors))
                errors.Add("The Account Number entered does not exist");

            if (DA.CheckForDuplicate(AccountNumber.Text, RecipientId.Text, Arc.Text, Comment.Text, ArcAddDate.Text.ToDate()))
                if (!NotifyUser())
                    return false;

            if (errors.Any())
            {
                MessageBox.Show("Please review the following errors: \n\n" + string.Join("\n", errors.Select(p => " - " + p).ToArray()),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool NotifyUser()
        {
            if (!Dialog.Warning.YesNo($"There is already an ARC: {Arc.Text} for Account: {AccountNumber.Text} in the database to process on {ArcAddDate.Text}. Do you want to add a duplicate ARC?"))
            {
                AccountNumber.Text = "";
                RecipientId.Text = "";
                Arc.Text = "";
                Comment.Text = "";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the local warehouse first then the session to verify the borrower exists.
        /// </summary>
        private bool ValidateAccountExists(List<string> errors)
        {
            if (!DA.ValidateAccountNumber(AccountNumber.Text))
            {
                try
                {
                    SystemBorrowerDemographics demos = RI.GetDemographicsFromTx1j(AccountNumber.Text);
                    if (demos.AccountNumber.IsPopulated())
                    {
                        string message = $"The Borrower: {AccountNumber.Text} was not found in UDW but is in TX1J.  The ARC will still be added to ArcAddProcesing. A Refresh might be needed to add this borrower to UDW.";
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
                catch (DemographicException)
                {
                    return false;
                }
            }
            return true;
        }
    }
}