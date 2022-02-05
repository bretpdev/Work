using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace OALETTERS
{
    public partial class LetterSelection : Form
    {
        public string ScriptId { get; set; }
        public ProcessLogData LogData { get; set; }
        public DataAccess Da { get; set; }
        public BorrowerData Bor { get; set; }
        public BorrowerData CompanyData { get; set; }
        public bool IsCompany { get; set; }
        public List<LetterTypes> Letters { get; set; }
        public LetterTypes Letter { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public LetterSelection(string scriptId, ProcessLogData logData, string userName, string password)
        {
            InitializeComponent();
            ScriptId = scriptId;
            LogData = logData;
            Da = new DataAccess();

            List<string> states = new List<string>(Da.GetStateCodes());
            states.Insert(0, "");
            State.DataSource = states;

            Letters = Da.GetLetters();
            Letters.Insert(0, new LetterTypes() { LetterTypeId = 0, Letter = "", LetterType = "" });
            LetterId.DisplayMember = "Letter";
            LetterId.ValueMember = "LetterTypeId";
            LetterId.DataSource = Letters;
            UtId.Text = userName;
            Pwd.Text = password;
        }

        private void AccountIdentifier_TextChanged(object sender, EventArgs e)
        {
            if (AccountIdentifier.Text.Length >= 9 && AccountIdentifier.Text.Length <= 10)
            {
                Bor = new BorrowerData();
                Bor = Da.GetBorrowerData(AccountIdentifier.Text);
                ClearFields();
                if (Bor != null)
                    LoadFields(Bor);
            }
            else
                ClearFields();
        }

        private void Company_Click(object sender, EventArgs e)
        {
            if (((LetterTypes)(LetterId.SelectedItem)).LetterTypeId == 6 || IsCompany)
            {
                if (Company.Text == "Company")
                {
                    ClearFields();
                    UnlockFields();
                    Company.Text = "Borrower";
                    BorrowerName.Watermark = "Company Name";
                    LoadFields(CompanyData);
                }
                else
                {
                    LoadCompanyData();
                    ClearFields();
                    LockFields();
                    Company.Text = "Company";
                    BorrowerName.Watermark = "Borrower Name";
                    LoadFields(Bor);
                }
            }
            BorrowerName.Focus();
        }

        private void LetterId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((LetterTypes)(LetterId.SelectedItem)).IsCompany)
                IsCompany = true;
            else
            {
                if (IsCompany)
                    Company_Click(sender, e);
                IsCompany = false;
            }

            if (((LetterTypes)(LetterId.SelectedItem)).HasPaymentSource)
                PaymentSource.Enabled = true;
            else
                PaymentSource.Enabled = false;

            if (((LetterTypes)(LetterId.SelectedItem)).HasEffectiveDate)
                EffectiveDate.Enabled = true;
            else
                EffectiveDate.Enabled = false;

            if (((LetterTypes)(LetterId.SelectedItem)).HasAccountNumber)
                AccountIdentifier.Enabled = true;
            else
                AccountIdentifier.Enabled = false;
        }

        private void RefundAmount_Leave(object sender, EventArgs e)
        {
            if (RefundAmount.Text.IsPopulated())
                if (!RefundAmount.Text.Contains("."))
                    RefundAmount.Text = string.Format("{0:0.00}", RefundAmount.Text.ToDecimal());
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (Bor == null && ((LetterTypes)(LetterId.SelectedItem)).HasAccountNumber)
            {
                MessageBox.Show("Please provide a borrower account identifier", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (IsCompany && CompanyData == null && BorrowerName.Watermark == "Borrower Name")
            {
                MessageBox.Show("Please provide the Company information", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearFields();
                Company_Click(sender, e);
                return;
            }
            if (IsCompany)
                LoadCompanyData();
            else
                CompanyData = new BorrowerData();
            if (Verified())
            {
                if (Bor == null)
                    Bor = new BorrowerData();
                Bor.EffectiveDate = EffectiveDate.Text;
                Bor.RefundAmount = RefundAmount.Text;
                Bor.PaymentSource = PaymentSource.Text;
                Letter = (LetterTypes)LetterId.SelectedItem;
                string utId = GetUtId();
                if (utId.IsNullOrEmpty())
                    return;
                UserName = utId;
                Password = Pwd.Text;
                Verification v = new Verification(this);
                v.ShowDialog();
                if (v.DialogResult == DialogResult.OK)
                    this.DialogResult = DialogResult.OK;
                else
                    return;
            }
        }

        /// <summary>
        /// Loads the form fields with the current BorrowerData object
        /// </summary>
        /// <param name="data"></param>
        private void LoadFields(BorrowerData data)
        {
            if (data != null)
            {
                AccountIdentifier.Text = data.AccountNumber;
                BorrowerName.Text = string.Format("{0} {1}", data.FirstName, data.LastName);
                Address1.Text = data.Address1;
                Address2.Text = data.Address2;
                City.Text = data.City;
                State.Text = data.State;
                Zip.Text = data.Zip;
                Country.Text = data.Country;
            }
        }

        /// <summary>
        /// Loads the company data into a new BorrowerData objectS
        /// </summary>
        private void LoadCompanyData()
        {
            if (Company.Text == "Borrower")
            {
                CompanyData = new BorrowerData();
                CompanyData.AccountNumber = AccountIdentifier.Text;
                CompanyData.FirstName = BorrowerName.Text;
                CompanyData.Address1 = Address1.Text;
                CompanyData.Address2 = Address2.Text;
                CompanyData.City = City.Text;
                CompanyData.State = State.Text;
                CompanyData.Zip = Zip.Text;
                CompanyData.Country = Country.Text == "Country" ? "" : Country.Text;
            }
        }

        /// <summary>
        /// Resets all the fields
        /// </summary>
        private void ClearFields()
        {
            BorrowerName.Text = "";
            Address1.Text = "";
            Address2.Text = "";
            City.Text = "";
            State.SelectedIndex = 0;
            Zip.Text = "";
            Country.Text = "";
        }

        /// <summary>
        /// Locks all the fields so they can not be changed
        /// </summary>
        private void LockFields()
        {
            BorrowerName.Enabled = false;
            Address1.Enabled = false;
            Address2.Enabled = false;
            City.Enabled = false;
            State.Enabled = false;
            Zip.Enabled = false;
        }

        /// <summary>
        /// Unlocks all the fields to be edited
        /// </summary>
        private void UnlockFields()
        {
            BorrowerName.Enabled = true;
            Address1.Enabled = true;
            Address2.Enabled = true;
            City.Enabled = true;
            State.Enabled = true;
            Zip.Enabled = true;
        }

        /// <summary>
        /// Checks all the fields to see if any data is missing
        /// </summary>
        /// <returns></returns>
        private bool Verified()
        {
            bool isVerified = true;
            string message = "Please provide the missing fields\r\n\r\n";
            if (LetterId.Text == "")
            {
                message += "Letter Type\r\n";
                isVerified = false;
            }
            if (AccountIdentifier.Text == "" && Bor == null && ((LetterTypes)(LetterId.SelectedItem)).HasAccountNumber)
            {
                message += "Account Identifier\r\n";
                isVerified = false;
            }
            if (IsCompany)
            {
                if (BorrowerName.Text == "")
                {
                    message += "Company Name\r\n";
                    isVerified = false;
                }
                if (Address1.Text == "")
                {
                    message += "Address1\r\n";
                    isVerified = false;
                }
                if (City.Text == "")
                {
                    message += "City\r\n";
                    isVerified = false;
                }
                if (State.Text == "")
                {
                    message += "State\r\n";
                    isVerified = false;
                }
                if (Zip.Text == "")
                {
                    message += "Zip Code\r\n";
                    isVerified = false;
                }
            }
            if (RefundAmount.Text == "")
            {
                message += "Refund Amount\r\n";
                isVerified = false;
            }
            if (EffectiveDate.Text.Replace("/", "").Replace("_", "").Trim() == "" && ((LetterTypes)(LetterId.SelectedItem)).HasEffectiveDate)
            {
                message += "Effective Date\r\n";
                isVerified = false;
            }
            if (((LetterTypes)(LetterId.SelectedItem)).LetterTypeId == 4 && PaymentSource.Text == "")
            {
                message += "Payment Source\r\n";
                isVerified = false;
            }
            if (UtId.Text == "")
            {
                message += "UT ID\r\n";
                isVerified = false;
            }
            if (Pwd.Text == "")
            {
                message += "Password";
                isVerified = false;
            }
            if (!isVerified)
                MessageBox.Show(message, "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return isVerified;
        }

        /// <summary>
        /// Formats the UT ID in the correct format
        /// </summary>
        /// <returns></returns>
        private string GetUtId()
        {
            if (UtId.Text.Contains("UT") && UtId.Text.Length == 7)
                return UtId.Text;
            else if (!UtId.Text.Contains("UT") && UtId.Text.Length == 3)
                return string.Format("UT00{0}", UtId.Text);
            else if (!UtId.Text.Contains("UT") && UtId.Text.Length == 4)
                return string.Format("UT0{0},", UtId.Text);
            else if (!UtId.Text.Contains("UT") && UtId.Text.Length == 5)
                return string.Format("UT{0}", UtId.Text);

            MessageBox.Show("Invalid UT Id. Please correct and try again.", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UtId.Text = "UT";
            return "";
        }
    }
}