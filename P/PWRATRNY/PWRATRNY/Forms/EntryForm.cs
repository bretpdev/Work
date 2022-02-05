using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using static Uheaa.Common.Dialog;

namespace PWRATRNY
{
    public partial class EntryForm : Form
    {
        private UserPOAEntry Entry { get; set; }

        public EntryForm(UserPOAEntry entry)
        {
            InitializeComponent();
            Entry = entry;
            Text = $"{Text} :: Version:{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (CheckAccountIdentifier() && CheckFirst() && CheckLast() && CheckDate())
            {
                BindData();
                DialogResult = DialogResult.OK;
            }
        }

        public bool CheckAccountIdentifier()
        {
            if (AccountIdentfierTextBox.Validate().Success && AccountIdentfierTextBox.Text.Length > 8)
                return true;
            else
            {
                Warning.Ok("Please enter an Account Number or a Social Security Number in the proper format.", "Invalid Data Provided");
                return false;
            }
        }

        public bool CheckFirst()
        {
            if (ReferenceFirstTextBox.Validate().Success && ReferenceFirstTextBox.Text.Length > 0)
                return true;
            else
            {
                Warning.Ok("Please enter a valid reference first name.", "Invalid Data Provided");
                return false;
            }
        }

        public bool CheckLast()
        {
            if (ReferenceLastTextBox.Validate().Success && ReferenceLastTextBox.Text.Length > 0)
                return true;
            else
            {
                Warning.Ok("Please enter a valid reference last name.", "Invalid Data Provided");
                return false;
            }
        }

        public bool CheckDate()
        {
            if (!POAExpirationTextBox.Text.ToDateNullable().HasValue || POAExpirationTextBox.Text.ToDateNullable() < DateTime.Now)
            {
                Warning.Ok("Please enter a valid date. Dates can not be in the past.", "Invalid Data Provided");
                return false;
            }
            return true;
        }

        private void BindData()
        {
            Entry.UserEnteredAccountNumberOrSSN = AccountIdentfierTextBox.Text.TrimStart(new char[] { ' ' }).TrimEnd(new char[] { ' ' });
            Entry.FirstName = ReferenceFirstTextBox.Text.Trim();
            Entry.LastName = ReferenceLastTextBox.Text.Trim();
            Entry.ExpirationDate = POAExpirationTextBox.Text.ToDateNullable().Value.ToShortDateString() ?? null;
        }
    }
}