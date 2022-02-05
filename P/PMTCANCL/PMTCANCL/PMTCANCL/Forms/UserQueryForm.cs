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

namespace PMTCANCL
{
    public partial class UserQueryForm : Form
    {
        private UserQuery userQuery { get; set; }
        private UserValidator validator { get; set; }
        public UserQueryForm(UserQuery userQuery, UserValidator validator)
        {
            InitializeComponent();
            this.userQuery = userQuery;
            this.validator = validator;
            InitFromQuery();
        }

        private void InitFromQuery()
        {
            UheaaRadioButton.Checked = userQuery.region == DataAccessHelper.Region.Uheaa ? true : false;
            CornerstoneRadioButton.Checked = userQuery.region == DataAccessHelper.Region.CornerStone ? true : false;
            if(userQuery.processed.HasValue)
            {
                ProcessedCheckBox.Checked = userQuery.processed.Value;
                UnprocessedCheckBox.Checked = !userQuery.processed.Value;
            }
            else
            {
                ProcessedCheckBox.Checked = true;
                UnprocessedCheckBox.Checked = true;
            }
            if (userQuery.madeAfter != null)
            {
                if(MadeAfterDateTimePicker.MinDate < userQuery.madeAfter && MadeAfterDateTimePicker.MaxDate > userQuery.madeAfter)
                {
                    DateEnabled.Checked = true;
                    MadeAfterDateTimePicker.Value = userQuery.madeAfter;
                }
            }
            if(userQuery.borrower != null)
            {
                BorrowerTextBox.Text = userQuery.borrower;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                BindData();
                DialogResult = DialogResult.OK;
            }
        }

        private void RegionCheckChanged(object sender, EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (UheaaRadioButton.Checked == CornerstoneRadioButton.Checked)
                UheaaRadioButton.Checked = !CornerstoneRadioButton.Checked;
            else
                CornerstoneRadioButton.Checked = !UheaaRadioButton.Checked;
        }

        private void ProcessedCheckChanged(object sender, EventArgs e)
        {
            //Use an "if" statement to avoid an infinite cause/effect loop.
            if (!ProcessedCheckBox.Checked && !UnprocessedCheckBox.Checked)
            {
                ProcessedCheckBox.Checked = true;
                UnprocessedCheckBox.Checked = true;
            }
        }

        private bool CheckRegion()
        {
            if(!UheaaRadioButton.Checked && !CornerstoneRadioButton.Checked)
            {
                MessageBox.Show("Please select a region.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(UheaaRadioButton.Checked && !validator.UserHasUheaaAccess())
            {
                MessageBox.Show("User role does not have access to the Uheaa region.", "Invalid Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (CornerstoneRadioButton.Checked && !validator.UserHasFedAccess())
            {
                MessageBox.Show("User role does not have access to the Cornerstone region.", "Invalid Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool CheckBorrower()
        {
            if(BorrowerTextBox.Text.Length > 0 && BorrowerTextBox.Text.Length != 9 && BorrowerTextBox.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid 9 or 10 digit account identifier.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidateSelection()
        {
            if(!CheckRegion() || !CheckBorrower())
            {
                return false;
            }
            return true;
        }

        private void BindData()
        {
            userQuery.region = UheaaRadioButton.Checked ? DataAccessHelper.Region.Uheaa : DataAccessHelper.Region.CornerStone;
            if(ProcessedCheckBox.Checked && !UnprocessedCheckBox.Checked)
            {
                userQuery.processed = true;
            }
            else if (UnprocessedCheckBox.Checked && !ProcessedCheckBox.Checked)
            {
                userQuery.processed = false;
            }
            else
            {
                userQuery.processed = null;
            }
            if(ValidMadeAfter())
            {
                userQuery.madeAfter = MadeAfterDateTimePicker.Value;
            }
            else
            {
                userQuery.madeAfter = MadeAfterDateTimePicker.MinDate;
            }
            userQuery.borrower = ValidBorrower() ? BorrowerTextBox.Text : null;
            
        }

        private bool ValidMadeAfter()
        {
            if(DateEnabled.Checked)
            {
                return true;
            }
            return false;
        }

        private bool ValidBorrower()
        {
            if(BorrowerTextBox.Text.Length == 9 || BorrowerTextBox.Text.Length == 10)
            {
                return true;
            }
            return false;
        }

        private void DateEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if(MadeAfterDateTimePicker.Enabled)
            {
                MadeAfterDateTimePicker.Enabled = false;
            }
            else
            {
                MadeAfterDateTimePicker.Enabled = true;
            }
        }
    }
}
