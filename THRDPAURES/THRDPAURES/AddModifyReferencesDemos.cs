using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace THRDPAURES
{
    public partial class AddModifyReferencesDemos : Form
    {
        private static Dictionary<string, string> RelationshipCodes = new Dictionary<string, string>()
        {
            {"01", "Unknown"},
            {"02", "Parent"},
            {"03", "Relative"},
            {"04", "Non-Relative"},
            {"05", "Employer"},
            {"06", "Spouse"},
            {"07", "Sibling"},
            {"08", "Roommate"},
            {"09", "Neighbor"},
            {"10", "Child"},
            {"11", "Friend"},
            {"12", "Guardian Ref"},
            {"13", "Physician"},
            {"14", "Survivor"},
            {"15", "Attorney Ref"},
            {"16", "POA"}
        };      
                
        private static Dictionary<string, string> PhoneCodes = new Dictionary<string, string>()
        {
            {"H", "Home"},
            {"A", "Alternate"},
            {"M", "Mobile"},
            {"W", "DayTime"}
        };

        private static Dictionary<string, string> PhoneSource = new Dictionary<string, string>()
        {
            {"01", "ELECTRONIC MEDIA"},
            {"02", "APPLICATION"},
            {"03", "PROMISSORY NOTE"},
            {"04", "APP/PROM NOTE"},
            {"05", "CORRESPONDENCE"},
            {"06", "REPAYMENT OBLIGATI"},
            {"07", "CORRECT FORM"},
            {"08", "DEFERMENT FORM"},
            {"09", "FORBEARANCE FORM"},
            {"10", "GUARANTEE STATEMENT"},
            {"11", "CREDIT REPORT"},
            {"12", "PARENT"},
            {"13", "SPOUSE"},
            {"14", "SIBLING"},
            {"15", "ROOMMATE"},
            {"16", "NEIGHBOR"},
            {"18", "AUNT/UNCLE"},
            {"19", "GRANDPARENT"},
            {"20", "COUSIN"},
            {"21", "NIECE/NEPHEW"},
            {"22", "CHILD"},
            {"23", "EMPLOYER"},
            {"24", "DIRECTORY ASSISTANC"},
            {"26", "DEPART MOTOR VEHIC"},
            {"27", "LANDLORD"},
            {"28", "MILITARY"},
            {"29", "IRS"},
            {"30", "OUT COLLECTOR"},
            {"31", "UNKNOWN"},
            {"32", "INFO FROM BRWR"},
            {"33", "RETURNED EMAIL"},
            {"41", "BORROWER PHONE CALL"},
            {"42", "2ND PRTY PHONE CAL"},
            {"43", "3RD PARTY PHONE CAL"},
            {"44", "PRISON"},
            {"45", "FRIEND"},
            {"46", "STUDENT"},
            {"47", "PAROLE"},
            {"49", "COUPON STATEMNENT"},
            {"50", "FORMER EMPLOYER"},
            {"51", "BAR ASSOCIATION"},
            {"52", "VR MAILBX"},
            {"55", "WEBMASTER"},
            {"56", "GUARANTOR"},
            {"57", "SCHOOL"},
            {"58", "NSLC"},
            {"59", "TELEPHONE COMPANY"},
            {"60", "PERSON LOCATOR"},
            {"61", "ACXIOM PH SCRUB"},
            {"62", "EMAIL"},
            {"63", "LENDER"},
            {"64", "DUPLICATE PHONE"},
            {"97", "CRC"},
            {"98", "CAM"},
            {"99", "ANNUAL STATEMENT"}
        };

        private BorrowerData BData { get; set; }
        private Dictionary<string, string> AddressTypeCodes { get; set; }
        private List<string> ForeignCodes { get; set; }
        private string CurrentAddressType { get; set; }
        private string CurrentPhoneType { get; set; }
        public ReferencesDemographics RefDemos { get; set; }
       

        public AddModifyReferencesDemos(ReferencesDemographics referneceDemos, BorrowerData bData)
        {
            InitializeComponent();
            RefDemos = referneceDemos;
            BData = bData;
            Ssn.Text = bData.Ssn;
            FirstName.Text = RefDemos.FirstName;
            MiddleName.Text = RefDemos.MiddleInital;
            LastName.Text = RefDemos.LastName;
            EmailAddress.Text = RefDemos.EmailAddress;
            LoadRelationships(BData.IsPowerOfAttorney, RefDemos.RelationshipCode);
            AddressTypeCodes = new Dictionary<string, string>();
            LoadAddressFromObject(RefDemos.Legal);
            ForeignCodes = DataAccessHelper.ExecuteList<string>("spGENR_GetCountryNames", DataAccessHelper.Database.Csys);
            ForeignCode.DataSource = ForeignCodes;
            PhoneType.DataSource = PhoneCodes.Select(p => p.Value).ToList();
            SourceCode.DataSource = PhoneSource.Select(p => p.Value).ToList();
            LoadPhoneFromObject(RefDemos.HomePhones);
            CurrentAddressType = "Legal";
            CurrentPhoneType = "Home";
            if (RefDemos.HomePhones == null)
                Mbl.SelectedIndex = 0;

            if (RefDemos.Legal != null && !RefDemos.Legal.ForeignCode.IsNullOrEmpty())
            {
                string name = DataAccessHelper.ExecuteSingle<string>("spGENR_GetCountryNameForCode", DataAccessHelper.Database.Csys, RefDemos.Legal.ForeignCode.ToSqlParameter("Code"));
                ForeignCode.SelectedItem = ForeignCodes.Where(p => p == name).First();
            }
            else
                ForeignCode.SelectedIndex = -1;

            if (BData.IsPowerOfAttorney)
                Relationship.SelectedIndex = 15;

            AddressTypeCodes = ReferencesDemographics.GetAddressTypes(referneceDemos);

            AddressType.DataSource = AddressTypeCodes.Select(p => p.Value).ToList();
            Address1.Focus();
        }

        private void LoadRelationships(bool isPowerOfAttorney, string existingCode = "01")
        {
            Relationship.DataSource = RelationshipCodes.Select(p => p.Value).ToList();
            if (isPowerOfAttorney)
                Relationship.SelectedItem = "POA";
            else
            {
                if (!existingCode.IsNullOrEmpty())
                    Relationship.SelectedItem = RelationshipCodes.Where(p => p.Key == existingCode).First().Value;
            }
        }

        private void ForeignDemos_CheckedChanged(object sender, EventArgs e)
        {
            if (ForeignDemos.Checked)
            {
                StateLabel.Text = "Foreign State:";
                State.Width = 125;
                State.Left = State.Left + 54;
                ForeignCodeLabel.Visible = true;
                ForeignCode.Visible = true;
                State.MaxLength = 15;
                State.Text = "";
            }
            else
            {
                StateLabel.Text = "State:";
                State.Width = 51;
                State.Left = State.Left - 54;
                ForeignCodeLabel.Visible = false;
                ForeignCode.Visible = false;
                State.MaxLength = 2;
                State.Text = "";
                ForeignCode.SelectedIndex = -1;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!ValidateEntry())
                return;

            SaveAddressDemos();
            SavePhoneDemos();
            RefDemos.EmailAddress = EmailAddress.Text;
            RefDemos.RelationshipCode = RelationshipCodes.Where(p => p.Value == Relationship.SelectedValue).First().Key.ToString();

            DialogResult = DialogResult.OK;
        }

        private bool ValidateEntry()
        {
            List<string> errors = new List<string>();
            if (Address1.Text.IsNullOrEmpty())
                errors.Add("You must enter an Address.");

            

            if (!Address1.Text.IsNullOrEmpty())
            {
                if (City.Text.IsNullOrEmpty())
                    errors.Add("You must enter a city.");
                if (State.Text.IsNullOrEmpty() && !ForeignDemos.Checked)
                    errors.Add("You must enter a state.");
                if (ZipCode.Text.IsNullOrEmpty())
                    errors.Add("You must enter a zip code.");
            }
            else
            {
                if (NoDemos.Checked == false)
                    errors.Add("You must select No Address Provided if the reference does not have a address.");
            }

            if (ForeignDemos.Checked)
            {
                if (!State.Text.IsNullOrEmpty())
                {
                    if (ForeignCode.SelectedIndex <= 0)
                        errors.Add("You must enter a foreign code.");
                }
            }

            Func<string, string> RemoveMask = (val) => val.Replace("", "_", "(", ")", " ", "-", "/");

            if (RemoveMask(LastVerifiedDate.Text).Trim().IsNullOrEmpty())
                errors.Add("You must enter a Last Verified Date");

            if (!RemoveMask(Phone.Text).IsNullOrEmpty() && !RemoveMask(ForeignPhone.Text).IsNullOrEmpty())
                errors.Add("You cannot have a foreign and domestic phone.");

            if (Consent.Text.Replace(" ", "").IsNullOrEmpty())
                Consent.Text = "N";

            if (errors.Any())
            {
                MessageBox.Show("Please review the following errors: \n" + string.Join("\n", errors.Select(e => " - " + e).ToArray()),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void NoDemos_CheckedChanged(object sender, EventArgs e)
        {
            LoadNoDemos(BData);
        }

        private void LoadNoDemos(BorrowerData demos)
        {
            Address1.Text = demos.Street1;
            Address2.Text = demos.Street2;
            City.Text = demos.City;
            ZipCode.Text = demos.Zip;

            if (demos.ForeignState.IsNullOrEmpty())
            {
                ForeignDemos.Checked = false;
                State.Text = demos.State;
            }
            else
            {
                ForeignDemos.Checked = true;
                State.Text = demos.ForeignState;
            }

            ForeignCode.Text = demos.ForeignCountry;
        }

        private void SaveAddressDemos()
        {
            string foreignCode = string.Empty;
            if (!ForeignCode.Text.IsNullOrEmpty())
                foreignCode = DataAccessHelper.ExecuteSingle<string>("spGENR_GetCountryCodeForName", DataAccessHelper.Database.Csys, ForeignCode.Text.ToSqlParameter("Name"));
            AddressDemographics demos = new AddressDemographics()
            {
                Street1 = Address1.Text,
                Street2 = Address2.Text,
                Street3 = Address3.Text,
                City = City.Text,
                Zip = ZipCode.Text,
                State = ForeignDemos.Checked ? "" : State.Text,
                ForeignCode = foreignCode,
                ForeignState = ForeignDemos.Checked ? State.Text : ""
            };

            string type = AddressTypeCodes.Where(p => p.Key == CurrentAddressType.Substring(0, 1)).Single().Value;

            if (type == "Legal")
                RefDemos.Legal = demos;
            else if (type == "Disbursement")
                RefDemos.Disbursement = demos;
            else if (type == "Billing")
                RefDemos.Billing = demos;
        }

        private void AddressType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SaveAddressDemos();

            string nextType = AddressType.Text;


            if (nextType == "Legal")
                LoadAddressFromObject(RefDemos.Legal);
            else if (nextType == "Disbursement")
                LoadAddressFromObject(RefDemos.Disbursement);
            else if (nextType == "Billing")
                LoadAddressFromObject(RefDemos.Billing);
        }

        private void LoadAddressFromObject(AddressDemographics demos)
        {
            if (demos == null)
                return;

            Address1.Text = demos.Street1;
            Address2.Text = demos.Street2;
            Address3.Text = demos.Street3;
            City.Text = demos.City;
            ZipCode.Text = demos.Zip;

            if (demos.ForeignCode.IsNullOrEmpty())
            {
                ForeignDemos.Checked = false;
                State.Text = demos.State;
            }
            else
            {
                ForeignDemos.Checked = true;
                State.Text = demos.ForeignState;
            }

            ForeignCode.Text = demos.ForeignCode;

        }

        private void AddressType_MouseClick(object sender, MouseEventArgs e)
        {
            CurrentAddressType = AddressType.Text;
        }

        private void PhoneType_MouseClick(object sender, MouseEventArgs e)
        {
            CurrentPhoneType = PhoneType.Text;
        }

        private void LoadPhoneFromObject(ReferencePhone phone)
        {
            if (phone == null)
            {
                Phone.Text = "";
                PhoneExt.Text = "";
                LastVerifiedDate.Text = "";
                ValidPhone.Checked = false;
                Mbl.Text = "";
                Consent.Text = "";
                SourceCode.Text = "";
                ForeignPhone.Text = "";
                ForeignPhoneExt.Text = "";
                return;
            }

            Phone.Text = phone.Phone;
            PhoneExt.Text = phone.PhoneExtension;
            LastVerifiedDate.Text = phone.LastVerifiedDate;
            ValidPhone.Checked = phone.DomesticPhoneValid.HasValue ? phone.DomesticPhoneValid.Value : false;
            Mbl.Text = phone.Mbl;
            Consent.Text = phone.Consent;
            SourceCode.SelectedItem = PhoneSource.Where(p => p.Key == phone.SourceCode).First().Value;
            ForeignPhone.Text = phone.ForeignPhone;
            ForeignPhoneExt.Text = phone.ForeignPhoneExtension;
        }

        private void SavePhoneDemos()
        {
            ReferencePhone phone = new ReferencePhone()
            {
                Phone = Phone.Text,
                PhoneExtension = PhoneExt.Text,
                DomesticPhoneValid = ValidPhone.Checked,
                ForeignPhone = ForeignPhone.Text.Replace("-", "").Replace("_", ""),
                ForeignPhoneExtension = ForeignPhoneExt.Text,
                Mbl = Mbl.Text,
                Consent = Consent.Text,
                LastVerifiedDate = LastVerifiedDate.Text,
                SourceCode = SourceCode.Text.IsNullOrEmpty() ? "" : PhoneSource.Where(p => p.Value == SourceCode.Text).First().Key
            };

            string phoneType = PhoneCodes.Where(p => p.Value == CurrentPhoneType).Single().Value;

            if (phoneType == "Home")
                RefDemos.HomePhones = phone;
            else if (phoneType == "Alternate")
                RefDemos.AlternatePhones = phone;
            else if (phoneType == "Mobile")
                RefDemos.MobilePhones = phone;
            else if (phoneType == "DayTime")
                RefDemos.DayTimePhones = phone;
        }

        private void PhoneType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SavePhoneDemos();

            string nextType = PhoneType.Text;

            if (nextType == "Home")
                LoadPhoneFromObject(RefDemos.HomePhones);
            else if (nextType == "Alternate")
                LoadPhoneFromObject(RefDemos.AlternatePhones);
            else if (nextType == "Mobile")
                LoadPhoneFromObject(RefDemos.MobilePhones);
            else if (nextType == "DayTime")
                LoadPhoneFromObject(RefDemos.DayTimePhones);
        }

        private void Mbl_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Mbl.SelectedItem.ToString() == "L")
                Consent.Text = "Y";
        }

        private void LastVerifiedDate_Leave(object sender, EventArgs e)
        {
            DateTime val = new DateTime();
            if (!DateTime.TryParse(LastVerifiedDate.Text, out val))
            {
                MessageBox.Show("You must enter a valid date, please try again.");
                LastVerifiedDate.Focus();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (BData != null && !BData.Phone.IsNullOrEmpty())
            {
                Phone.Text = BData.Phone;
                PhoneExt.Text = BData.PhoneExt;
                LastVerifiedDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ValidPhone.Checked = true;
                Mbl.Text = BData.Mbl;
                Consent.Text = BData.Consent;
                SourceCode.Text = BData.SourceCode;
                ForeignPhone.Text = BData.ForeignPhone;
                ForeignPhoneExt.Text = BData.ForeignExt;
            }
        }



    }
}
