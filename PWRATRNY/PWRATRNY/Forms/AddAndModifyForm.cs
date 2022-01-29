using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;
using static Uheaa.Common.Dialog;

namespace PWRATRNY
{
    public partial class AddAndModifyForm : Form
    {
        private DataAccess DA { get; set; }
        private static List<Relationship> Relationships { get; set; }
        private static List<StateCode> StateCodes { get; set; }
        private PowerOfAttorneyData POAData { get; set; }

        public bool IsUpdated { get; set; } = false;
        public bool IsUpdatedAddress { get; set; } = false;
        public bool IsUpdatedHomePhone { get; set; } = false;
        public bool IsUpdatedOtherPhone { get; set; } = false;
        public bool IsUpdatedForeignPhone { get; set; } = false;
        public bool IsUpdatedEmail { get; set; } = false;
        public bool IsUpdatedName { get; set; } = false;
        public bool IsUpdatedRelationship { get; set; } = false;

        public AddAndModifyForm(PowerOfAttorneyData pOAData, DataAccess da, List<Relationship> relationships)
        {
            InitializeComponent();
            Text = $"{Text} :: Version:{Assembly.GetExecutingAssembly().GetName().Version}";

            DA = da;
            LoadDropDowns(relationships);

            POAData = pOAData;
            InitFields();
            RegisterDirty(Controls);
        }

        private void LoadDropDowns(List<Relationship> relationships)
        {
            Relationships = relationships;
            Relationships.Insert(0, new Relationship());
            RelationshipCbo.DataSource = Relationships;
            RelationshipCbo.DisplayMember = "Description";

            StateCodes = DA.GetStateCodes();
            StateCodes.Insert(0, new StateCode());
            StateCbo.DataSource = StateCodes;
            StateCbo.DisplayMember = "Code";
        }

        private void RegisterDirty(Control.ControlCollection ctrls)
        {
            foreach (Control c in ctrls)
            {
                if (c is TextBox)
                    (c as TextBox).TextChanged += new EventHandler(SetUpdated);
                if (c is AlphaTextBox)
                    (c as AlphaTextBox).TextChanged += new EventHandler(SetUpdated);
                if (c is AlphaNumericTextBox)
                    (c as AlphaNumericTextBox).TextChanged += new EventHandler(SetUpdated);
                if (c is NumericTextBox)
                    (c as NumericTextBox).TextChanged += new EventHandler(SetUpdated);
                if (c is OmniTextBox)
                    (c as OmniTextBox).TextChanged += new EventHandler(SetUpdated);
                if (c is SsnTextBox)
                    (c as SsnTextBox).TextChanged += new EventHandler(SetUpdated);
                if (c is ComboBox)
                {
                    (c as ComboBox).TextChanged += new EventHandler(SetUpdated);
                    (c as ComboBox).SelectedIndexChanged += new EventHandler(SetUpdated);
                }
                if (c.HasChildren)
                    RegisterDirty(c.Controls);
            }
        }

        private void SetUpdated(object sender, EventArgs e) =>
            IsUpdated = true;

        private void SetDirtyAddress(object sender, EventArgs e) =>
            IsUpdatedAddress = true;

        private void SetDirtyHomePhone(object sender, EventArgs e) =>
            IsUpdatedHomePhone = true;

        private void SetDirtyOtherPhone(object sender, EventArgs e) =>
            IsUpdatedOtherPhone = true;

        private void SetDirtyForeignPhone(object sender, EventArgs e) =>
            IsUpdatedForeignPhone = true;

        private void SetDirtyEmail(object sender, EventArgs e) =>
            IsUpdatedEmail = true;

        private void SetDirtyName(object sender, EventArgs e) =>
            IsUpdatedName = true;

        private void SetDirtyRelationship(object sender, EventArgs e) =>
            IsUpdatedRelationship = true;

        private void InitFields()
        {
            BorrowerSSNTextBox.Text = POAData.BorrowerDemos.Ssn ?? "";
            ReferenceFirstTextBox.Text = POAData.RefData.FirstName ?? "";
            ReferenceLastTextBox.Text = POAData.RefData.LastName ?? "";
            MiddleInitialTextBox.Text = POAData.RefData.MiddleIntial ?? "";
            if (POAData.RefData.RelationshipToBorrower is object)
                RelationshipCbo.SelectedIndex = RelationshipCbo.FindString(POAData.RefData.RelationshipToBorrower.Description);
            Address1TextBox.Text = POAData.RefData.Address1 ?? "";
            Address2TextBox.Text = POAData.RefData.Address2 ?? "";
            CityTextBox.Text = POAData.RefData.City ?? "";
            if (POAData.RefData.State != null)
                StateCbo.SelectedIndex = StateCbo.FindString(POAData.RefData.State);
            ZipTextBox.Text = POAData.RefData.ZipCode ?? "";
            HomePhoneTextBox.Text = POAData.RefData.PrimaryPhone ?? "";
            OtherPhoneTextBox.Text = POAData.RefData.AlternatePhone ?? "";
            ForeignPhoneTextBox.Text = POAData.RefData.ForeignPhone ?? "";
            EmailTextBox.Text = POAData.RefData.EmailAddress ?? "";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (IsValidUserEntries())
            {
                if (IsUpdated)
                    BindData();
                DialogResult = DialogResult.OK;
            }
        }

        public bool IsValidUserEntries()
        {
            if (!CheckSSN())
                return false;

            if (!CheckFirst())
                return false;

            if (!CheckLast())
                return false;

            if (!CheckMiddle())
                return false;

            if (!CheckRelationship())
                return false;

            if (!CheckAddress(Address1TextBox, "Address 1"))
                return false;

            if (!CheckAddress(Address2TextBox, "Address 2"))
                return false;

            if (!CheckCity())
                return false;

            if (!CheckState())
                return false;

            if (!CheckZip())
                return false;

            if (!CheckPhone(HomePhoneTextBox, "Home Phone"))
                return false;

            if (!CheckPhone(OtherPhoneTextBox, "Other Phone"))
                return false;

            if (!CheckPhone(ForeignPhoneTextBox, "Foreign Phone"))
                return false;

            if (!CheckEmail())
                return false;

            return true;
        }

        public bool CheckSSN()
        {
            if (BorrowerSSNTextBox.Validate().Success)
                return true;
            else
                Warning.Ok("Please enter a valid SSN.", "Invalid Data Provided");
            return false;
        }

        public bool CheckFirst()
        {
            if (ReferenceFirstTextBox.Validate().Success && ReferenceFirstTextBox.Text.Length > 0)
                return true;
            else
                Warning.Ok("Please enter a first name.", "Invalid Data Provided");
            return false;
        }

        public bool CheckLast()
        {
            if (ReferenceLastTextBox.Validate().Success && ReferenceLastTextBox.Text.Length > 0)
                return true;
            else
                Warning.Ok("Please enter a last name.", "Invalid Data Provided");
            return false;
        }

        public bool CheckMiddle()
        {
            if (MiddleInitialTextBox.Text.Length > 0)
            {
                if (MiddleInitialTextBox.Validate().Success)
                    return true;
                else
                {
                    Warning.Ok("Please enter a valid middle intial.", "Invalid Data Provided");
                    return false;
                }
            }
            return true;
        }

        public bool CheckRelationship()
        {
            if (RelationshipCbo.Text.IsPopulated() && Relationships.Select(r => r.Description).Contains(((Relationship)RelationshipCbo.SelectedItem).Description))
                return true;
            else
                Warning.Ok("Please select a relationship.", "Invalid Data Provided");
            return false;
        }

        public bool CheckAddress(OmniTextBox textBox, string fieldName)
        {
            if (textBox.Text.Length > 0)
            {
                if (textBox.Validate())
                    return true;
                else
                {
                    Warning.Ok("Please enter a valid " + fieldName + ". Valid addresses contain no periods, commas, or number signs.", "Invalid Data Provided");
                    return false;
                }
            }
            if (fieldName == "Address 1")
            {
                Warning.Ok("Please enter an " + fieldName + ".", "Invalid Data Provided");
                return false;
            }
            return true;
        }

        public bool CheckCity()
        {
            if (CityTextBox.Text.Length > 0)
            {
                if (CityTextBox.Validate())
                    return true;
                else
                {
                    Warning.Ok("Please enter a valid city.", "Invalid Data Provided");
                    return false;
                }
            }
            Warning.Ok("Please enter a city.", "Invalid Data Provided");
            return false;
        }

        public bool CheckState()
        {
            List<string> validCodes = new List<string>(StateCodes.Select(c => c.Code));
            if (StateCbo.Text.IsPopulated() && ((StateCode)StateCbo.SelectedItem).Code != null)
                if (validCodes.Contains(((StateCode)StateCbo.SelectedItem).Code))
                    return true;
            Warning.Ok("Please enter a valid state.", "Invalid Data Provided");
            return false;
        }

        public bool CheckZip()
        {
            if (ZipTextBox.Text.Length > 0)
            {
                if (ZipTextBox.Validate().Success && (ZipTextBox.Text.Length == 5 || ZipTextBox.Text.Length == 9))
                    return true;
                else
                {
                    Warning.Ok("Please enter a valid zip.", "Invalid Data Provided");
                    return false;
                }
            }
            Warning.Ok("Please enter a zip.", "Invalid Data Provided");
            return false;
        }

        public bool CheckPhone(NumericTextBox textBox, string fieldName)
        {
            if (textBox.Text.Length > 0)
            {
                if (textBox.Validate().Success)
                    return true;
                else
                {
                    Warning.Ok("Please enter a valid " + fieldName + ". Do not include any hyphens or parenthisis", "Invalid Data Provided");
                    return false;
                }
            }
            return true;
        }

        public bool CheckEmail()
        {
            if (EmailTextBox.Text.Length > 0)
            {
                EmailTextBox.AllowAlphaCharacters = true;
                EmailTextBox.AllowNumericCharacters = true;
                EmailTextBox.AllowedAdditionalCharacters = "!#$%&'*+-/=?^_`{|}~(),:;<>@[\\]";
                if (EmailTextBox.Validate())
                    return true;
                else
                {
                    Warning.Ok("Please enter a valid email.", "Invalid Data Provided");
                    return false;
                }
            }
            return true;
        }

        public void BindData()
        {
            POAData.RefData.FirstName = ReferenceFirstTextBox.Text;
            POAData.RefData.LastName = ReferenceLastTextBox.Text;
            POAData.RefData.MiddleIntial = MiddleInitialTextBox.Text;
            POAData.RefData.RelationshipToBorrower = Relationships.Where(r => r.Description == RelationshipCbo.Text).FirstOrDefault();
            POAData.RefData.Address1 = Address1TextBox.Text;
            POAData.RefData.Address2 = Address2TextBox.Text;
            POAData.RefData.City = CityTextBox.Text;
            POAData.RefData.State = StateCbo.Text;
            POAData.RefData.ZipCode = ZipTextBox.Text;
            POAData.RefData.PrimaryPhone = HomePhoneTextBox.Text;
            POAData.RefData.AlternatePhone = OtherPhoneTextBox.Text;
            POAData.RefData.ForeignPhone = ForeignPhoneTextBox.Text;
            POAData.RefData.EmailAddress = EmailTextBox.Text;
        }
    }
}