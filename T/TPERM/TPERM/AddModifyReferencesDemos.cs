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
using Uheaa.Common.Scripts;

namespace TPERM
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
            {"W", "Work"}
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

        private ApplicationData AppData { get; set; }
        private List<string> ForeignCodes { get; set; }
        private string CurrentAddressType { get; set; }
        private string CurrentPhoneType { get; set; }


        public AddModifyReferencesDemos(ApplicationData appData)
        {
            InitializeComponent();
            AppData = appData;
            ForeignCodes = DataAccessHelper.ExecuteList<string>("spGENR_GetCountryNames", DataAccessHelper.Database.Csys);
            ForeignCode.DataSource = ForeignCodes;
            ForeignCode.SelectedIndex = -1;
            PhoneType.DataSource = PhoneCodes.Select(p => p.Value).ToList();
            LoadDemos();
        }

        private void LoadDemos()
        {
            RFName.Text = AppData.ReferenceInfo.FirstName;
            RLName.Text = AppData.ReferenceInfo.LastName;
            if (AppData.ReferenceInfo.RefAddress != null)
            {
                Address1.Text = AppData.ReferenceInfo.RefAddress.Street1;
                Address2.Text = AppData.ReferenceInfo.RefAddress.Street2;
                Address3.Text = AppData.ReferenceInfo.RefAddress.Street3;
                City.Text = AppData.ReferenceInfo.RefAddress.City;
                State.Text = AppData.ReferenceInfo.RefAddress.ForeignState.IsNullOrEmpty() ? AppData.ReferenceInfo.RefAddress.State : AppData.ReferenceInfo.RefAddress.ForeignState;
                ZipCode.Text = AppData.ReferenceInfo.RefAddress.Zip;
                AddrValid.Checked = AppData.ReferenceInfo.RefAddress.IsValid;

                if (AppData.ReferenceInfo.RefAddress.ForeignCode.IsPopulated())
                {
                    string name = DataAccessHelper.ExecuteSingle<string>("spGENR_GetCountryNameForCode", DataAccessHelper.Database.Csys, AppData.ReferenceInfo.RefAddress.ForeignCode.ToSqlParameter("Code"));
                    ForeignCode.SelectedItem = ForeignCodes.Where(p => p == name).First();
                    ForeignDemos.Checked = true;
                }
                else
                    ForeignCode.SelectedIndex = -1;
            }

            if (AppData.ReferenceInfo.HomePhone != null)
            {
                LoadPhoneFromObject(AppData.ReferenceInfo.HomePhone);
            }
        }

        private void ForeignDemos_CheckedChanged(object sender, EventArgs e)
        {
            if (ForeignDemos.Checked)
            {
                StateLabel.Text = "Foreign State:";
                StateLabel.Location = new Point(432, 84);
                State.Width = 125;
                ForeignCodeLabel.Visible = true;
                ForeignCode.Visible = true;
                State.MaxLength = 15;
                State.Text = "";
                ForeignCode.Enabled = true;
            }
            else
            {
                StateLabel.Text = "State:";
                State.Width = 51;
                StateLabel.Location = new Point(490, 84);
                ForeignCodeLabel.Visible = false;
                ForeignCode.Visible = false;
                State.MaxLength = 2;
                State.Text = "";
                ForeignCode.SelectedIndex = -1;
                ForeignCode.Enabled = false;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!ValidateEntry())
                return;

            SaveAddressDemos();
            SavePhoneDemos();
            AppData.AuthEndDate = AuthEndDate.Text.ToDateNullable();
            AppData.ReferenceInfo.NotAuthed = NoAuth.Checked;

            DialogResult = DialogResult.OK;
        }

        private bool ValidateEntry()
        {
            bool isValid = true;
            if (!NoDemos.Checked)
            {
                if (Address1.Text.IsNullOrEmpty())
                {
                    Address1.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    Address1.BackColor = SystemColors.Window;

                if (City.Text.IsNullOrEmpty())
                {
                    City.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    City.BackColor = SystemColors.Window;
                if (State.Text.IsNullOrEmpty() && !ForeignDemos.Checked)
                {
                    State.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    State.BackColor = SystemColors.Window;
                if (ZipCode.Text.IsNullOrEmpty())
                {
                    ZipCode.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    ZipCode.BackColor = SystemColors.Window;

                if (ZipCode.Text.Length < 5)
                {
                    ZipCode.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    ZipCode.BackColor = SystemColors.Window;
            }


            if (ForeignDemos.Checked)
            {
                if (!State.Text.IsNullOrEmpty())
                {
                    if (ForeignCode.SelectedIndex <= 0)
                    {
                        ForeignCode.BackColor = Color.LightPink;
                        isValid = false;
                    }
                    else
                        ForeignCode.BackColor = SystemColors.Window;
                }
            }

            Func<string, string> RemoveMask = (val) => val.Replace("", "_", "(", ")", " ", "-", "/");

            if (!NoPhone.Checked)
            {
                if (RemoveMask(Phone.Text).IsNullOrEmpty() && RemoveMask(ForeignPhone.Text).IsNullOrEmpty())
                {
                    Phone.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    Phone.BackColor = SystemColors.Window;

                if (!RemoveMask(Phone.Text).IsNullOrEmpty() && RemoveMask(Phone.Text).Length < 10)
                {
                    Phone.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    Phone.BackColor = SystemColors.Window;

                if (!RemoveMask(ForeignPhone.Text).IsNullOrEmpty() && RemoveMask(ForeignPhone.Text).Length < 9)
                {
                    Phone.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    Phone.BackColor = SystemColors.Window;

                if (PhoneType.SelectedIndex == -1)
                {
                    PhoneType.BackColor = Color.LightPink;
                    isValid = false;
                }
                else
                    PhoneType.BackColor = SystemColors.Window;
            }

            if(AuthEndDate.BackColor == Color.LightPink)
                isValid = false;

            if (!RemoveMask(Phone.Text).IsNullOrEmpty() && !RemoveMask(ForeignPhone.Text).IsNullOrEmpty())
            {
                Dialog.Error.Ok("You cannot have a foreign and domestic phone.");
                return false;
            }

            return isValid;
        }

        private void NoDemos_CheckedChanged(object sender, EventArgs e)
        {
            if (NoDemos.Checked)
            {
                ForeignDemos.Checked = ForeignDemos.Enabled = Address1.Enabled = Address2.Enabled = Address3.Enabled = City.Enabled = State.Enabled = ZipCode.Enabled = ForeignDemos.Checked  = AddrValid.Checked = AddrValid.Enabled = false;
                AppData.ReferenceInfo.RefAddress = null;
                Address1.Text = ".";
                State.Text = "PA";
                ZipCode.Text = "00000";
                City.Text = ".";
                Address2.Text = Address3.Text = string.Empty;
                ForeignCode.SelectedIndex = -1;
                ZipCode.BackColor = State.BackColor = City.BackColor = Address1.BackColor = SystemColors.Window;
            }
            else
            {
                ForeignDemos.Enabled = AddrValid.Enabled =  Address1.Enabled = Address2.Enabled = Address3.Enabled = City.Enabled = State.Enabled = ZipCode.Enabled = ForeignCode.Enabled = true;
                Address1.Text = Address2.Text = Address3.Text = City.Text = State.Text = ZipCode.Text = string.Empty;
            }
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
                ForeignState = ForeignDemos.Checked ? State.Text : "",
                IsValid = AddrValid.Checked

            };

            AppData.ReferenceInfo.RefAddress = demos;
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

        private void PhoneType_MouseClick(object sender, MouseEventArgs e)
        {
            CurrentPhoneType = PhoneType.Text;
        }


        private void SavePhoneDemos()
        {
            Func<string, string> RemoveMask = (val) => val.Replace("", "_", "(", ")", " ", "-", "/");
            ReferencePhone phone = new ReferencePhone()
            {
                Phone = RemoveMask(Phone.Text),
                PhoneExtension = PhoneExt.Text,
                ForeignPhone = RemoveMask(ForeignPhone.Text),
                ForeignPhoneExtension = ForeignPhoneExt.Text,
                SourceCode = "05",
                Mbl = "U",
                Consent = "N",
                IsValid = ValidPhone.Checked
            };

            if (phone.Phone.IsNullOrEmpty() && phone.ForeignPhone.IsNullOrEmpty())
                phone = null;

            string phoneType = PhoneCodes.Where(p => p.Value == PhoneType.Text).Single().Value;

            if (phoneType == "Home")
            {
                if (phone != null && AppData.ReferenceInfo.HomePhone != null)
                {
                    phone.Mbl = AppData.ReferenceInfo.HomePhone.Mbl ?? "U";
                    phone.Consent = AppData.ReferenceInfo.HomePhone.Consent ?? "N";
                }
                AppData.ReferenceInfo.HomePhone = phone;
            }
            else if (phoneType == "Alternate")
            {
                if (phone != null && AppData.ReferenceInfo.AltPhone != null)
                {
                    phone.Mbl = AppData.ReferenceInfo.AltPhone.Mbl ?? "U";
                    phone.Consent = AppData.ReferenceInfo.AltPhone.Consent ?? "N";
                }
                AppData.ReferenceInfo.AltPhone = phone;
            }
            else if (phoneType == "Mobile")
            {
                if (phone != null && AppData.ReferenceInfo.MobilePhone != null)
                {
                    phone.Mbl = AppData.ReferenceInfo.MobilePhone.Mbl ?? "U";
                    phone.Consent = AppData.ReferenceInfo.MobilePhone.Consent ?? "N";
                }
                AppData.ReferenceInfo.MobilePhone = phone;
            }
            else if (phoneType == "Work")
            {
                if (phone != null && AppData.ReferenceInfo.WorkPhone != null)
                {
                    phone.Mbl = AppData.ReferenceInfo.WorkPhone.Mbl ?? "U";
                    phone.Consent = AppData.ReferenceInfo.WorkPhone.Consent ?? "N";
                }
                AppData.ReferenceInfo.WorkPhone = phone;
            }
        }

        private void PhoneType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SavePhoneDemos();

            string nextType = PhoneType.SelectedItem.ToString();

            if (nextType == "Home")
                LoadPhoneFromObject(AppData.ReferenceInfo.HomePhone);
            else if (nextType == "Alternate")
                LoadPhoneFromObject(AppData.ReferenceInfo.AltPhone);
            else if (nextType == "Mobile")
                LoadPhoneFromObject(AppData.ReferenceInfo.MobilePhone);
            else if (nextType == "Work")
                LoadPhoneFromObject(AppData.ReferenceInfo.WorkPhone);
        }

        private void LoadPhoneFromObject(ReferencePhone phone)
        {
            if (phone == null)
            {
                Phone.Text = "";
                PhoneExt.Text = "";
                ValidPhone.Checked = true;
                ForeignPhone.Text = "";
                ForeignPhoneExt.Text = "";

                return;
            }

            Phone.Text = phone.Phone;
            PhoneExt.Text = phone.PhoneExtension;
            ValidPhone.Checked = phone.IsValid;
            ForeignPhone.Text = phone.ForeignPhone;
            ForeignPhoneExt.Text = phone.ForeignPhoneExtension;
            ValidPhone.Checked = phone.IsValid;
            PhoneType.BackColor  = Phone.BackColor = SystemColors.Window;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (NoPhone.Checked)
            {
                AppData.ReferenceInfo.AltPhone = AppData.ReferenceInfo.WorkPhone = AppData.ReferenceInfo.MobilePhone = AppData.ReferenceInfo.HomePhone = null;
                PhoneType.Enabled = Phone.Enabled = PhoneExt.Enabled = ValidPhone.Enabled = ForeignPhone.Enabled = ForeignPhoneExt.Enabled = false;
                Phone.Clear();
                PhoneExt.Clear();
                ForeignPhone.Clear();
                ForeignPhoneExt.Clear();
                ValidPhone.Checked = false;
                PhoneType.BackColor = Phone.BackColor = SystemColors.Window;
            }
            else
            {
                PhoneType.Enabled = Phone.Enabled = PhoneExt.Enabled = ValidPhone.Enabled = ForeignPhone.Enabled = ForeignPhoneExt.Enabled = true; ;
            }
        }

        private void AuthEndDate_Leave(object sender, EventArgs e)
        {
            if (AuthEndDate.Text == "  /  /")
            {
                AuthEndDate.BackColor = SystemColors.Window;
                return;
            }

            if (!AuthEndDate.Text.ToDateNullable().HasValue)
                AuthEndDate.BackColor = Color.LightPink;
            else
                AuthEndDate.BackColor = SystemColors.Window;
        }
    }
}
