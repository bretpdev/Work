using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using WHOAMI;

namespace MANMAIL
{
    public partial class ReturnMail : Form
    {
        private ReflectionInterface RI { get; set; }
        public DateTime LetterCreateDate { get; set; }
        public DateTime LetterReturnDate { get; set; }
        public string AccountIdentifier { get; set; }
        public string SelectedAccountIdentifier { get; set; }
        public string LetterReturnMailReason { get; set; }
        public string Letter { get; set; }
        public DataAccess DA { get; set; }

        public ReturnMail(ReflectionInterface ri, string barcodeInfo, DataAccess da)
        {
            InitializeComponent();
            DA = da;
            CreateDate.SelectionStart = 0;
            RI = ri;
            Reasons.DataSource = DA.GetReasons().OrderBy(p => p.ToString()).ToList();
            Reasons.SelectedIndex = -1;
            LoadLetterIds();
            if (!barcodeInfo.IsNullOrEmpty())
                LoadBarcodeData(barcodeInfo);
        }

        private void LoadBarcodeData(string barcode)
        {
            if (barcode.EndsWith("="))
                barcode = LegacyCryptography.Decrypt(barcode, LegacyCryptography.Keys.NoradOPS);
            AccountIdentifer.Text = barcode.Substring(0, 10).Trim();
            LetterCode.Text = barcode.Substring(10, 10).Trim();
            CreateDate.Text = barcode.Substring(20, 8).Trim();
        }

        private void LoadLetterIds()
        {
            List<string> ids = DA.GetLetterIds().OrderBy(p => p.ToString()).ToList();
            ids.Insert(0, "");
            LetterCode.DataSource = ids;
        }

        private void WhoAMI_Click(object sender, EventArgs e)
        {
            new BorrowerSearch(RI).Main();
            if (RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
            {
                AccountIdentifer.Text = RI.GetText(3, 23, 9);
            }
            if(RI.CheckForText(1, 71, "TXX1R"))
            {
                AccountIdentifer.Text = RI.GetText(3, 12, 11).Replace(" ", "");
            }
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            if (!RI.IsLoggedIn)
            {
                Dialog.Warning.Ok("You have been logged out of the session. Please log in and hit Enter again", "Logged Out");
                return;
            }
            DateTime? createDate = CreateDate.Text.ToDateNullable();
            DateTime? returnDate = ReturnDate.Text.ToDateNullable();
            string acct = AccountIdentifer.Text;
            acct = DecryptAcs(acct);
            AccountIdentifier = AccountIdentifer.Text;
            string acctId = GetReturnedAccount(acct);        
            acct = acctId;
            if (!Validate(createDate, returnDate, acct))
                return;

            LetterCreateDate = createDate.Value;
            LetterReturnDate = returnDate.Value;
            SelectedAccountIdentifier = acct;
            LetterReturnMailReason = Reasons.Text;
            Letter = LetterCode.Text;

            DialogResult = DialogResult.OK;
        }

        private string GetReturnedAccount(string accountIdentifer)
        {
            AssociatedBorAccounts accts = new AssociatedBorAccounts(DA.GetAssociatedAccounts(accountIdentifer));
            string account = "";
            while (account.IsNullOrEmpty())
            {
                if (accts.ShowDialog() == DialogResult.Cancel)
                    return "";
                account = accts.SelectedAccount;
            }
            accts.Close();
            accts.Dispose();
            return account;
        }

        private bool Validate(DateTime? createDate, DateTime? returnDate, string accountIdentifier)
        {
            List<string> errors = new List<string>();
            if (AccountIdentifer.Text.IsNullOrEmpty())
                errors.Add("The Account Identifier is required.");
            if (!createDate.HasValue)
            {
                errors.Add("The Create Date entered is not valid.");
                CreateDate.BackColor = Color.LightPink;
            }
            if(accountIdentifier.IsNullOrEmpty())
            {
                errors.Add("The form was cancelled or selected Account Identfier was empty.");
            }
            if (!returnDate.HasValue)
            {
                errors.Add("The Return Date entered is not valid.");
                ReturnDate.BackColor = Color.LightPink;
            }
            if (Reasons.SelectedIndex == -1)
            {
                errors.Add("You must choose a return mail reason.");
                Reasons.BackColor = Color.LightPink;
            }

            if (errors.Any())
            {
                ShowErrors(errors);
                return false;
            }

            if (createDate.Value > DateTime.Now)
                errors.Add("The Create Date cannot be a Future Date.");
            if (returnDate.Value > DateTime.Now)
                errors.Add("The Return Date cannot be a Future Date.");
            if (returnDate.Value < createDate.Value)
                errors.Add("The Return Date cannot be set before the Create Date.");

            if (errors.Any())
            {
                ShowErrors(errors);
                return false;
            }
            return true;
        }

        private void ShowErrors(List<string> errors)
        {
            Dialog.Error.Ok("Please review the following errors: \n\r\n\r" + string.Join(Environment.NewLine, errors));
        }

        private string DecryptAcs(string value)
        {
            if (value.StartsWith("RF@"))
                return value;
            value = value.Replace("M", "0");
            value = value.Replace("Y", "1");
            value = value.Replace("L", "2");
            value = value.Replace("A", "3");
            value = value.Replace("U", "4");
            value = value.Replace("G", "5");
            value = value.Replace("H", "6");
            value = value.Replace("T", "7");
            value = value.Replace("E", "8");
            value = value.Replace("R", "9");

            return value;
        }

        private void CreateDate_Enter(object sender, EventArgs e)
        {
            CreateDate.Focus();
            CreateDate.SelectionStart = 0;
        }

        private void ReturnDate_Enter(object sender, EventArgs e)
        {
            ReturnDate.Focus();
            ReturnDate.SelectionStart = 0;
        }

        private void CreateDate_TextChanged(object sender, EventArgs e)
        {
            if (CreateDate.Text.Replace("/", "").Replace(" ", "").Length == 4)
            {
                if (CreateDate.Text.Substring(0, 2).ToInt() > DateTime.Now.Month)
                    CreateDate.Text = CreateDate.Text + (DateTime.Now.Year - 1);
                else
                    CreateDate.Text = CreateDate.Text + DateTime.Now.Year;
                ReturnDate.Focus();
            }
        }

        private void ReturnDate_TextChanged(object sender, EventArgs e)
        {
            if (ReturnDate.Text.Replace("/", "").Replace(" ", "").Length == 4)
            {
                if (ReturnDate.Text.Substring(0, 2).ToInt() > DateTime.Now.Month)
                    ReturnDate.Text = ReturnDate.Text + (DateTime.Now.Year - 1);
                else
                    ReturnDate.Text = ReturnDate.Text + DateTime.Now.Year;
                Reasons.Focus();
            }
        }
    }
}