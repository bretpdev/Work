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

namespace IDRUSERPRO
{
    public partial class HouseholdInformation : UserControl
    {
        public bool SpouseRequired { get; private set; }
        public HouseholdInformation()
        {
            InitializeComponent();
        }

        int? previousFamilySize;
        public Func<bool> getRepayeOrLowest;
        public Func<bool> getBorrowerAgiReflects;
        Action onFamilySizeChecked;
        GroupBox spouseInfoGroup;
        IncomeInformation spouseIncomeInformation;
        List<PayStubs> spousePaystubs;
        ControlHelper ch = new ControlHelper();

        public void LoadControl(int? previousFamilySize, Func<bool> getRepayeOrLowest, Func<bool> getBorrowerAgiReflects, Action<bool> setSpouseInfoGroupVisible, GroupBox spouseInfoGroup, IncomeInformation spouseIncomeInformation, List<PayStubs> spousePaystubs, Action onFamilySizeChecked)
        {
            this.previousFamilySize = previousFamilySize;
            this.getRepayeOrLowest = getRepayeOrLowest;
            this.getBorrowerAgiReflects = getBorrowerAgiReflects;
            this.spouseInfoGroup = spouseInfoGroup;
            this.spouseIncomeInformation = spouseIncomeInformation;
            this.spousePaystubs = spousePaystubs;
            this.onFamilySizeChecked = onFamilySizeChecked;
        }

        private void MaritalFilingSpouse_DataChanged(object sender, EventArgs e)
        {
            SetHouseholdVisibility();
        }

        private bool FamilySizeChecked = false;
        private void SetHouseholdVisibility()
        {
            bool married = MaritalStatusCbo.Text == "Married";
            bool filingJointly = FilingStatusCbo.Text == "Married Filing Jointly";
            bool filingSeparately = FilingStatusCbo.Text == "Married Filing Separately";
            bool repayeOrLowest = getRepayeOrLowest();
            bool agiReflects = getBorrowerAgiReflects();

            bool spouseExternalLoansVisible = false;
            if (filingJointly)
                spouseExternalLoansVisible = true;
            else if (filingSeparately && repayeOrLowest)
                spouseExternalLoansVisible = true;

            SpouseExternalLoansBox.Visible = SpouseExternalLoansLabel.Visible = spouseExternalLoansVisible;
            if (!spouseExternalLoansVisible)
                SpouseExternalLoansBox.SelectedValue = null;
            bool spouseHasExternalLoans = SpouseExternalLoansBox.SelectedValue == true && ch.WouldBeVisible(SpouseExternalLoansBox);


            SpouseRequired = spouseHasExternalLoans;

            //Determine if the spouse information should be visible and required
            SpouseGroup.Visible = SpouseRequired;
            spouseInfoGroup.Visible = SpouseRequired;

            bool spouseIncomeVisible = false;
            if (filingJointly && !agiReflects)
                spouseIncomeVisible = true;
            else if (filingSeparately && repayeOrLowest)
                spouseIncomeVisible = true;
            spouseIncomeInformation.Visible = spouseIncomeVisible;
            if (!spouseIncomeVisible)
            {
                spouseIncomeInformation.ResetFields();
                spousePaystubs.Clear();
            }

            if (!SpouseRequired)
            {
                SpouseFirstName.Text = "";
                SpouseMiddleName.Text = "";
                SpouseLastName.Text = "";
                SpouseSsn.Text = "";
                SpouseDOB.Text = "";
            }

            int size = GetFamilySize().Value;
            if (!FamilySizeChecked)
            {
                if (size > previousFamilySize && (size - previousFamilySize) >= 4)
                {
                    if (Dialog.Warning.YesNo(string.Format("The borrower family size increased by more than four. Do you want to pend this application to wait for family size docs?\r\n\r\nThe family household size is currently set to {0}. If this in incorrect, please update the Household Tab", size), "Family Size Increase"))
                    {
                        FamilySizeIncreasedCbx.Visible = true;
                        FamilySizeIncreasedCbx.Checked = true;
                    }
                    else
                    {
                        FamilySizeIncreasedCbx.Visible = false;
                        FamilySizeIncreasedCbx.Checked = false;
                    }
                    onFamilySizeChecked();
                    FamilySizeChecked = true;
                }
            }
        }

        private int? GetFamilySize()
        {
            int size = 1; //Start with 1 for borrower
            size += (Children.Text.ToIntNullable() ?? 0) + (Dependents.Text.ToIntNullable() ?? 0);
            if (MaritalStatusCbo.Text.IsIn("Married", "Married, No Access"))
                ++size; //Increase 1 for spouse
            return size;
        }

        public void PerformValidation(Action<string, Control, Label> setError)
        {

            if (Children.Text.ToIntNullable() == null)
                setError("Please enter a valid number.", Children, ChildrenLabel);
            if (Dependents.Text.ToIntNullable() == null)
                setError("Please enter a valid number.", Dependents, DependentsLabel);

            if (MaritalStatusCbo.SelectedIndex <= 0)
                setError("Please select a Marital Status", MaritalStatusCbo, MaritalStatusLbl);

            if (FilingStatusCbo.SelectedIndex <= 0)
                setError("Please select a Filing Status", FilingStatusCbo, FilingStatusLbl);

            if (ch.WouldBeVisible(SpouseExternalLoansBox))
                if (SpouseExternalLoansBox.SelectedValue == null)
                    setError("Does Spouse have External Loans?", SpouseExternalLoansBox, SpouseExternalLoansLabel);

            if (SpouseRequired)
            {
                if (SpouseFirstName.Text.IsNullOrEmpty())
                    setError("First Name required.", SpouseFirstName, SpouseFirstNameLabel);
                if (SpouseLastName.Text.IsNullOrEmpty())
                    setError("Last Name required.", SpouseLastName, SpouseLastNameLabel);
                if (SpouseSsn.TextLength < 9)
                    setError("SSN must be 9 digits.", SpouseSsn, SpouseSsnLabel);
                using (var group = ch.Group(setError, SpouseDOB, SpouseDobLabel))
                {
                    group.SetErrorIf("Date of Birth is required.", dob => dob.Text.ToDateNullable() == null);
                    group.SetErrorIf("Date of Birth must be in the past.", dob => dob.Text.ToDateNullable() > DateTime.Now);
                    group.SetErrorIf($"Date of Birth must not predate the earliest allowable date of {DateTime.MinValue}.", dob => dob.Text.ToDateNullable() < DateTime.MinValue);
                }
            }
        }

        public void LoadHousehold(ApplicationData appData, SpouseData spouse)
        {
            Children.Text = appData.NumberOfChildren?.ToString() ?? "0";
            Dependents.Text = appData.NumberOfDependents?.ToString() ?? "0";
            MaritalStatusCbo.SelectedValue = appData.MaritalStatusId ?? 0;
            FilingStatusCbo.SelectedValue = appData.FilingStatusId ?? 0;

            if (appData.MaritalStatusId == 2 && appData.FilingStatusId.IsIn(2, 3))
            {
                SpouseExternalLoansBox.SelectedValue = true;
            }
            SpouseFirstName.Text = spouse.FirstName;
            SpouseMiddleName.Text = spouse.MiddleName;
            SpouseLastName.Text = spouse.LastName;
            SpouseSsn.Text = spouse.Ssn;
            SpouseDOB.Text = spouse.BirthDate?.ToString("MM/dd/yyyy");
        }

        public void SaveSpouseData(SpouseData spouse)
        {
            spouse.FirstName = SpouseFirstName.Text;
            spouse.MiddleName = SpouseMiddleName.Text;
            spouse.LastName = SpouseLastName.Text;
            spouse.Ssn = SpouseSsn.Text;
            spouse.BirthDate = SpouseDOB.Text.ToDate();
        }
    }
}

