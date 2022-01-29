using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;
using System.Linq.Expressions;
using System.Reflection;
using Uheaa.Common.DataAccess;
using System.Net.Mail;
using Uheaa.Common.Scripts;

namespace MDIntermediary
{
    public partial class BorrowerInfoControl : UserControl
    {
        public bool EnableNinetyDayValidations { get; set; } = false;

        public BorrowerInfoControl()
        {
            InitializeComponent();
            string[] normalList = new string[] { "", "M", "L", "U" };
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            var initPhone = new Action<PhoneBox, ComboBox, CheckButton, CheckButton>((pb, cob, consent, foreign) =>
            {
                Action setSize = new Action(() =>
                {
                    if (pb.IsForeign)
                        pb.Font = new Font("Arial", 8);
                    else
                        pb.Font = new Font("Arial", 10);
                });
                cob.Items.AddRange(normalList);
                ToolTip tt = new ToolTip();
                tt.SetToolTip(cob, "M: Mobile\nL: Landline\nU: Unknown");
                cob.SelectedIndexChanged += (o, ea) =>
                {
                    if (cob.Text == "L")
                        consent.IsChecked = true;
                    else
                        consent.IsChecked = false;
                };
                foreign.Click += (o, ea) =>
                {
                    pb.IsForeign = foreign.IsChecked;
                };
                pb.TextChanged += (o, ea) =>
                {
                    foreign.IsChecked = pb.IsForeign;
                };
                setSize();
            });
            initPhone(HomePhoneBox, HomePhoneTypeBox, HomePhoneConsentButton, HomePhoneForeignButton);
            initPhone(OtherPhoneBox, OtherPhoneTypeBox, OtherPhoneConsentButton, OtherPhoneForeignButton);
            initPhone(OtherPhone2Box, OtherPhone2TypeBox, OtherPhone2ConsentButton, OtherPhone2ForeignButton);

            this.AllowPoBox = true;
            try
            {
                this.ForeignCountryBox.DataSource = new Country[] { new Country() { CountryName = "" } }.Union(Country.GetCountries()).ToList();
            }
            catch (Exception)
            {
                //in designer, don't handle
            }
            this.ForeignCountryBox.DisplayMember = "CountryName";
            this.ForeignCountryBox.ValueMember = "CountryCode";
            this.ForeignCountryBox.KeyPress += (o, ea) => ea.KeyChar = char.ToUpper(ea.KeyChar);
        }

        private MDBorrowerDemographics demographics;
        public bool NoPhone { get { return NoPhoneBox.IsChecked; } }
        public bool EmailEnabled
        {
            get { return EmailBox.Enabled; }
            set
            {
                EmailBox.Enabled = OtherEmailBox.Enabled = OtherEmail2Box.Enabled = EmailVer.Enabled = OtherEmailVer.Enabled = OtherEmail2Ver.Enabled = value;
                if (!value)
                {
                    emailBindManager.RevertChanges();
                    otherEmailBindManager.RevertChanges();
                    otherEmail2BindManager.RevertChanges();
                }
            }
        }

        //Should be called in Initialize and Purplify
        public void SetNeedsVerification()
        {
            //Set needs verification always colors the background colors of the elements in case the last time they were set as error and no longer need to be
            Color errorColor = Color.Red;
            Color warningColor = Color.Yellow;
            if (!EnableNinetyDayValidations)
            {
                //set colors to normal background purple
                errorColor = Color.FromArgb(184, 174, 231);
                warningColor = Color.FromArgb(184, 174, 231);
            }

            DateTime NinetyDaysAgo = DateTime.Today.AddDays(-89); //We use 89 days so that the inequality includes the 90th day
            DateTime? addressVerifiedDate = demographics.SPAddrVerDt.ToDateNullable();
            if (addressVerifiedDate.HasValue && addressVerifiedDate < NinetyDaysAgo || !EnableNinetyDayValidations)
            {
                bool addressInvalid = demographics.SPAddrInd == "N";
                Address1Label.BackColor = addressInvalid ? errorColor : warningColor;
            }

            //Only verify phones if no-phone is not selected
            if (!NoPhone)
            {
                DateTime? homePhoneVerifiedDate = demographics.HomePhoneVerificationDate.ToDateNullable();
                if (homePhoneVerifiedDate.HasValue && homePhoneVerifiedDate < NinetyDaysAgo || !EnableNinetyDayValidations)
                {
                    bool homePhoneInvalid = demographics.HomePhoneValidityIndicator == "N";
                    HomePhoneLabel.BackColor = homePhoneInvalid ? errorColor : warningColor;
                }

                DateTime? otherPhone1VerifiedDate = demographics.OtherPhoneVerificationDate.ToDateNullable();
                bool otherPhoneValid = demographics.OtherPhoneValidityIndicator == "Y";
                if (otherPhone1VerifiedDate.HasValue && otherPhone1VerifiedDate < NinetyDaysAgo && otherPhoneValid || !EnableNinetyDayValidations)
                {
                    OtherPhoneLabel.BackColor = warningColor;
                }

                DateTime? otherPhone2VerifiedDate = demographics.OtherPhone2VerificationDate.ToDateNullable();
                bool otherPhone2Valid = demographics.OtherPhone2ValidityIndicator == "Y";
                if (otherPhone2VerifiedDate.HasValue && otherPhone2VerifiedDate < NinetyDaysAgo && otherPhone2Valid || !EnableNinetyDayValidations)
                {
                    OtherPhone2Label.BackColor = warningColor;
                }
            }

            DateTime? emailVerifiedDate = demographics.SPEmailVerDt.ToDateNullable();
            if (EmailEnabled && emailVerifiedDate.HasValue && emailVerifiedDate < NinetyDaysAgo || !EnableNinetyDayValidations)
            {
                bool emailInvalid = demographics.SPEmailInd == "N";
                EmailLabel.BackColor = emailInvalid ? errorColor : warningColor;
            }

            DateTime? otherEmailVerifiedDate = demographics.SPOtEmailVerDt.ToDateNullable();
            bool otherEmailValid = demographics.SPOtEmailInd == "Y";
            if (EmailEnabled && otherEmailVerifiedDate.HasValue && otherEmailVerifiedDate < NinetyDaysAgo && otherEmailValid || !EnableNinetyDayValidations)
            {
                OtherEmailLabel.BackColor = warningColor;
            }

            DateTime? otherEmail2VerifiedDate = demographics.SPOt2EmailVerDt.ToDateNullable();
            bool otherEmail2Valid = demographics.SPOt2EmailInd == "Y";
            if (EmailEnabled && otherEmail2VerifiedDate.HasValue && otherEmail2VerifiedDate < NinetyDaysAgo && otherEmail2Valid || !EnableNinetyDayValidations)
            {
                OtherEmail2Label.BackColor = warningColor;
            }
        }

        public void SetIVRValidated()
        {
            AddressVer.SetValidSelection();
            HomePhoneVer.SetValidSelection();
        }

        public List<string> ValidateNeedsVerifications()
        {
            List<string> errors = new List<string>();

            if (EnableNinetyDayValidations)
            {
                DateTime NinetyDaysAgo = DateTime.Today.AddDays(-89); //we use 89 so that the inequality include the 90th day
                DateTime? addressVerifiedDate = demographics.SPAddrVerDt.ToDateNullable();
                bool addressValidated = DemographicVerifications.Address == VerificationSelection.ValidWithChange
                    || DemographicVerifications.Address == VerificationSelection.ValidNoChange
                    || DemographicVerifications.Address == VerificationSelection.ValidWithChangeAndInvalidateFirst
                    || DemographicVerifications.Address == VerificationSelection.RefusedNoChange;
                bool addressInvalid = DemographicVerifications.Address == VerificationSelection.InvalidNoChange;
                bool invalidatingValidAddress = addressInvalid && demographics.SPAddrInd != "N";
                //not validating address && not invalidating valid address 
                if (addressVerifiedDate.HasValue && addressVerifiedDate < NinetyDaysAgo && !addressValidated && !invalidatingValidAddress)
                {
                    if (demographics.SPAddrInd == "N")
                    {
                        errors.Add("Address must be updated to resolve invalid demographics.");
                    }
                    else
                    {
                        errors.Add("Address must be verified because it has not been verified in the last 90 days.");
                    }
                }

                //Only verify phones if no-phone is not selected
                if (!NoPhone)
                {
                    DateTime? homePhoneVerifiedDate = demographics.HomePhoneVerificationDate.ToDateNullable();
                    bool homePhoneValidated = DemographicVerifications.HomePhone == VerificationSelection.ValidWithChange
                        || DemographicVerifications.HomePhone == VerificationSelection.ValidNoChange
                        || DemographicVerifications.HomePhone == VerificationSelection.ValidWithChangeAndInvalidateFirst
                        || DemographicVerifications.HomePhone == VerificationSelection.RefusedNoChange;
                    bool homePhoneInvalid = DemographicVerifications.HomePhone == VerificationSelection.InvalidNoChange;
                    bool invalidatingValidHomePhone = homePhoneInvalid && demographics.HomePhoneValidityIndicator != "N";
                    if (homePhoneVerifiedDate.HasValue && homePhoneVerifiedDate < NinetyDaysAgo && !homePhoneValidated && !invalidatingValidHomePhone)
                    {
                        if (demographics.HomePhoneValidityIndicator == "N")
                        {
                            errors.Add("Home phone must be updated to resolve invalid demographics.");
                        }
                        else
                        {
                            errors.Add("Home phone must be verified because it has not been verified in the last 90 days.");
                        }
                    }

                    DateTime? otherPhone1VerifiedDate = demographics.OtherPhoneVerificationDate.ToDateNullable();
                    bool otherPhoneValidated = DemographicVerifications.OtherPhone == VerificationSelection.ValidWithChange
                        || DemographicVerifications.OtherPhone == VerificationSelection.ValidNoChange
                        || DemographicVerifications.OtherPhone == VerificationSelection.RefusedNoChange;
                    bool otherPhoneInvalidated = DemographicVerifications.OtherPhone == VerificationSelection.InvalidNoChange || DemographicVerifications.OtherPhone == VerificationSelection.ValidWithChangeAndInvalidateFirst;
                    bool otherPhoneValid = demographics.OtherPhoneValidityIndicator == "Y";
                    if (otherPhone1VerifiedDate.HasValue && otherPhone1VerifiedDate < NinetyDaysAgo && !otherPhoneValidated && !otherPhoneInvalidated && otherPhoneValid)
                    {
                        errors.Add("Other phone 1 must be verified because it has not been verified in the last 90 days.");
                    }

                    DateTime? otherPhone2VerifiedDate = demographics.OtherPhone2VerificationDate.ToDateNullable();
                    bool otherPhone2Validated = DemographicVerifications.OtherPhone2 == VerificationSelection.ValidWithChange
                        || DemographicVerifications.OtherPhone2 == VerificationSelection.ValidNoChange
                        || DemographicVerifications.OtherPhone2 == VerificationSelection.RefusedNoChange;
                    bool otherPhone2Invalidated = DemographicVerifications.OtherPhone2 == VerificationSelection.InvalidNoChange || DemographicVerifications.OtherPhone2 == VerificationSelection.ValidWithChangeAndInvalidateFirst;
                    bool otherPhone2Valid = demographics.OtherPhone2ValidityIndicator == "Y";
                    if (otherPhone2VerifiedDate.HasValue && otherPhone2VerifiedDate < NinetyDaysAgo && !otherPhone2Validated && !otherPhone2Invalidated && otherPhone2Valid)
                    {
                        errors.Add("Other phone 2 must be verified because it has not been verified in the last 90 days.");
                    }
                }

                DateTime? emailVerifiedDate = demographics.SPEmailVerDt.ToDateNullable();
                bool emailValidated = DemographicVerifications.Email == VerificationSelection.ValidWithChange
                    || DemographicVerifications.Email == VerificationSelection.ValidNoChange
                    || DemographicVerifications.Email == VerificationSelection.ValidWithChangeAndInvalidateFirst
                    || DemographicVerifications.Email == VerificationSelection.RefusedNoChange;
                bool emailInvalid = DemographicVerifications.Email == VerificationSelection.InvalidNoChange;
                bool invalidatingValidEmail = emailInvalid && demographics.SPEmailInd != "N";
                if (EmailEnabled && emailVerifiedDate.HasValue && emailVerifiedDate < NinetyDaysAgo && !emailValidated && !invalidatingValidEmail)
                {
                    if (demographics.SPEmailInd == "N")
                    {
                        errors.Add("Email must be updated to resolve invalid demographics.");
                    }
                    else
                    {
                        errors.Add("Email must be verified because it has not been verified in the last 90 days.");
                    }
                }

                DateTime? otherEmailVerifiedDate = demographics.SPOtEmailVerDt.ToDateNullable();
                bool otherEmailValidated = DemographicVerifications.OtherEmail == VerificationSelection.ValidWithChange
                    || DemographicVerifications.OtherEmail == VerificationSelection.ValidNoChange
                    || DemographicVerifications.OtherEmail == VerificationSelection.RefusedNoChange;
                bool otherEmailInvalidated = DemographicVerifications.OtherEmail == VerificationSelection.InvalidNoChange || DemographicVerifications.OtherEmail == VerificationSelection.ValidWithChangeAndInvalidateFirst;
                bool otherEmailValid = demographics.SPOtEmailInd == "Y";
                if (EmailEnabled && otherEmailVerifiedDate.HasValue && otherEmailVerifiedDate < NinetyDaysAgo && !otherEmailValidated && !otherEmailInvalidated && otherEmailValid)
                {
                    errors.Add("Other email 1 must be verified because it has not been verified in the last 90 days.");
                }

                DateTime? otherEmail2VerifiedDate = demographics.SPOt2EmailVerDt.ToDateNullable();
                bool otherEmail2Validated = DemographicVerifications.OtherEmail2 == VerificationSelection.ValidWithChange
                    || DemographicVerifications.OtherEmail2 == VerificationSelection.ValidNoChange
                    || DemographicVerifications.OtherEmail2 == VerificationSelection.RefusedNoChange;
                bool otherEmail2Invalidated = DemographicVerifications.OtherEmail2 == VerificationSelection.InvalidNoChange || DemographicVerifications.OtherEmail2 == VerificationSelection.ValidWithChangeAndInvalidateFirst;
                bool otherEmail2Valid = demographics.SPOt2EmailInd == "Y";
                if (EmailEnabled && otherEmail2VerifiedDate.HasValue && otherEmail2VerifiedDate < NinetyDaysAgo && !otherEmail2Validated && !otherEmail2Invalidated && otherEmail2Valid)
                {
                    errors.Add("Other email 2 must be verified because it has not been verified in the last 90 days.");
                }
            }
            return errors;
        }

        public bool HasAddressChanges
        {
            get
            {
                return addressBindManager.StateControl.CurrentMode == VerificationState.CurrentModeEnum.ChangesMode;
            }
        }
        public bool EcorrCorrespondence { get { return EcorrCorrespondenceButton.IsChecked; } set { EcorrCorrespondenceButton.IsChecked = value; } }
        public bool EcorrBilling { get { return EcorrBillingButton.IsChecked; } set { EcorrBillingButton.IsChecked = value; } }
        public bool EcorrTax { get { return EcorrTaxButton.IsChecked; } set { EcorrTaxButton.IsChecked = value; } }
        public bool AllowPoBox { get; set; }
        private MdLettersHelper mdLettersHelper;

        private void NoPhoneBox_Click(object sender, EventArgs e)
        {
            homePhoneBindManager.Enabled = otherPhoneBindManager.Enabled = otherPhone2BindManager.Enabled = !NoPhoneBox.IsChecked;
            if (NoPhoneBox.IsChecked)
            {
                HomePhoneVer.InvalidSelection();
                OtherPhoneVer.InvalidSelection();
                OtherPhone2Ver.InvalidSelection();
            }
        }

        public void RevertChanges()
        {
            //TODO verify
            //re-enable if the emailbox is not enabled
            if(!EmailBox.Enabled)
            {
                EmailBox.Enabled = true;
            }
            foreach (var manager in bindManagers)
                manager.RevertChanges(true);
            foreach (var phoneBox in new PhoneBox[] { HomePhoneBox, OtherPhoneBox, OtherPhone2Box })
                if (phoneBox.IsForeign)
                    phoneBox.Font = new System.Drawing.Font("Arial", 8);
                else
                    phoneBox.Font = new System.Drawing.Font("Arial", 10);
        }

        /// <summary>
        /// Persist all changes to the original demographics object.
        /// If a different demographics object is passed in, changes will be persisted to that object instead.
        /// </summary>
        public void PersistChanges(bool activityCodeIsAM, MDBorrowerDemographics persistTo = null)
        {
            persistTo = persistTo ?? demographics;
            foreach (var manager in bindManagers)
                manager.PersistChanges(persistTo);
            DemographicVerifications.PersistChanges(persistTo, activityCodeIsAM);
            Func<bool, string> yn = new Func<bool, string>(b => b ? "Y" : "N");
            persistTo.HomePhoneConsent = yn(HomePhoneConsentButton.IsChecked);
            persistTo.OtherPhoneConsent = yn(OtherPhoneConsentButton.IsChecked);
            persistTo.OtherPhone2Consent = yn(OtherPhone2ConsentButton.IsChecked);
            persistTo.EcorrBilling = EcorrBilling;
            persistTo.EcorrCorrespondence = EcorrCorrespondence;
            persistTo.EcorrTax = EcorrTax;

            persistTo.SSN = demographics.SSN;
        }

        public List<string> ValidateInput(AcpSelectionResult selection, bool callsMode)
        {
            List<string> errors = new List<string>();
            if (selection == null)
                return errors;

            if (!demographics.EcorrBilling && EcorrBilling)
                errors.Add("Borrower cannot be opted into E-CORR Billing from MD.");
            if (!demographics.EcorrCorrespondence && EcorrCorrespondence)
                errors.Add("Borrower cannot be opted into E-CORR Correspondence from MD.");
            if (!demographics.EcorrTax && EcorrTax)
                errors.Add("Borrower cannot be opted into E-CORR Tax from MD.");

            var validPhones = new List<Tuple<PhoneBox, ComboBox, VerificationState, string>>();
            var allPhones = new List<Tuple<PhoneBox, ComboBox, VerificationState, string>>();
            var phoneCheck = new Action<PhoneBox, ComboBox, VerificationState, string>((pb, cb, vs, s) =>
            {
                if (!pb.IsForeign)
                {
                    if (pb.PhoneNumber.Length != 0 && pb.PhoneNumber.Length != 10)
                        errors.Add(string.Format("Please complete the {0} phone number.", s));
                    if (pb.PhoneNumber.StartsWith("0") || pb.PhoneNumber.StartsWith("1"))
                        errors.Add(string.Format("{0} area code cannot start with {1}", s, pb.PhoneNumber[0]));
                }

                if ((pb.PhoneNumber.EndsWith("5551212") || pb.PhoneNumber.Trim() == "801-55512-12") && vs.Selection.IsValid())
                    errors.Add("Phone number 555-1212 is invalid.");
                if (pb.PhoneNumber.Length + pb.ForeignCountry.Length > 0 && (cb.SelectedIndex <= 0))
                    errors.Add("Please enter a " + s + " phone type.");
                var tuple = new Tuple<PhoneBox, ComboBox, VerificationState, string>(pb, cb, vs, s);
                allPhones.Add(tuple);
                if (vs.Selection.IsValid() || vs.Selection == VerificationSelection.NoChange)
                    if (!string.IsNullOrEmpty(pb.PhoneNumber + pb.Extension + pb.ForeignCountry + pb.ForeignCity + pb.ForeignLocal))
                        validPhones.Add(tuple);
            });
            phoneCheck(HomePhoneBox, HomePhoneTypeBox, HomePhoneVer, "Home");
            phoneCheck(OtherPhoneBox, OtherPhoneTypeBox, OtherPhoneVer, "Other");
            phoneCheck(OtherPhone2Box, OtherPhone2TypeBox, OtherPhone2Ver, "Other (2)");
            foreach (var phone in validPhones)
                foreach (var other in validPhones)
                    if (other != phone)
                    {
                        if (!phone.Item1.IsForeign)
                        {
                            if (phone.Item1.PhoneNumber == other.Item1.PhoneNumber)
                                if (phone.Item1.Extension == other.Item1.Extension)
                                    errors.Add(string.Format("{0} and {1} phone are identical.", phone.Item4, other.Item4));
                        }
                        else
                        {
                            if (phone.Item1.ForeignCountry == other.Item1.ForeignCountry)
                                if (phone.Item1.ForeignCity == other.Item1.ForeignCity)
                                    if (phone.Item1.ForeignLocal == other.Item1.ForeignLocal)
                                        errors.Add(string.Format("{0} and {1} phone are identical.", phone.Item4, other.Item4));
                        }
                    }
            if (AddressVer.Selection != VerificationSelection.RefusedNoChange)
            {
                if (string.IsNullOrEmpty(Address1Box.Text))
                    errors.Add("Please enter an Address.");
                if (string.IsNullOrEmpty(CityBox.Text))
                    errors.Add("Please enter a City");
                if (string.IsNullOrEmpty(ZipBox.Text))
                    errors.Add("Please enter a Zip Code");
                if (!ForeignAddressBox.IsChecked)
                {
                    if (string.IsNullOrEmpty(StateBox.Text))
                        errors.Add("Please select a State");
                    if (!(ZipBox.TextLength == 5 || ZipBox.TextLength == 9))
                        errors.Add("Zip Code must be 5 or 9 characters");
                    if (ZipBox.Text.Any(o => !char.IsNumber(o)))
                        errors.Add("Zip Code must only be numbers");
                    if (ZipBox.Text.EndsWith("0000"))
                        errors.Add("Zip Code cannot end in 0000");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ForeignCountryBox.Text))
                        errors.Add("Please enter a foreign Country");
                    else
                    {
                        if (!Country.GetCountries().Any(o => o.CountryName.ToLower() == ForeignCountryBox.Text.ToLower()))
                            errors.Add("Please enter a valid foreign Country");
                    }
                }
            }

            var emailCheck = new Action<TextBox, string>((eb, s) =>
            {
                string email = eb.Text;

                try
                {
                    if (!email.IsNullOrEmpty())
                    {
                        MailAddress m = new MailAddress(email);
                    }
                    return;
                }
                catch(FormatException e)
                {
                    errors.Add($"The {s} email provided is not a valid email address. Internal error: {e.Message}");
                    return;
                }
            });
            emailCheck(EmailBox, "Home");
            emailCheck(OtherEmailBox, "Other");
            emailCheck(OtherEmail2Box, "Other 2");

            if (!AllowPoBox)
            {
                var badBoxes = new string[] { "P.O. BOX", "PO BOX", "P O BOX", "P. O BOX", "POBOX", "P.O.BOX", "P/O BOX" };
                foreach (var bad in badBoxes)
                    if (Address1Box.Text.Contains(bad) || Address2Box.Text.Contains(bad))
                    {
                        errors.Add("This borrower has a pending disbursement and cannot have an address with a P.O. Box");
                        break;
                    }

            }

            //Get address/phone/email verification errors
            if (callsMode) //only want need verification call if the script is in calls mode
            {
                errors.AddRange(ValidateNeedsVerifications());
            }

            return errors;
        }

        /// <summary>
        /// Make the buttons on this form purple.
        /// </summary>
        public void Purplify()
        {
            Color purple = Color.FromArgb(184, 174, 231);
            foreach (Button b in this.Controls.Filter<Button>())
                b.BackColor = purple;
            foreach (var ver in bindManagers)
                ver.StateControl.Purplify();

            //Sets label backgrounds to red when they need to be validated before proceeding
            SetNeedsVerification();
        }

        public DemographicVerifications DemographicVerifications
        {
            get
            {
                return new DemographicVerifications()
                {
                    Address = AddressVer.Selection,
                    HomePhone = HomePhoneVer.Selection,
                    HomePhoneConsent = HomePhoneConsentButton.IsChecked,
                    OtherPhone = OtherPhoneVer.Selection,
                    OtherPhoneConsent = OtherPhoneConsentButton.IsChecked,
                    OtherPhone2 = OtherPhone2Ver.Selection,
                    OtherPhone2Consent = OtherPhone2ConsentButton.IsChecked,
                    Email = EmailVer.Selection,
                    OtherEmail = OtherEmailVer.Selection,
                    OtherEmail2 = OtherEmail2Ver.Selection
                };
            }
        }

        public bool IncludeRefused
        {
            get { return AddressVer.IncludeRefused; }
            set
            {
                AddressVer.IncludeRefused = HomePhoneVer.IncludeRefused = OtherPhoneVer.IncludeRefused =
                    OtherPhone2Ver.IncludeRefused = EmailVer.IncludeRefused =
                    OtherEmailVer.IncludeRefused = OtherEmail2Ver.IncludeRefused = value;
            }
        }
        public bool EnableEcorr
        {
            get { return EcorrCorrespondenceButton.Enabled; }
            set
            {
                EcorrBillingButton.Enabled = EcorrCorrespondenceButton.Enabled = EcorrTaxButton.Enabled = value;
            }
        }

        #region DataBinding
        private List<BindManager> bindManagers;
        private BindManager addressBindManager;
        private BindManager homePhoneBindManager;
        private BindManager otherPhoneBindManager;
        private BindManager otherPhone2BindManager;
        private BindManager emailBindManager;
        private BindManager otherEmailBindManager;
        private BindManager otherEmail2BindManager;
        /// <summary>
        /// Load the given demographics object without altering any current bindings.
        /// </summary>
        public void LoadWithoutBinding(MDBorrowerDemographics demographics)
        {
            foreach (var manager in bindManagers)
            {
                foreach (var control in manager.Controls)
                {
                    control.ControlSetter.Invoke(control.Control, new object[] { control.ValueGetter.Invoke(demographics, null) });
                }
            }
        }
        /// <summary>
        /// Bind the controls on the form to the given demographics object.
        /// </summary>
        public void Bind(MDBorrowerDemographics demographics, DataAccessHelper.Region region, bool regionClicked = false)
        {
            this.demographics = demographics;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            bindManagers = new List<BindManager>();

            addressBindManager = new BindManager(AddressVer, demographics, demographics.SPAddrInd == "N")
                .Add(Address1Box, Address1Label, d => d.Addr1)
                .Add(Address2Box, Address2Label, d => d.Addr2, false)
                .Add(CityBox, CityLabel, d => d.City)
                .Add(StateBox, StateLabel, d => d.State, false)
                .Add(ZipBox, ZipLabel, d => d.Zip)
                .Add(ForeignStateBox, ForeignStateLabel, d => d.ForeignState, false)
                .Add(ForeignCountryBox, ForeignCountryLabel, d => d.Country, false);
            if (!string.IsNullOrWhiteSpace(demographics.Country) || !string.IsNullOrWhiteSpace(demographics.ForeignState))
                ForeignAddressBox.IsChecked = true;
            EnableAddressControls();

            Func<string, string> nIfBlank = new Func<string, string>(s => string.IsNullOrWhiteSpace(s) ? "N" : s);
            demographics.HomePhoneConsent = nIfBlank(demographics.HomePhoneConsent);
            demographics.OtherPhoneConsent = nIfBlank(demographics.OtherPhoneConsent);
            demographics.OtherPhone2Consent = nIfBlank(demographics.OtherPhone2Consent);

            homePhoneBindManager = new BindManager(HomePhoneVer, demographics, demographics.HomePhoneValidityIndicator == "N")
                .Add(HomePhoneBox, HomePhoneLabel, c => c.PhoneNumber, d => d.HomePhoneNum, false)
                .Add(HomePhoneBox, HomePhoneLabel, c => c.Extension, d => d.HomePhoneExt, false)
                .Add(HomePhoneTypeBox, HomePhoneTypeLabel, d => d.HomePhoneMBL)
                .Add(HomePhoneBox, HomePhoneLabel, c => c.ForeignCountry, d => d.HomePhoneForeignCountry, false)
                .Add(HomePhoneBox, HomePhoneLabel, c => c.ForeignCity, d => d.HomePhoneForeignCity, false)
                .Add(HomePhoneBox, HomePhoneLabel, c => c.ForeignLocal, d => d.HomePhoneForeignLocalNumber, false)
                .Add(HomePhoneConsentButton, HomePhoneLabel, c => c.IsCheckedYN, d => d.HomePhoneConsent);
            homePhoneBindManager.RevertChanges();

            otherPhoneBindManager = new BindManager(OtherPhoneVer, demographics, demographics.OtherPhoneValidityIndicator == "N")
                .Add(OtherPhoneBox, OtherPhoneLabel, c => c.PhoneNumber, d => d.OtherPhoneNum, false)
                .Add(OtherPhoneBox, OtherPhoneLabel, c => c.Extension, d => d.OtherPhoneExt, false)
                .Add(OtherPhoneTypeBox, OtherPhoneTypeLabel, d => d.OtherPhoneMBL)
                .Add(OtherPhoneBox, OtherPhoneLabel, c => c.ForeignCountry, d => d.OtherPhoneForeignCountry, false)
                .Add(OtherPhoneBox, OtherPhoneLabel, c => c.ForeignCity, d => d.OtherPhoneForeignCity, false)
                .Add(OtherPhoneBox, OtherPhoneLabel, c => c.ForeignLocal, d => d.OtherPhoneForeignLocalNumber, false)
                .Add(OtherPhoneConsentButton, OtherPhoneLabel, c => c.IsCheckedYN, d => d.OtherPhoneConsent);
            otherPhoneBindManager.RevertChanges();

            otherPhone2BindManager = new BindManager(OtherPhone2Ver, demographics, demographics.OtherPhone2ValidityIndicator == "N")
                .Add(OtherPhone2Box, OtherPhone2Label, c => c.PhoneNumber, d => d.OtherPhone2Num, false)
                .Add(OtherPhone2Box, OtherPhone2Label, c => c.Extension, d => d.OtherPhone2Ext, false)
                .Add(OtherPhone2TypeBox, OtherPhone2TypeLabel, d => d.OtherPhone2MBL)
                .Add(OtherPhone2Box, OtherPhone2Label, c => c.ForeignCountry, d => d.OtherPhone2ForeignCountry, false)
                .Add(OtherPhone2Box, OtherPhone2Label, c => c.ForeignCity, d => d.OtherPhone2ForeignCity, false)
                .Add(OtherPhone2Box, OtherPhone2Label, c => c.ForeignLocal, d => d.OtherPhone2ForeignLocalNumber, false)
                .Add(OtherPhone2ConsentButton, OtherPhone2Label, c => c.IsCheckedYN, d => d.OtherPhone2Consent);
            otherPhone2BindManager.RevertChanges();

            emailBindManager = new BindManager(EmailVer, demographics, demographics.SPEmailInd == "N", true)
                .Add(EmailBox, EmailLabel, d => d.Email);
            otherEmailBindManager = new BindManager(OtherEmailVer, demographics, demographics.SPOtEmailInd == "N", true)
                .Add(OtherEmailBox, OtherEmailLabel, d => d.OtherEmail);
            otherEmail2BindManager = new BindManager(OtherEmail2Ver, demographics, demographics.SPOt2EmailInd == "N", true)
                .Add(OtherEmail2Box, OtherEmail2Label, d => d.OtherEmail2);

            bindManagers.AddRange(new BindManager[] { addressBindManager, homePhoneBindManager, otherPhoneBindManager, otherPhone2BindManager, emailBindManager, otherEmailBindManager, otherEmail2BindManager });

            EcorrCorrespondence = demographics.EcorrCorrespondence;
            EcorrBilling = demographics.EcorrBilling;
            EcorrTax = demographics.EcorrTax;

            if (!string.IsNullOrEmpty(demographics.SPAddrVerDt))
                LastVerifiedLabel.Text = "Last Verified " + demographics.SPAddrVerDt;
            else
                LastVerifiedLabel.Visible = false;

            FormatSettingsButton.Visible = FormatSettingsLabel.Visible = false;

            SetBindManagerComments();
            SetNeedsVerification();
        }
        #endregion

        #region Comment Generation
        public string GenerateCommentString()
        {
            List<string> comment1 = new List<string>();
            List<string> comment2 = new List<string>();
            foreach (var comment in BindManagerComments)
            {
                if (comment.BindManager.StateControl.Selection == VerificationSelection.InvalidNoChange)
                {
                    comment1.Add(comment.CommentCode + "I");
                    comment2.Add(comment.CommentDescription + " Verif Invalid");
                }
                else if (comment.BindManager.StateControl.Selection.IsValid())
                {
                    comment1.Add(comment.CommentCode + "V");
                    comment2.Add(comment.CommentDescription + " Verif");
                }
                else
                {
                    comment1.Add(comment.CommentCode + "X");
                    comment2.Add(comment.CommentDescription + " Not Verif");
                }
            }
            if (NoPhone)
            {
                comment1.Add("NP");
                comment2.Add("No Phone");
            }
            var d = demographics;
            string addr = string.Join(" ", d.Addr1, d.Addr2, d.City, d.State, d.Zip, string.Join(",", d.HomePhoneNum, d.OtherPhoneNum, d.OtherPhone2Num));
            string compiledString = string.Join(",", string.Join(",", comment1.Union(comment2)), addr) + "{MAUIDUDE}";
            return compiledString;
        }
        /// <summary>
        /// Ties a bind manager to a comment code/description combo.
        /// </summary>
        public class BindManagerComment
        {
            public BindManager BindManager { get; set; }
            public string CommentCode { get; set; }
            public string CommentDescription { get; set; }
            public BindManagerComment(BindManager bindManager, string commentCode, string commentDescription)
            {
                this.BindManager = bindManager;
                this.CommentCode = commentCode;
                this.CommentDescription = commentDescription;
            }
        }
        public List<BindManagerComment> BindManagerComments;
        /// <summary>
        /// Tie the existing Bind Managers to code/description combinations so a comment string can later be generated.
        /// </summary>
        private void SetBindManagerComments()
        {
            BindManagerComments = new List<BindManagerComment>(new BindManagerComment[] {
                new BindManagerComment(addressBindManager, "A", "Address"),
                new BindManagerComment(homePhoneBindManager, "P", "Phone"),
                new BindManagerComment(otherPhoneBindManager, "O1", "Other Phone"),
                new BindManagerComment(otherPhone2BindManager, "O2", "Other Phone 2"),
                new BindManagerComment(emailBindManager, "E", "Email"),
                new BindManagerComment(otherEmailBindManager, "OE", "Other Email"),
                new BindManagerComment(otherEmail2BindManager, "OE2", "Other Email 2")
            });

        }
        #endregion

        private void AlternateAddressButton_Click(object sender, EventArgs e)
        {
            new AlternateAddressForm(DataAccessHelper.CurrentRegion, demographics.AccountNumber).ShowDialog();
        }

        private void StateBox_Leave(object sender, EventArgs e)
        {
            StateBox.Text = StateBox.Text.ToUpper();
            if (!StateBox.Items.Cast<string>().Contains(StateBox.Text))
                StateBox.Text = "";
        }

        private void FormatSettingsButton_Cycle(object sender)
        {
            if (mdLettersHelper != null)
            {
                this.demographics.UpdatedAlternateFormat = FormatSettingsButton.SelectedValue.CorrespondenceFormat;
                this.demographics.UpdatedAlternateFormatId = FormatSettingsButton.SelectedValue.CorrespondenceFormatId;
            }
        }

        private void ForeignAddressBox_Click(object sender, EventArgs e)
        {
            EnableAddressControls();
            var revert = new Action<Control>(control =>
            {
                var boundControl = addressBindManager.Controls.Where(o => o.Control == control).Single();
                boundControl.SetControlValue(boundControl.OriginalValue);
            });
            if (ForeignAddressBox.IsChecked)
            {
                revert(StateBox);
            }
            else
            {
                revert(ForeignCountryBox);
                revert(ForeignStateBox);
            }
        }

        private void EnableAddressControls()
        {
            if (ForeignAddressBox.IsChecked)
            {
                ForeignCountryBox.Enabled = ForeignStateBox.Enabled = true;
                StateBox.Enabled = false;
                demographics.IsForeignAddress = true;
            }
            else
            {
                ForeignCountryBox.Enabled = ForeignStateBox.Enabled = false;
                StateBox.Enabled = true;
                demographics.IsForeignAddress = false;
            }
        }
    }
}