using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;

namespace OLDEMOS
{
    public partial class Demographics : Form
    {
        public ReflectionInterface RI { get; set; }
        public Borrower Bor { get; set; }
        public bool HasLoaded { get; set; } = false;
        List<Sources> Srcs { get; set; }

        public Demographics(Borrower bor)
        {
            InitializeComponent();
            RI = Helper.RI;

            Bor = bor;
            LoadDropDownData();
            SetRequiredBackground();
            LoadBorrowerDemos();
            BwrInfo411Processor.Show411Form(bor);
        }

        private void LoadDropDownData()
        {
            List<Country> countries = Helper.DA.GetCountries();
            countries.Insert(0, new Country() { CountryName = "" });
            Country.DataSource = countries;
            Country.DisplayMember = "CountryName";
            Country.ValueMember = "CountryCode";
            HasLoaded = true;

            Srcs = Helper.DA.GetSources();
            Srcs.Insert(0, new Sources() { Name = "" });
            Source.DataSource = Srcs;
            Source.DisplayMember = "Name";
            Source.ValueMember = "Code";
        }

        private void LoadBorrowerDemos()
        {
            NameTxt.Text = $"{Bor.FirstName}{(Bor.MiddleInitial.IsPopulated() ? $" {Bor.MiddleInitial}" : "")} {Bor.LastName}";
            SsnTxt.Text = Bor.Ssn;
            AccountTxt.Text = Bor.AccountNumber.Replace(" ", "");
            DobTxt.Text = $"{Bor.DateOfBirth:MM/dd/yyyy}";
            Address1.Text = Bor.Address1;
            Address2.Text = Bor.Address2;
            City.Text = Bor.City;
            State.Text = Bor.State;
            ZipCode.Text = Bor.Zip;
            Country.Text = Bor.Country;
            AddressValid.Checked = Bor.AddressValid;
            AddressForeign.Checked = Bor.AddressForeign;
            PrimaryPhone.Text = $"{Bor.PrimaryPhone}{Bor.PrimaryPhoneExt}";
            PrimaryPhoneConsent.Checked = Bor.PrimaryPhoneConsent;
            PrimaryPhoneValid.Checked = Bor.PrimaryPhoneValid;
            AlternatePhone.Text = $"{Bor.AlternatePhone}{Bor.AlternatePhoneExt}";
            AlternatePhoneConsent.Checked = Bor.AlternatePhoneConsent;
            AlternatePhoneValid.Checked = Bor.AlternatePhoneValid;
            OtherPhone.Text = $"{Bor.OtherPhone}{Bor.OtherPhoneExt}";
            OtherPhoneConsent.Checked = Bor.OtherPhoneConsent;
            OtherPhoneValid.Checked = Bor.OtherPhoneValid;
            ForeignPrimaryPhone.Text = $"{Bor.ForeignPrimaryPhone}{Bor.ForeignPrimaryPhoneExt}";
            ForeignAltPhone.Text = $"{Bor.ForeignAltPhone}{Bor.ForeignAltPhoneExt}";
            ForeignOtherPhone.Text = $"{Bor.ForeignOtherPhone}{Bor.ForeignOtherPhoneExt}";
            Email.Text = Bor.Email;
            EmailValid.Checked = Bor.EmailValid;
        }

        private void SetRequiredBackground()
        {
            Address1.BackColor = Color.LightPink;
            City.BackColor = Color.LightPink;
            State.BackColor = Color.LightPink;
            ZipCode.BackColor = Color.LightPink;
            PrimaryPhone.BackColor = Color.LightPink;
            Source.BackColor = Color.LightPink;
        }

        private void SecurityIncidentToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string arguments = $"--ticketType Incident --region UHEAA --accountNumber {Bor.AccountNumber} --name \"{Bor.FullName}\" --state {Bor.State}";
            if (DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void PhysicalThreatToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string arguments = $"--ticketType Threat --region UHEAA --accountNumber {Bor.AccountNumber} --name \"{Bor.FullName}\" --state {Bor.State}";
            if (DataAccessHelper.TestMode)
            {
                arguments += " --dev";
            }
            Proc.Start("IncidentReporting", arguments);
        }

        private void ToolStripMenuItem411_Click(object sender, EventArgs e)
        {
            BwrInfo411Processor.Show411Form(Bor, true);
        }

        private void Source_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Srcs.Any(p => p.Name == Source.Text))
                Source.Text = "";
            else if (Source.Text.IsNullOrEmpty())
                Source.BackColor = Color.LightPink;
            else
                Source.BackColor = Color.LightYellow;
        }

        private void Address1_TextChanged(object sender, EventArgs e)
        {
            Bor.Address1Changed = false;
            if (Address1.Text == Bor.Address1)
                Address1.BackColor = Color.LightGreen;
            else if (Address1.Text.IsNullOrEmpty())
                Address1.BackColor = Color.LightPink;
            else if (Address1.Text != Bor.Address1)
            {
                Address1.BackColor = Color.LightYellow;
                Bor.Address1Changed = true;
            }
        }

        private void Address2_TextChanged(object sender, EventArgs e)
        {
            Bor.Address2Changed = false;
            if (Address2.Text == Bor.Address2)
                Address2.BackColor = Color.LightGreen;
            else if (Address2.Text.IsNullOrEmpty())
                Address2.BackColor = Color.LightPink;
            else if (Address2.Text != Bor.Address2)
            {
                Address2.BackColor = Color.LightYellow;
                Bor.Address2Changed = true;
            }
        }

        private void City_TextChanged(object sender, EventArgs e)
        {
            Bor.CityChanged = false;
            if (City.Text == Bor.City)
                City.BackColor = Color.LightGreen;
            else if (City.Text.IsNullOrEmpty())
                City.BackColor = Color.LightPink;
            else if (City.Text != Bor.City)
            {
                City.BackColor = Color.LightYellow;
                Bor.CityChanged = true;
            }
        }

        private void State_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bor.StateChanged = false;
            if (State.Text == Bor.State)
                State.BackColor = Color.LightGreen;
            else if (State.Text.IsNullOrEmpty())
                State.BackColor = Color.LightPink;
            else if (State.Text != Bor.State)
            {
                State.BackColor = Color.LightYellow;
                Bor.StateChanged = true;
            }
        }

        private void ZipCode_TextChanged(object sender, EventArgs e)
        {
            Bor.ZipChanged = false;
            if (ZipCode.Text == Bor.Zip)
                ZipCode.BackColor = Color.LightGreen;
            else if (ZipCode.Text.IsNullOrEmpty())
                ZipCode.BackColor = Color.LightPink;
            else if (ZipCode.Text != Bor.Zip)
            {
                ZipCode.BackColor = Color.LightYellow;
                Bor.ZipChanged = true;
            }
        }

        private void Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HasLoaded)
            {
                Bor.CountryChanged = false;
                if (Country.Text == Bor.Country)
                    Country.BackColor = Color.LightGreen;
                else if (Country.Text != Bor.Country)
                {
                    Country.BackColor = Color.LightYellow;
                    Bor.CountryChanged = true;
                }
                if (Country.Text.IsNullOrEmpty() && Bor.Country.IsNullOrEmpty())
                    Country.BackColor = SystemColors.Window;
            }
        }

        private void CheckAddressForeign(object sender, EventArgs e)
        {
            Bor.AddressForeignChanged = false;
            if (AddressForeign.Checked && Bor.AddressForeign)
                AddressForeign.BackColor = Color.LightGreen;
            else if ((AddressForeign.Checked && !Bor.AddressForeign) || (!AddressForeign.Checked && Bor.AddressForeign))
            {
                AddressForeign.BackColor = Color.LightYellow;
                Bor.AddressForeignChanged = true;
            }
            else if (!AddressForeign.Checked && !Bor.AddressForeign)
                AddressForeign.BackColor = SystemColors.Control;
        }

        private void CheckAddressValid(object sender, EventArgs e)
        {
            Bor.AddressValidChanged = false;
            if (AddressValid.Checked && Bor.AddressValid)
                AddressValid.BackColor = Color.LightGreen;
            else if ((AddressValid.Checked && !Bor.AddressValid) || (!AddressValid.Checked && Bor.AddressValid))
            {
                AddressValid.BackColor = Color.LightYellow;
                Bor.AddressValidChanged = true;
            }
            else if (!AddressValid.Checked && !Bor.AddressValid)
                AddressValid.BackColor = SystemColors.Control;

            //Makes sure the address is provided if valid address is clicked.
            if (AddressValid.Checked)
            {
                if (Address1.Text.IsNullOrEmpty())
                    Address1.BackColor = Color.LightPink;
                if (City.Text.IsNullOrEmpty())
                    City.BackColor = Color.LightPink;
                if (State.Text.IsNullOrEmpty())
                    State.BackColor = Color.Pink;
                if (ZipCode.Text.IsNullOrEmpty())
                    ZipCode.BackColor = Color.LightPink;
            }
        }

        private void PrimaryPhone_TextChanged(object sender, EventArgs e)
        {
            Bor.PrimaryPhoneChanged = false;
            if (PrimaryPhone.Text.FormatPhone() == $"{Bor.PrimaryPhone.FormatPhone()}{Bor.PrimaryPhoneExt}")
                PrimaryPhone.BackColor = Color.LightGreen;
            else if (PrimaryPhone.Text.FormatPhone().IsNullOrEmpty() && ForeignPrimaryPhone.Text.FormatPhone().IsNullOrEmpty())
                PrimaryPhone.BackColor = Color.LightPink;
            else if (PrimaryPhone.Text.FormatPhone() != $"{Bor.PrimaryPhone.FormatPhone()}{Bor.PrimaryPhoneExt}")
            {
                PrimaryPhone.BackColor = Color.LightYellow;
                Bor.PrimaryPhoneChanged = true;
            }

            if (PrimaryPhone.Text.FormatPhone().IsNullOrEmpty() && !PrimaryPhoneConsent.Checked && !PrimaryPhoneValid.Checked)
                PrimaryPhone.BackColor = SystemColors.Window;
            else if (PrimaryPhone.Text.FormatPhone().IsNullOrEmpty() && (PrimaryPhoneConsent.Checked || PrimaryPhoneValid.Checked))
                PrimaryPhone.BackColor = Color.LightPink;
        }

        private void CheckPrimaryPhoneConsent(object sender, EventArgs e)
        {
            Bor.PrimaryPhoneConsentChanged = false;
            if (PrimaryPhoneConsent.Checked && Bor.PrimaryPhoneConsent)
                PrimaryPhoneConsent.BackColor = Color.LightGreen;
            else if ((PrimaryPhoneConsent.Checked && !Bor.PrimaryPhoneConsent) || (!PrimaryPhoneConsent.Checked && Bor.PrimaryPhoneConsent))
            {
                PrimaryPhoneConsent.BackColor = Color.LightYellow;
                Bor.PrimaryPhoneConsentChanged = true;
            }
            else if (!PrimaryPhoneConsent.Checked && !Bor.PrimaryPhoneConsent)
                PrimaryPhoneConsent.BackColor = SystemColors.Control;

            if (PrimaryPhoneConsent.Checked && PrimaryPhone.Text.FormatPhone().IsNullOrEmpty())
                PrimaryPhone.BackColor = Color.LightPink;
            else if (!PrimaryPhoneConsent.Checked && !PrimaryPhoneValid.Checked)
                PrimaryPhone_TextChanged(sender, e); //Call the text changed to reset the colors}
        }

        private void CheckPrimaryPhoneValid(object sender, EventArgs e)
        {
            Bor.PrimaryPhoneValidChanged = false;
            if (PrimaryPhoneValid.Checked && Bor.PrimaryPhoneValid)
                PrimaryPhoneValid.BackColor = Color.LightGreen;
            else if ((PrimaryPhoneValid.Checked && !Bor.PrimaryPhoneValid) || (!PrimaryPhoneValid.Checked && Bor.PrimaryPhoneValid))
            {
                PrimaryPhoneValid.BackColor = Color.LightYellow;
                Bor.PrimaryPhoneValidChanged = true;
            }
            else if (!PrimaryPhoneValid.Checked && !Bor.PrimaryPhoneValid)
                PrimaryPhoneValid.BackColor = SystemColors.Control;

            if (PrimaryPhoneValid.Checked && PrimaryPhone.Text.FormatPhone().IsNullOrEmpty())
                PrimaryPhone.BackColor = Color.LightPink;
            else if (!PrimaryPhoneConsent.Checked && !PrimaryPhoneValid.Checked)
                PrimaryPhone_TextChanged(sender, e); //Call the text changed to reset the colors}
        }

        private void AlternatePhone_TextChanged(object sender, EventArgs e)
        {
            Bor.AlternatePhoneChanged = false;
            if (AlternatePhone.Text.FormatPhone() == $"{Bor.AlternatePhone.FormatPhone()}{Bor.AlternatePhoneExt}")
                AlternatePhone.BackColor = Color.LightGreen;
            else if (AlternatePhone.Text.FormatPhone() != $"{Bor.AlternatePhone.FormatPhone()}{Bor.AlternatePhoneExt}")
            {
                AlternatePhone.BackColor = Color.LightYellow;
                Bor.AlternatePhoneChanged = true;
            }

            if (AlternatePhone.Text.FormatPhone().IsNullOrEmpty() && !AlternatePhoneConsent.Checked && !AlternatePhoneValid.Checked)
                AlternatePhone.BackColor = SystemColors.Window;
            else if (AlternatePhone.Text.FormatPhone().IsNullOrEmpty() && (AlternatePhoneConsent.Checked || AlternatePhoneValid.Checked))
                AlternatePhone.BackColor = Color.LightPink;
        }

        private void CheckAlternateConsent(object sender, EventArgs e)
        {
            Bor.AlternatePhoneConsentChanged = false;
            if (AlternatePhoneConsent.Checked && Bor.AlternatePhoneConsent)
                AlternatePhoneConsent.BackColor = Color.LightGreen;
            else if ((AlternatePhoneConsent.Checked && !Bor.AlternatePhoneConsent) || (!AlternatePhoneConsent.Checked && Bor.AlternatePhoneConsent))
            {
                AlternatePhoneConsent.BackColor = Color.LightYellow;
                Bor.AlternatePhoneConsentChanged = true;
            }
            else if (!AlternatePhoneConsent.Checked && !Bor.AlternatePhoneConsent)
                AlternatePhoneConsent.BackColor = SystemColors.Control;

            if (AlternatePhoneConsent.Checked && AlternatePhone.Text.FormatPhone().IsNullOrEmpty())
                AlternatePhone.BackColor = Color.LightPink;
            else if (!AlternatePhoneConsent.Checked && !AlternatePhoneValid.Checked)
                AlternatePhone_TextChanged(sender, e); //Call the text changed to reset the colors}
        }

        private void CheckAlternateValid(object sender, EventArgs e)
        {
            Bor.AlternatePhoneValidChanged = false;
            if (AlternatePhoneValid.Checked && Bor.AlternatePhoneValid)
                AlternatePhoneValid.BackColor = Color.LightGreen;
            else if ((AlternatePhoneValid.Checked && !Bor.AlternatePhoneValid) || (!AlternatePhoneValid.Checked && Bor.AlternatePhoneValid))
            {
                AlternatePhoneValid.BackColor = Color.LightYellow;
                Bor.AlternatePhoneValidChanged = true;
            }
            else if (!AlternatePhoneValid.Checked && !Bor.AlternatePhoneValid)
                AlternatePhoneValid.BackColor = SystemColors.Control;

            if (AlternatePhoneValid.Checked && AlternatePhone.Text.FormatPhone().IsNullOrEmpty())
                AlternatePhone.BackColor = Color.LightPink;
            else if (!AlternatePhoneConsent.Checked && !AlternatePhoneValid.Checked)
                AlternatePhone_TextChanged(sender, e); //Call the text changed to reset the colors}
        }

        private void OtherPhone_TextChanged(object sender, EventArgs e)
        {
            Bor.OtherPhoneChanged = false;
            if (OtherPhone.Text.FormatPhone() == $"{Bor.OtherPhone.FormatPhone()}{Bor.OtherPhoneExt}")
                OtherPhone.BackColor = Color.LightGreen;
            else if (OtherPhone.Text.FormatPhone() != $"{Bor.OtherPhone.FormatPhone()}{Bor.OtherPhoneExt}")
            {
                OtherPhone.BackColor = Color.LightYellow;
                Bor.OtherPhoneChanged = true;
            }

            if (OtherPhone.Text.FormatPhone().IsNullOrEmpty() && !OtherPhoneConsent.Checked && !OtherPhoneValid.Checked)
                OtherPhone.BackColor = SystemColors.Window;
            else if (OtherPhone.Text.FormatPhone().IsNullOrEmpty() && (OtherPhoneConsent.Checked || OtherPhoneValid.Checked))
                OtherPhone.BackColor = Color.LightPink;
        }

        private void CheckOtherPhoneConsent(object sender, EventArgs e)
        {
            Bor.OtherPhoneConsentChanged = false;
            if (OtherPhoneConsent.Checked && Bor.OtherPhoneConsent)
                OtherPhoneConsent.BackColor = Color.LightGreen;
            else if ((OtherPhoneConsent.Checked && !Bor.OtherPhoneConsent) || (!OtherPhoneConsent.Checked && Bor.OtherPhoneConsent))
            {
                OtherPhoneConsent.BackColor = Color.LightYellow;
                Bor.OtherPhoneConsentChanged = true;
            }
            else if (!OtherPhoneConsent.Checked && !Bor.OtherPhoneConsent)
                OtherPhoneConsent.BackColor = SystemColors.Control;

            if (OtherPhoneConsent.Checked && OtherPhone.Text.FormatPhone().IsNullOrEmpty())
                OtherPhone.BackColor = Color.LightPink;
            else if (!OtherPhoneConsent.Checked && !OtherPhoneValid.Checked)
                OtherPhone_TextChanged(sender, e); //Call the text changed to reset the colors}
        }

        private void CheckOtherPhoneValid(object sender, EventArgs e)
        {
            Bor.OtherPhoneValidChanged = false;
            if (OtherPhoneValid.Checked && Bor.OtherPhoneValid)
                OtherPhoneValid.BackColor = Color.LightGreen;
            else if ((OtherPhoneValid.Checked && !Bor.OtherPhoneValid) || (!OtherPhoneValid.Checked && Bor.OtherPhoneValid))
            {
                OtherPhoneValid.BackColor = Color.LightYellow;
                Bor.OtherPhoneValidChanged = true;
            }
            else if (!OtherPhoneValid.Checked && !Bor.OtherPhoneValid)
                OtherPhoneValid.BackColor = SystemColors.Control;

            if (OtherPhoneValid.Checked && OtherPhone.Text.FormatPhone().IsNullOrEmpty())
                OtherPhone.BackColor = Color.LightPink;
            else if (!OtherPhoneValid.Checked && !OtherPhoneConsent.Checked)
                OtherPhone_TextChanged(sender, e); //Call the text changed to reset the colors
        }

        private void ForeignPrimaryPhone_TextChanged(object sender, EventArgs e)
        {
            Bor.ForeignPrimaryPhoneChanged = false;
            if (ForeignPrimaryPhone.Text.FormatPhone() == $"{Bor.ForeignPrimaryPhone.FormatPhone()}{Bor.ForeignPrimaryPhoneExt}")
                ForeignPrimaryPhone.BackColor = Color.LightGreen;
            else if (ForeignPrimaryPhone.Text.FormatPhone() != $"{Bor.ForeignPrimaryPhone.FormatPhone()}{Bor.ForeignPrimaryPhoneExt}")
            {
                ForeignPrimaryPhone.BackColor = Color.LightYellow;
                Bor.ForeignPrimaryPhoneChanged = true;
            }
        }

        private void ForeignAltPhone_TextChanged(object sender, EventArgs e)
        {
            Bor.ForeignAltPhoneChanged = false;
            if (ForeignAltPhone.Text.FormatPhone() == $"{Bor.ForeignAltPhone.FormatPhone()}{Bor.ForeignAltPhoneExt}")
                ForeignAltPhone.BackColor = Color.LightGreen;
            else if (ForeignAltPhone.Text.FormatPhone() != $"{Bor.ForeignAltPhone.FormatPhone()}{Bor.ForeignAltPhoneExt}")
            {
                ForeignAltPhone.BackColor = Color.LightYellow;
                Bor.ForeignAltPhoneChanged = true;
            }
        }

        private void ForeignOtherPhone_TextChanged(object sender, EventArgs e)
        {
            Bor.ForeignOtherPhoneChanged = false;
            if (ForeignOtherPhone.Text.FormatPhone() == $"{Bor.ForeignOtherPhone.FormatPhone()}{Bor.ForeignOtherPhoneExt}")
                ForeignOtherPhone.BackColor = Color.LightGreen;
            else if (ForeignOtherPhone.Text.FormatPhone() != $"{Bor.ForeignOtherPhone.FormatPhone()}{Bor.ForeignOtherPhoneExt}")
            {
                ForeignOtherPhone.BackColor = Color.LightYellow;
                Bor.ForeignOtherPhoneChanged = true;
            }
        }

        private void Email_TextChanged(object sender, EventArgs e)
        {
            Bor.EmailChanged = false;
            if (Email.Text == Bor.Email && Email.Text.IsPopulated())
                Email.BackColor = Color.LightGreen;
            else if ((Email.Text.IsPopulated() && !IsValidEmail()) || (Email.Text.IsNullOrEmpty() && Bor.Email.IsNullOrEmpty() && EmailValid.Checked))
                Email.BackColor = Color.LightPink;
            else if (Email.Text != Bor.Email && IsValidEmail())
            {
                Email.BackColor = Color.LightYellow;
                Bor.EmailChanged = true;
            }
            else if (Email.Text.IsNullOrEmpty() && Bor.Email.IsNullOrEmpty() && !EmailValid.Checked)
                Email.BackColor = SystemColors.Window;

        }

        private void CheckEmailValid(object sender, EventArgs e)
        {
            if (EmailValid.Checked && Bor.EmailValid)
                EmailValid.BackColor = Color.LightGreen;
            else if ((EmailValid.Checked && !Bor.EmailValid) || (!EmailValid.Checked && Bor.EmailValid))
            {
                EmailValid.BackColor = Color.LightYellow;
                Bor.EmailValidChanged = true;
            }
            else if (!EmailValid.Checked && !Bor.EmailValid)
                EmailValid.BackColor = SystemColors.Control;
            if (EmailValid.Checked && Email.Text.IsNullOrEmpty())
                Email.BackColor = Color.LightPink;
            else if (!EmailValid.Checked && Email.Text.IsNullOrEmpty())
                Email.BackColor = SystemColors.Window;
        }

        private void State_Leave(object sender, EventArgs e)
        {
            State.Text = State.Text.ToUpper();
            if (!State.Items.Cast<string>().Contains(State.Text))
                State.Text = "";
        }

        private void Process_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                UpdateDemos();
                this.DialogResult = DialogResult.OK;
            }
            else
                return;
        }

        private bool ValidateFields()
        {
            string message = "Please provide the missing data:\r\n\r";
            bool isValid = true;

            if (!Srcs.Any(p => p.Name == Source.Text))
            {
                message += "\r\nSource";
                isValid = false;
            }

            if (Address1.Text.IsNullOrEmpty())
            {
                message += "\r\nAddress1";
                isValid = false;
            }

            if (City.Text.IsNullOrEmpty())
            {
                message += "\r\nCity";
                isValid = false;
            }

            if (State.Text.IsNullOrEmpty())
            {
                message += "\r\nState";
                isValid = false;
            }

            if (ZipCode.Text.IsNullOrEmpty())
            {
                message += "\r\nZipCode";
                isValid = false;
            }

            if (PrimaryPhone.Text.FormatPhone().IsNullOrEmpty() && ForeignPrimaryPhone.Text.FormatPhone().IsNullOrEmpty())
            {
                message += "\r\nPrimary Phone or Foreign Phone";
                isValid = false;
            }

            if (EmailValid.Checked && Email.Text.IsNullOrEmpty())
            {
                message += "\r\nEmail required when Valid is checked";
                isValid = false;
            }

            message += CheckValidPhones();

            if (!isValid)
                Error.Ok(message);

            if (CheckValidity()) //If they want to review the validity indicators, this will be true and will return false to stay on the UI.
                return false;

            return isValid;
        }

        private string CheckValidPhones()
        {
            string message = "";
            if (PrimaryPhone.Text.ToLower().Split('x')[0].FormatPhone().Length != 10)
                message += "\r\nThe Primary Phone is not formatted correctly";
            if (AlternatePhone.Text.ToLower().Split('x')[0].FormatPhone().Length != 10)
                message += "\r\nThe Alternate Phone is not formatted correctly";
            if (OtherPhone.Text.ToLower().Split('x')[0].FormatPhone().Length != 10)
                message += "\r\nThe Other Phone is not formatted correctly";
            if (PrimaryPhone.Text.ToLower().Split('x')[0].FormatPhone().Length < 10)
                message += "\r\nThe Primary Phone is too short";
            if (PrimaryPhone.Text.ToLower().Split('x')[0].FormatPhone().Length < 10)
                message += "\r\nThe Primary Phone is too short";
            if (PrimaryPhone.Text.ToLower().Split('x')[0].FormatPhone().Length < 10)
                message += "\r\nThe Primary Phone is too short";
            return message;
        }

        private bool CheckValidity()
        {
            string message = "The following fields are provided but not set as valid. Do you want to review these fields? Choose no to continue processing without a review.\r\n";
            bool needsCheck = false;

            if (Address1.Text.IsPopulated() && !AddressValid.Checked)
            {
                message += "\r\nAddress Validity";
                needsCheck = true;
            }

            if (PrimaryPhone.Text.FormatPhone().IsPopulated() && !PrimaryPhoneValid.Checked)
            {
                message += "\r\nPrimary Phone Validity";
                needsCheck = true;
            }

            if (AlternatePhone.Text.FormatPhone().IsPopulated() && !AlternatePhoneValid.Checked)
            {
                message += "\r\nAlternate Phone Validity";
                needsCheck = true;
            }

            if (OtherPhone.Text.FormatPhone().IsPopulated() && !OtherPhoneValid.Checked)
            {
                message += "\r\nOther Phone Validity";
                needsCheck = true;
            }

            if (Email.Text.IsPopulated() && !EmailValid.Checked)
            {
                message += "\r\nEmail Validity";
                needsCheck = true;
            }

            if (needsCheck && !Question.YesNo(message))
                return false;
            return true;
        }

        /// <summary>
        /// Check the email address to make sure it is in the proper format
        /// </summary>
        public bool IsValidEmail()
        {
            string email = Email.Text;
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = System.Text.RegularExpressions.Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return System.Text.RegularExpressions.Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private void UpdateDemos()
        {
        }
    }

    public static class PhoneExtension
    {
        public static string FormatPhone(this string phone)
        {
            return phone.ToLower().Replace("x", "").Replace("_", "").Replace("-", "").Trim();
        }
    }
}