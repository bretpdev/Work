using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public partial class IncomeInformation : UserControl
    {
        const string TwoDecimalPlaces = "0.00";
        public Func<AdoiCalculator> GetCalculator { get; private set; }
        public Func<FilingStatuses> GetFilingStatus { get; private set; }
        public Func<bool> GetAppStatusIsApproved { get; private set; }
        public Action OnAltIncomeChanged { get; private set; }
        public Action OnAgiReflectsCurrentIncomeChanged { get; private set; }
        public List<string> ValidStateCodes { get; private set; }
        public List<IncomeSource> IncomeSources { get; private set; }
        public List<FilingStatus> FilingStatuses { get; private set; }
        public List<PayStubs> PayStubs { get; private set; }
        public bool IsSpouse { get; private set; }
        bool ControlLoaded = false;
        ControlHelper ch = new ControlHelper();

        #region Accessors
        private bool? agiReflectsCurrentIncome;
        public bool? AgiReflectsCurrentIncome
        {
            get
            {
                return agiReflectsCurrentIncome;
            }
            set
            {
                agiReflectsCurrentIncome = value;
                SetAgiReflectsVisibility();
            }
        }
        private bool? taxesFiled;
        public bool? TaxesFiled
        {
            get
            {
                return taxesFiled;
            }
            set
            {
                taxesFiled = value;
                SetAgiReflectsVisibility();
            }
        }
        public bool? TaxableIncome
        {
            get
            {
                return TaxableIncomeBox.SelectedValue;
            }
            set
            {
                using (ch.TemporarilyDisableEvent(TaxableIncomeBox, Control_Changed, "SelectedValueChanged"))
                    TaxableIncomeBox.SelectedValue = value;
            }
        }
        public bool? SupportingDocsRequired
        {
            get
            {
                return SupportingDocsRequiredBox.SelectedValue;
            }
            set
            {
                using (ch.TemporarilyDisableEvent(SupportingDocsRequiredBox, Control_Changed, "SelectedValueChanged"))
                    SupportingDocsRequiredBox.SelectedValue = value;
            }
        }
        public DateTime? ReceivedDate
        {
            get
            {
                return ReceivedDateBox.Text.ToDateNullable();
            }
            set
            {
                using (ch.TemporarilyDisableEvent(ReceivedDateBox, Control_Changed, "TextChanged"))
                {
                    if (value.HasValue)
                        ReceivedDateBox.Text = value.Value.ToString("MM/dd/yyyy");
                    else
                        ReceivedDateBox.Text = "";
                }
            }
        }
        public decimal? AgiFromTaxes
        {
            get
            {
                return AgiFromTaxesBox.Text.ToDecimalNullable();
            }
            set
            {
                using (ch.TemporarilyDisableEvent(AgiFromTaxesBox, Control_Changed, "TextChanged"))
                {
                    if (value == null)
                        AgiFromTaxesBox.Text = "";
                    else
                        AgiFromTaxesBox.Text = value.Value.ToString(TwoDecimalPlaces);
                }
            }
        }
        public int? TaxYear
        {
            get
            {
                return TaxYearBox.Text.ToIntNullable();
            }
            set
            {
                using (ch.TemporarilyDisableEvent(TaxYearBox, Control_Changed, "TextChanged"))
                {
                    if (value.HasValue)
                        TaxYearBox.Text = value.Value.ToString();
                    else
                        TaxYearBox.Text = "";
                }
            }
        }
        public string StateCode
        {
            get
            {
                return StateBox.Text;
            }
            set
            {
                using (ch.TemporarilyDisableEvent(StateBox, Control_Changed, "TextChanged"))
                    StateBox.Text = value;
            }
        }

        public IncomeSource IncomeSource
        {
            get
            {
                IDRUSERPRO.IncomeSources source;
                var selection = GetSelectedAgiReflects();
                if (selection == AgiReflectsCurrentIncomeSelection.Yes)
                    source = IDRUSERPRO.IncomeSources.Taxes;
                else if (selection == AgiReflectsCurrentIncomeSelection.No || selection == AgiReflectsCurrentIncomeSelection.NoTaxes)
                    source = IDRUSERPRO.IncomeSources.AltAdoi;
                else
                    return null;

                return IncomeSources.Single(p => p.SourceId == (int)source);
            }
        }
        public int? SpouseFilingStatusId
        {
            get
            {
                if (!IsSpouse)
                    return null;
                var status = (SpouseFilingStatusBox.SelectedItem as FilingStatus);
                if (status == null || status.FilingStatusId == 0)
                    return null;
                return status.FilingStatusId;
            }
            set
            {
                if (!IsSpouse)
                    return;
                var status = FilingStatuses.SingleOrDefault(o => o.FilingStatusId == value);
                SpouseFilingStatusBox.SelectedItem = status;
            }
        }

        public bool NoSelectionsMade
        {
            get
            {
                return AgiReflectsCurrentIncomeBox.SelectedIndex <= 0
                    && TaxableIncomeBox.SelectedIndex <= 0
                    && SupportingDocsRequiredBox.SelectedIndex <= 0
                    && ReceivedDateBox.Text.ToDateNullable() == null;
            }
        }


        #endregion
        public IncomeInformation()
        {
            InitializeComponent();
        }

        public void Control_Changed(object sender, EventArgs e)
        {
            if (ControlLoaded)
                SetVisibility();
        }

        public void LoadControl(List<PayStubs> paystubs, List<string> validStatesCodes, List<IncomeSource> incomeSources, List<FilingStatus> filingStatuses, Func<AdoiCalculator> getCalculator, Func<bool> getAppStatusIsApproved, bool isSpouse, Action onAltIncomeChanged, Action onAgiReflectsCurrentIncomeChanged)
        {
            ControlLoaded = true;
            PayStubs = paystubs;
            ValidStateCodes = validStatesCodes;
            IncomeSources = incomeSources;
            GetCalculator = getCalculator;
            GetAppStatusIsApproved = getAppStatusIsApproved;
            IsSpouse = isSpouse;
            OnAltIncomeChanged = onAltIncomeChanged;
            OnAgiReflectsCurrentIncomeChanged = onAgiReflectsCurrentIncomeChanged;
            FilingStatuses = filingStatuses;
            if (IsSpouse)
            {
                TitleLabel.Text = "Spouse's Income";
                StatePanel.Visible = false;
                AltIncomeLabel.Text = "Spouse Alt Income";
                TotalAltIncomeBox.Visible = TotalAltIncomeLabel.Visible = false;
                filingStatuses.Insert(0, new FilingStatus());
                SpouseFilingStatusBox.DataSource = filingStatuses;
                SpouseFilingStatusBox.ValueMember = "FilingStatusId";
                SpouseFilingStatusBox.DisplayMember = "FilingStatusDescription";
            }
            else
                TitleLabel.Text = "Borrower's Income";
        }

        public void SetVisibility()
        {
            bool agiVisible = false;
            bool altVisible = false;
            var calc = GetCalculator();

            var agiSelection = GetSelectedAgiReflects();
            if (agiSelection == AgiReflectsCurrentIncomeSelection.Yes && TaxableIncomeBox.SelectedValue == false)
            {
                IncomeSourceBox.Text = IncomeSource.SourceDescription;
                AgiFromTaxes = 0;
                AgiFromTaxesBox.Enabled = false;
                agiVisible = true;
            }
            else if (agiSelection == AgiReflectsCurrentIncomeSelection.Yes && TaxableIncomeBox.SelectedValue == true)
            {
                IncomeSourceBox.Text = IncomeSource.SourceDescription;
                if (!AgiFromTaxesBox.Enabled && AgiFromTaxesBox.Text.ToDecimalNullable() == 0)
                    AgiFromTaxes = null;
                AgiFromTaxesBox.Enabled = true;
                agiVisible = true;
            }
            else if ((agiSelection == AgiReflectsCurrentIncomeSelection.No || agiSelection == AgiReflectsCurrentIncomeSelection.NoTaxes) && TaxableIncomeBox.SelectedValue != null)
            {
                IncomeSourceBox.Text = IncomeSources.Single(p => p.SourceId == (int)IDRUSERPRO.IncomeSources.AltAdoi).SourceDescription;
                altVisible = true;
            }
            else
                IncomeSourceBox.Text = "";


            AgiPanel.Visible = agiVisible;
            AltIncomePanel.Visible = altVisible;
            if (!ch.WouldBeVisible(AgiPanel))
            {
                AgiFromTaxes = null;
                TaxYear = null;
            }

            if (IsSpouse)
                SpouseFilingStatusPanel.Visible = GetSelectedAgiReflects() == AgiReflectsCurrentIncomeSelection.Yes;

            if (SupportingDocsRequired == false)
            {
                ReceivedDate = null;
                ReceivedDateBox.Enabled = false;
            }
            else
                ReceivedDateBox.Enabled = true;


            calc = GetCalculator();
            using (ch.TemporarilyDisableEvent(AltIncomeBox, AltIncomeBox_TextChanged))
            {
                if (!IsSpouse)
                    AltIncomeBox.Text = calc.BorrowerAltIncome.ToString(TwoDecimalPlaces);
                else
                    AltIncomeBox.Text = calc.SpouseAltIncome.ToString(TwoDecimalPlaces);
            }
            TotalAltIncomeBox.Text = calc.TotalAltIncome.ToString(TwoDecimalPlaces);

            if (!IsSpouse)
            {
                if (calc.BorrowerIncome?.IncomeSource == null)
                    CalcButton.Visible = false;
                else
                    CalcButton.Visible = calc.NeededAdoi == NeededAdois.Borrower || calc.NeededAdoi == NeededAdois.Both;
            }
            else
            {
                if (calc.SpouseIncome?.IncomeSource == null)
                    CalcButton.Visible = false;
                else
                    CalcButton.Visible = calc.NeededAdoi == NeededAdois.Spouse || calc.NeededAdoi == NeededAdois.Both;
            }
            if (!ch.WouldBeVisible(CalcButton))
            {
                PayStubs.Clear();
                calc = GetCalculator();
                if (!IsSpouse)
                    AltIncomeBox.Text = calc.BorrowerAltIncome.ToString(TwoDecimalPlaces);
                else
                    AltIncomeBox.Text = calc.SpouseAltIncome.ToString(TwoDecimalPlaces);
                TotalAltIncomeBox.Text = calc.TotalAltIncome.ToString(TwoDecimalPlaces);
            }
        }

        private void SetAgiReflectsVisibility()
        {
            using (ch.TemporarilyDisableEvent(AgiReflectsCurrentIncomeBox, AgiReflectsCurrentIncomeBox_SelectedIndexChanged))
            {
                AgiReflectsCurrentIncomeSelection? selection = null;
                if (agiReflectsCurrentIncome == true)
                    selection = AgiReflectsCurrentIncomeSelection.Yes;
                else if (agiReflectsCurrentIncome == false)
                    selection = AgiReflectsCurrentIncomeSelection.No;
                else if (agiReflectsCurrentIncome == null && taxesFiled == false)
                    selection = AgiReflectsCurrentIncomeSelection.NoTaxes;
                if (selection == null)
                    AgiReflectsCurrentIncomeBox.SelectedIndex = 0;
                else
                    AgiReflectsCurrentIncomeBox.SelectedIndex = (int)selection;
            }
        }

        public void PerformValidation(Action<string, Control, Label> setError, string appSubStatus, bool borrowerAndSpouseSigned)
        {
            var agiSelection = GetSelectedAgiReflects();
            if (agiSelection == null)
                setError("Does AGI reflect current income?", AgiReflectsCurrentIncomeBox, AgiReflectsCurrentIncomeLabel);
            if (TaxableIncomeBox.SelectedValue == null)
                setError("Was Income Taxable?", TaxableIncomeBox, TaxableIncomeLabel);
            if (SupportingDocsRequiredBox.SelectedValue == null)
                setError("Is Supporting Documentation required?", SupportingDocsRequiredBox, SupportingDocsRequiredLabel);

            var denialOther = "Denial - Other";
            var waitingForDocumentation = "Waiting for Documentation";
            var multipleAppsReceived = "Multiple Applications Received/Other Application Processed";
            string[] exemptSubStatuses = new string[] { "Missing Income Documentation", "Unable to Determine Monthly Income", multipleAppsReceived, "Borrower In-School Outside of Permitted Hold Period", waitingForDocumentation, denialOther };
            bool skipsValidation = false;
            if (appSubStatus == denialOther)
                skipsValidation = true;
            else if (appSubStatus.IsIn(exemptSubStatuses) && borrowerAndSpouseSigned)
                skipsValidation = true;
            else if (appSubStatus.IsIn(denialOther, waitingForDocumentation, multipleAppsReceived) && !borrowerAndSpouseSigned)
                skipsValidation = true;

            if (agiSelection == AgiReflectsCurrentIncomeSelection.No || agiSelection == AgiReflectsCurrentIncomeSelection.NoTaxes)
            {
                if (ch.WouldBeVisible(CalcButton))
                {
                    if (!skipsValidation)
                    {
                        if (AltIncomeBox.Text.ToDecimalNullable() == 0)
                        {
                            setError("Alternate Income required", AltIncomeBox, AltIncomeLabel);
                            setError("Alternate Income required", CalcButton, AltIncomeLabel);
                        }
                        if (!IsSpouse)
                            if (TotalAltIncomeBox.Text.ToDecimalNullable() > 9999999)
                                setError("Total Alt Income must be less than 7 digits.", TotalAltIncomeBox, TotalAltIncomeLabel);
                    }
                }
            }

            if (TaxableIncomeBox.SelectedValue == true)
                if (agiSelection == AgiReflectsCurrentIncomeSelection.Yes)
                {
                    using (var group = ch.Group(setError, AgiFromTaxesBox, AgiFromTaxesLabel))
                    {
                        var noInput = group.Control.Text.IsNullOrEmpty();
                        var badInput = group.Control.Text.ToDecimalNullable() == null && !noInput;
                        var moreThanTwoDecimals = group.Control.Text.Split('.').Last().Length > 2 && group.Control.Text.Contains(".");
                        var longInput = group.Control.Text.ToDecimal() > 9999999;
                        if (noInput && !skipsValidation)
                            group.SetError("AGI From Taxes required");
                        else if (badInput || moreThanTwoDecimals || longInput)
                            group.SetError("AGI From Taxes must be less than 7 digits before the decimal, and no more than two after.");
                    }
                }

            if (SupportingDocsRequiredBox.SelectedValue == true && GetAppStatusIsApproved())
            {
                if (ReceivedDateBox.Text.ToDateNullable() == null)
                    setError("Received Date Required", ReceivedDateBox, ReceivedDateLabel);
            }
            else if (!ReceivedDateBox.Text.Trim(' ', '/').IsNullOrEmpty())
            {
                if (ReceivedDateBox.Text.ToDateNullable() == null)
                    setError("Valid date required.", ReceivedDateBox, ReceivedDateLabel);
            }
            if (ReceivedDateBox.Text.ToDateNullable() < DateTime.MinValue)
                setError($"Received date cannot predate {DateTime.MinValue}.", ReceivedDateBox, ReceivedDateLabel);
            if (ReceivedDateBox.Text.ToDateNullable() > DateTime.Now)
                setError("Received Date cannot be in the future.", ReceivedDateBox, ReceivedDateLabel);
            if (ch.WouldBeVisible(AgiPanel))
            {
                using (var group = ch.Group(setError, TaxYearBox, TaxYearLabel))
                {
                    group.SetErrorIf("Tax Year is required", tb => tb.TextLength == 0 && !skipsValidation);
                    group.SetErrorIf("Tax Year must be 4 digits long.", tb => tb.TextLength < 4 && tb.TextLength > 0);
                    group.SetErrorIf("Tax Year must be less than " + DateTime.Now.Year + ".", tb => tb.Text.ToIntNullable() >= DateTime.Now.Year);
                }
            }
            if (!IsSpouse)
                using (var group = ch.Group(setError, StateBox, StateLabel))
                {
                    group.SetErrorIf("State Code must be 2 characters long.", tb => tb.TextLength < 2);
                    if (StateBox.TextLength == 2)
                    {
                        var validCodes = ValidStateCodes.ToList();
                        var matches = validCodes.Contains(StateBox.Text);
                        if (!matches)
                        {
                            string message = "Must be a valid US State code.";
                            var similarCodes = validCodes.Where(o => o[0] == StateBox.Text[0] || o[1] == StateBox.Text[1]);
                            if (similarCodes.Any())
                                message += " Did you mean one of these codes?  " + string.Join(",", similarCodes);
                            group.SetErrorIf(message, tb => !validCodes.Contains(tb.Text));
                        }
                    }
                }
            if (ch.WouldBeVisible(SpouseFilingStatusPanel) && IsSpouse)
                if (SpouseFilingStatusId == null)
                    setError("Spouse Filing Status required", SpouseFilingStatusBox, SpouseFilingStatusLabel);

        }

        private void CalcButton_Click(object sender, EventArgs e)
        {
            new PaystubsForm(PayStubs).ShowDialog();
            SetVisibility();
            OnAltIncomeChanged();
        }

        public AdoiIncomeInput GetAdoiIncomeInput()
        {
            if (!ControlLoaded || IncomeSource == null)
                return null;
            var stubs = PayStubs;
            if (!ch.WouldBeVisible(AltIncomePanel) || !ch.WouldBeVisible(CalcButton))
                stubs = new List<IDRUSERPRO.PayStubs>();
            var borrowerInfo = new AdoiIncomeInput()
            {
                IncomeSource = (IncomeSources)IncomeSource.SourceId,
                TaxableIncome = TaxableIncome,
                AgiReflectsCurrentIncome = AgiReflectsCurrentIncome,
                ReceivedDate = ReceivedDate,
                SupportingDocsRequired = SupportingDocsRequired == true,
                Paystubs = stubs
            };
            return borrowerInfo;
        }

        private void AltIncomeBox_TextChanged(object sender, EventArgs e)
        {
            OnAltIncomeChanged();
        }

        private void AgiReflectsCurrentIncomeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetFields(false);
            var selection = GetSelectedAgiReflects();
            if (selection == AgiReflectsCurrentIncomeSelection.Yes)
            {
                agiReflectsCurrentIncome = true;
                taxesFiled = true;
            }
            else if (selection == AgiReflectsCurrentIncomeSelection.No)
            {
                agiReflectsCurrentIncome = false;
                taxesFiled = true;
            }
            else if (selection == AgiReflectsCurrentIncomeSelection.NoTaxes)
            {
                agiReflectsCurrentIncome = null;
                taxesFiled = false;
            }
            else
            {
                agiReflectsCurrentIncome = null;
                taxesFiled = null;
            }
            OnAgiReflectsCurrentIncomeChanged?.Invoke();
            SetVisibility();
        }

        public enum AgiReflectsCurrentIncomeSelection
        {
            Yes = 1,
            No = 2,
            NoTaxes = 3
        }

        public AgiReflectsCurrentIncomeSelection? GetSelectedAgiReflects()
        {
            if (AgiReflectsCurrentIncomeBox.SelectedIndex <= 0)
                return null;
            return (AgiReflectsCurrentIncomeSelection)AgiReflectsCurrentIncomeBox.SelectedIndex;
        }

        public void ResetFields(bool includeArci = true)
        {
            if (includeArci)
                AgiReflectsCurrentIncome = null;
            TaxableIncome = null;
            SupportingDocsRequired = null;
            ReceivedDate = null;
            AgiFromTaxes = null;
            TaxYear = null;
            StateCode = null;
            SpouseFilingStatusBox.SelectedIndex = -1;
            OnAltIncomeChanged?.Invoke();
        }

        private void AgiFromTaxesBox_Leave(object sender, EventArgs e)
        {
            if (AgiFromTaxesBox.Enabled && AgiFromTaxesBox.Text.ToDecimalNullable() == 0)
            {
                AgiFromTaxes = null;
                SetVisibility();
            }
        }
    }
}
